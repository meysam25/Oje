using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Sanab.Interfaces;
using Oje.Sanab.Models.View;
using System.Text;

namespace Oje.Sanab.Services
{
    public class UserInquiry : IUserInquiry
    {
        readonly IHttpClientFactory HttpClientFactory = null;
        readonly ISanabLoginService SanabLoginService = null;

        public UserInquiry
            (
                IHttpClientFactory HttpClientFactory,
                ISanabLoginService SanabLoginService
            )
        {
            this.HttpClientFactory = HttpClientFactory;
            this.SanabLoginService = SanabLoginService;
        }

        public async Task<UserResultVM> GetUserInfo(int? siteSettingId , string NationalCode, string Mobile, string birthdate)
        {
            var switchToken = await SanabLoginService.GenerateAccessToken(siteSettingId);
            var loginToke = await SanabLoginService.LoginAsync(siteSettingId);

            if (string.IsNullOrEmpty(switchToken) || string.IsNullOrEmpty(loginToke))
                throw BException.GenerateNewException(BMessages.Sanab_Empty_Token);

            UserResultVM result = null;

            using (var stringContent = new StringContent(JsonConvert.SerializeObject(new { NationalCode = NationalCode, Mobile = Mobile, birthdate = birthdate }, Formatting.Indented), Encoding.UTF8, "application/json"))
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, GlobalConfig.Configuration["SanabConfig:baseUrl"] + "/cii/test/v2/2/api/UserAuthentication/IIXGetUserAuthentication")
                {
                    Headers =
                    {
                        { "token", "Bearer " + switchToken},
                        { HeaderNames.Authorization, "Bearer " + loginToke}
                    },
                    Content = stringContent
                };
                var httpResponseMessage = await HttpClientFactory.CreateClient().SendAsync(httpRequestMessage);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<UserResultVM>(contentStream);
                }

                return result;
            }
        }

        public async Task<DriverLicenceResultVM> GetDriverLicence(int? siteSettingId, string LicenseNumber, string NationalId)
        {
            var switchToken = await SanabLoginService.GenerateAccessToken(siteSettingId);
            var loginToke = await SanabLoginService.LoginAsync(siteSettingId);

            if (string.IsNullOrEmpty(switchToken) || string.IsNullOrEmpty(loginToke))
                throw BException.GenerateNewException(BMessages.Sanab_Empty_Token);

            DriverLicenceResultVM result = null;

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, GlobalConfig.Configuration["SanabConfig:baseUrl"] + "/cii/test/GL/1/api/LicenseV/IIXGetLicenseV2?LicenseNumber=" + LicenseNumber + "&NationalId=" + NationalId)
            {
                Headers =
                    {
                        { "token", "Bearer " + switchToken},
                        { HeaderNames.Authorization, "Bearer " + loginToke}
                    }
            };
            var httpResponseMessage = await HttpClientFactory.CreateClient().SendAsync(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<DriverLicenceResultVM>(contentStream);
            }

            return result;
        }
    }
}
