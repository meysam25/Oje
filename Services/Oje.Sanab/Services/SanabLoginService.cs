using Microsoft.Net.Http.Headers;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Sanab.Interfaces;
using System.Text;

namespace Oje.Sanab.Services
{
    public class SanabLoginService : ISanabLoginService
    {
        readonly ISanabUserService SanabUserService = null;
        readonly IHttpClientFactory HttpClientFactory = null;
        public SanabLoginService
            (
                ISanabUserService SanabUserService,
                IHttpClientFactory HttpClientFactory
            )
        {
            this.SanabUserService = SanabUserService;
            this.HttpClientFactory = HttpClientFactory;
        }

        public async Task<string> GenerateAccessToken(string token)
        {
            string result = "";

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, GlobalConfig.Configuration["SanabConfig:baseUrl"] + "/token?grant_type=client_credentials")
            {
                Headers =
                {
                    { HeaderNames.Authorization, "Bearer " + token}
                }
            };
            var httpResponseMessage = await HttpClientFactory.CreateClient().SendAsync(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                result = contentStream;
            }

            return result;
        }

        public async Task<string> LoginAsync(int? siteSettingId)
        {
            string result = "";

            var foundUserAndPassword = SanabUserService.GetActive(siteSettingId);
            if (foundUserAndPassword == null || string.IsNullOrEmpty(foundUserAndPassword.Password) || string.IsNullOrEmpty(foundUserAndPassword.Username))
                throw BException.GenerateNewException(BMessages.Please_Enter_Username);

            using (var stringContent = new StringContent("UserName=" + foundUserAndPassword.Username + "&Password=" + foundUserAndPassword.Password, Encoding.UTF8, "multipart/form-data"))
            using (var client = HttpClientFactory.CreateClient())
            {
                client.BaseAddress = new Uri(GlobalConfig.Configuration["SanabConfig:baseUrl"]);
                HttpResponseMessage clientResult = await client.PostAsync("/cii/test/IIXGetUseAuth/1/api/security/login", stringContent);
                var temp = await clientResult.Content.ReadAsStringAsync();

                //var confirmResult = JsonConvert.DeserializeObject<SizPayConfirmPaymentPostResult>(temp);
                if (clientResult.IsSuccessStatusCode)
                    result = temp;
                else
                    throw BException.GenerateNewException(BMessages.UnknownError);
            }


            return result;
        }
    }
}
