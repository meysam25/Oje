using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Oje.Infrastructure;
using Oje.Sanab.Interfaces;
using Oje.Sanab.Models.View;

namespace Oje.Sanab.Services
{
    public class CarInquiry : ICarInquiry
    {
        readonly IHttpClientFactory HttpClientFactory = null;
        public CarInquiry(
                IHttpClientFactory HttpClientFactory
            )
        {
            this.HttpClientFactory = HttpClientFactory;
        }

        /// <summary>
        /// تخفیفات خودرو
        /// </summary>
        /// <param name="token">توکن</param>
        /// <param name="PlkSrl">شماره سریال پلاک خودرو</param>
        /// <param name="Plk1">دو رقم سمت چپ پلاک خودرو</param>
        /// <param name="Plk2">کد حرف وسط</param>
        /// <param name="Plk3">سه رقم سمت راست پلاک</param>
        /// <param name="NationalCode">کد ملی مالک</param>
        public async Task<CarDiscountResultVM> CarDiscount(string token, int PlkSrl, int Plk1, int Plk2, int Plk3, string NationalCode)
        {
            CarDiscountResultVM result = null;

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, GlobalConfig.Configuration["SanabConfig:baseUrl"] + "/cii/test/v2/IIXGetCarAndDis/2/api/CarAndDiscountInquiry/IIXGetCarAndDiscountInquiry?PlkSrl=" + PlkSrl + "&Plk1=" + Plk1 + "&Plk2=" + Plk2 + "&Plk3=" + Plk3 + "&NationalCode=" + NationalCode)
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
                result = JsonConvert.DeserializeObject<CarDiscountResultVM>(contentStream);
            }

            return result;
        }
    }
}
