using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Oje.Infrastructure;
using Oje.Sanab.Interfaces;
using Oje.Sanab.Models.View;
using System.Text;

namespace Oje.Sanab.Services
{
    public class UserInquiry : IUserInquiry
    {
        readonly IHttpClientFactory HttpClientFactory = null;
        public UserInquiry
            (
                IHttpClientFactory HttpClientFactory
            )
        {
            this.HttpClientFactory = HttpClientFactory;
        }

        public async Task<UserResultVM> GetUserInfo(string token, string NationalCode, string Mobile, string birthdate)
        {
            UserResultVM result = null;

            using (var stringContent = new StringContent(JsonConvert.SerializeObject(new { NationalCode = NationalCode, Mobile = Mobile, birthdate = birthdate }, Formatting.Indented), Encoding.UTF8, "application/json"))
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, GlobalConfig.Configuration["SanabConfig:baseUrl"] + "/cii/test/v2/2/api/UserAuthentication/IIXGetUserAuthentication")
                {
                    Headers =
                    {
                        { HeaderNames.Authorization, "Bearer " + token}
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

        public async Task<DriverLicenceResultVM> GetDriverLicence(string token, string LicenseNumber, string NationalId)
        {
            DriverLicenceResultVM result = null;

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, GlobalConfig.Configuration["SanabConfig:baseUrl"] + "/cii/test/IIXGetLicenseV2/v2/2/api/LicenseV2/IIXGetLicenseV2?LicenseNumber=" + LicenseNumber + "&NationalId=" + NationalId)
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
                result = JsonConvert.DeserializeObject<DriverLicenceResultVM>(contentStream);
            }

            return result;
        }
    }
}
