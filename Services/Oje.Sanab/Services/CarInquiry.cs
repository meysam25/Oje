using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Sanab.Interfaces;
using Oje.Sanab.Models.View;
using System.Text;

namespace Oje.Sanab.Services
{
    public class CarInquiry : ICarInquiry
    {
        readonly IHttpClientFactory HttpClientFactory = null;
        readonly ISanabLoginService SanabLoginService = null;
        public CarInquiry(
                IHttpClientFactory HttpClientFactory,
                ISanabLoginService SanabLoginService
            )
        {
            this.HttpClientFactory = HttpClientFactory;
            this.SanabLoginService = SanabLoginService;
        }

        /// <summary>
        /// تخفیفات خودرو
        /// </summary>
        /// <param name="PlkSrl">شماره سریال پلاک خودرو</param>
        /// <param name="Plk1">دو رقم سمت چپ پلاک خودرو</param>
        /// <param name="Plk2">کد حرف وسط</param>
        /// <param name="Plk3">سه رقم سمت راست پلاک</param>
        /// <param name="NationalCode">کد ملی مالک</param>
        public async Task<CarDiscountResultVM> CarDiscount(int? siteSettingId, string PlkSrl, string Plk1, string Plk2, string Plk3, string NationalCode)
        {
            CarDiscountResultVM result = null;

            var switchToken = await SanabLoginService.GenerateAccessToken(siteSettingId);
            var loginToke = await SanabLoginService.LoginAsync(siteSettingId);

            if (string.IsNullOrEmpty(switchToken) || string.IsNullOrEmpty(loginToke))
                throw BException.GenerateNewException(BMessages.Sanab_Empty_Token);

            using (var stringContent = new StringContent(JsonConvert.SerializeObject(new 
            { 
                NationalCode, 
                plk1 = Plk1, 
                plk2 = Plk2, 
                plk3 = Plk3, 
                plkSrl = PlkSrl
            }, Formatting.Indented), Encoding.UTF8, "application/json"))
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, GlobalConfig.Configuration["SanabConfig:baseUrl"] + ":8243/cii/pro/CAD/1/api/CarAndDiscountInquiry/IIXGetCarAndDiscountInquiry")
                {
                    Headers =
                    {
                        { "token", "Bearer " + switchToken},
                        { HeaderNames.Authorization, "Bearer " + loginToke}
                    },
                    Content = stringContent
                };
                var httpClient = HttpClientFactory.CreateClient("unTruset");
                
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<CarDiscountResultVM>(contentStream);
                    if (result != null && string.IsNullOrEmpty(result.Vin))
                        result = null;
                }

            }

            return result;
        }
    }
}
