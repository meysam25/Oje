using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Oje.Infrastructure.Services;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.View;
using System.Net.Http.Json;

namespace Oje.Sms.Services
{
    public class IdePardazanSmsSenderService : IIdePardazanSmsSenderService
    {
        readonly string SMSProviderSendApiUrl = "https://api.sms.ir";
        readonly IHttpClientFactory HttpClientFactory = null;
        public IdePardazanSmsSenderService(IHttpClientFactory HttpClientFactory)
        {
            this.HttpClientFactory = HttpClientFactory;
        }

        public async Task<SmsResult> Send(Models.DB.SmsConfig config, string destinalNumber, string message)
        {
            SmsResult validationResult = validationSend(config, destinalNumber, message);
            if (validationResult != null)
                return validationResult;

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, SMSProviderSendApiUrl + "/v1/send/bulk")
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/json" },
                    { HeaderNames.UserAgent, "OjeClient" },
                    { "X-API-KEY", config.Password.Decrypt() }
                }
            };
            httpRequestMessage.Content = JsonContent.Create(new { lineNumber = config.PhoneNumber, messageText = message, mobiles = new List<string>() { destinalNumber } });
            var httpResponseMessage = await HttpClientFactory.CreateClient().SendAsync(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                var successResult = JsonConvert.DeserializeObject<IdepardazanApiSuccessResult>(contentStream);
                if (successResult.status == 1 && successResult.data != null && !string.IsNullOrEmpty(successResult.data.packId) && successResult.data.messageIds != null && successResult.data.messageIds.Count == 1)
                    return new SmsResult() { isSuccess = true, message = "پیام با موفقیت فرستاده شد " + successResult.data.packId  , traceCode = successResult.data.messageIds.FirstOrDefault() + "", cId = config?.Id };
            }
            else
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                var errorResult = JsonConvert.DeserializeObject<IdepardazanApiErrorResult>(contentStream);

                return new SmsResult() { isSuccess = false, message = errorResult.message, cId = config?.Id };
            }

            return new SmsResult() { isSuccess = false, message = "خطای نا مشخص در ارسال پیامک", cId = config?.Id };
        }

        private SmsResult validationSend(Models.DB.SmsConfig config, string destinalNumber, string message)
        {
            if (config == null)
                return new SmsResult { isSuccess = false, message = "تنظیمات یافت نشد", cId = config?.Id };
            if (config.Type != Infrastructure.Enums.SmsConfigType.IdePardazan)
                return new SmsResult { isSuccess = false, message = "تنظیمات اشتباه می باشد", cId = config?.Id };
            if (string.IsNullOrEmpty(destinalNumber))
                return new SmsResult { isSuccess = false, message = "شماره مقصد خالی می باشد", cId = config?.Id };
            if (!destinalNumber.IsMobile())
                return new SmsResult { isSuccess = false, message = "شماره مقصد صحیح نمی باشد", cId = config?.Id };
            if (string.IsNullOrEmpty(message))
                return new SmsResult { isSuccess = false, message = "متن پیام نمی تواند خالی باشد", cId = config?.Id };
            if (message.Length > 4000)
                return new SmsResult { isSuccess = false, message = "متن پیام طولانی می باشد", cId = config?.Id };
            if (string.IsNullOrEmpty(config.PhoneNumber))
                return new SmsResult { isSuccess = false, message = "شماره مقصد خالی می باشد", cId = config?.Id };
            if (string.IsNullOrEmpty(config.Password))
                return new SmsResult { isSuccess = false, message = "کلمه عبور نمی تواند خالی باشد", cId = config?.Id };

            return null;
        }
    }
}
