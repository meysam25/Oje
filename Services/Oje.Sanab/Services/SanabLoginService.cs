using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Sanab.Interfaces;
using Oje.Sanab.Models.View.Api;
using System.Text;

namespace Oje.Sanab.Services
{
    public class SanabLoginService : ISanabLoginService
    {
        readonly ISanabUserService SanabUserService = null;
        readonly IHttpClientFactory HttpClientFactory = null;
        static SwitchTokenSuccessResult lastSwitchToken = null;
        static SanabLoginResult lastSanabLogin = null;

        public SanabLoginService
            (
                ISanabUserService SanabUserService,
                IHttpClientFactory HttpClientFactory
            )
        {
            this.SanabUserService = SanabUserService;
            this.HttpClientFactory = HttpClientFactory;
        }

        public async Task<string> GenerateAccessToken(int? siteSettingId)
        {
            if (lastSwitchToken != null && string.IsNullOrEmpty(lastSwitchToken.error_description) && string.IsNullOrEmpty(lastSwitchToken.error) && !string.IsNullOrEmpty(lastSwitchToken.access_token))
                return lastSwitchToken.access_token;

            var foundUser = SanabUserService.GetActive(siteSettingId);
            if (foundUser == null)
                throw BException.GenerateNewException(BMessages.No_Config_For_Sanab);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, GlobalConfig.Configuration["SanabConfig:baseUrl"] + "/token?grant_type=client_credentials")
            {
                Headers =
                {
                    { HeaderNames.Authorization, "Basic " + foundUser.Token}
                }
            };

            var httpResponseMessage = await HttpClientFactory.CreateClient().SendAsync(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                try
                {
                    lastSwitchToken = JsonConvert.DeserializeObject<SwitchTokenSuccessResult>(contentStream);
                    if (lastSwitchToken != null && string.IsNullOrEmpty(lastSwitchToken.error_description) && string.IsNullOrEmpty(lastSwitchToken.error) && !string.IsNullOrEmpty(lastSwitchToken.access_token))
                        return lastSwitchToken.access_token;
                }
                catch
                {
                    throw BException.GenerateNewException(BMessages.Sanab_Miss_JsonResult);
                }
            }

            throw BException.GenerateNewException(BMessages.Sanab_Global_Error);
        }

        public async Task<string> LoginAsync(int? siteSettingId)
        {
            if (lastSanabLogin != null && lastSanabLogin.IsSucceed == true && !string.IsNullOrEmpty(lastSanabLogin.UserId) && !string.IsNullOrEmpty(lastSanabLogin.Token))
                return lastSanabLogin.Token;


            string switchAccessTokenStr = await GenerateAccessToken(siteSettingId);
            if (string.IsNullOrEmpty(switchAccessTokenStr))
                throw BException.GenerateNewException(BMessages.Sanab_Empty_Token);

            var foundUserAndPassword = SanabUserService.GetActive(siteSettingId);
            if (foundUserAndPassword == null || string.IsNullOrEmpty(foundUserAndPassword.Password) || string.IsNullOrEmpty(foundUserAndPassword.Username))
                throw BException.GenerateNewException(BMessages.Please_Enter_Username);



            using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, GlobalConfig.Configuration["SanabConfig:baseUrl"] + "/cii/test/IIXGetUseAuth/1/api/security/login")
            {
                Headers =
                {
                    { HeaderNames.Authorization, "Bearer " + switchAccessTokenStr}
                }
            })
            {
                using (var requestContent = new MultipartFormDataContent())
                {
                    requestContent.Add(new StringContent(foundUserAndPassword.Username, Encoding.UTF8), "UserName");
                    requestContent.Add(new StringContent(foundUserAndPassword.Password, Encoding.UTF8), "Password");

                    httpRequestMessage.Content = requestContent;
                    using (var client = HttpClientFactory.CreateClient())
                    {
                        client.BaseAddress = new Uri(GlobalConfig.Configuration["SanabConfig:baseUrl"]);
                        HttpResponseMessage clientResult = await client.SendAsync(httpRequestMessage);
                        if (clientResult.IsSuccessStatusCode)
                        {
                            var temp = await clientResult.Content.ReadAsStringAsync();
                            try
                            {
                                lastSanabLogin = JsonConvert.DeserializeObject<SanabLoginResult>(temp);
                                if (lastSanabLogin != null && lastSanabLogin.IsSucceed == true && !string.IsNullOrEmpty(lastSanabLogin.UserId) && !string.IsNullOrEmpty(lastSanabLogin.Token))
                                    return lastSanabLogin.Token;
                            }
                            catch
                            {
                                throw BException.GenerateNewException(BMessages.Sanab_Miss_JsonResult);
                            }
                        }
                        else
                            throw BException.GenerateNewException(BMessages.Sanab_Global_Error);
                    }

                    throw BException.GenerateNewException(BMessages.Sanab_Global_Error);
                }
            };
        }
    }
}
