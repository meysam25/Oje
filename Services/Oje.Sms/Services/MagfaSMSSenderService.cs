using Microsoft.Net.Http.Headers;
using Oje.Infrastructure.Services;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.View;
using System.Text;
using System.Web;

namespace Oje.Sms.Services
{
    public class MagfaSMSSenderService : IMagfaSMSSenderService
    {
        readonly string SMSProviderSendApiUrl = "https://sms.magfa.com/magfaHttpService";
        readonly Dictionary<string, string> errors = new Dictionary<string, string>
        {
            { "1", "شماره گیرنده (recipientNumber (نامعتبر (خالی) است. (بررسی مجدد فرمت شماره گیرنده در توضیح متد Enqueue" },
            { "2", "شماره فرستنده (senderNumber (نامعتبر (خالی) است. (بررسی مجدد فرمت شماره فرستنده در توضیح متد Enqueue" },
            { "3", "پارامتر encoding نامعتبراست. (بررسی صحت و همخوانی متن پیامک با encoding انتخابی" },
            { "4", "پارامتر mclass نامعتبر است. (بررسی صحت پارامتر mclass در توضیح متد Enqueue (" },
            { "6", "پارامتر UDH نامعتبر است. (بررسی UDH تنظیم شده ودریافت راهنماي مربوطه از واحد فروش سیستمهاي ارتباطی)" },
            { "10", "پارامتر Priority نامعتبر است. (تنها اعداد براي این پارامتر معتبر تلقی می شوند)" },
            { "13", "پیامک (ترکیب UDH و messageBody (خالی است. (بررسی مجدد اندازه متن پیامک و پارامتر UDH (" },
            { "14", "حساب اعتبار ریالی مورد نیاز را دارا نمیباشد. (افزایش اعتباراز طریق واحد فروش و یا از طریق سیستم Online انجام میشود)" },
            { "15", "سرور در هنگام ارسال پیام مشغول برطرف نمودن ایراد داخلی بوده است. پیامها دوباره ارسال شوند. (ارسال مجدد درخواست)" },
            { "16", "حساب غیر فعال میباشد. (تماس با واحد فروش سیستمهاي ارتباطی)" },
            { "17", "حساب منقضی شده است. (تماس با واحد فروش سیستمهاي ارتباطی)" },
            { "18", "نام کاربر و یا کلمه عبور نامعتبر است. (بررسی مجدد نام کاربري و کلمه عبور) "  },
            { "19", "درخواست معتبر نمیباشد. (ترکیب نام کاربري، رمز عبور و دامنه اشتباه است. تماس با واحد فروش براي دریافت کلمه عبور جدید)" },
            { "20", "شماره اختصاصی با نام کاربر حساب تطبیق ندارد." },
            { "22", "نوع سرویس درخواستی نامعتبر است. (سرویس اختصاص داده شده به حساب با نوع درخواست همخوانی ندارد." },
            { "23", "به دلیل ترافیک بالا، سرور آمادگی دریافت پیام جدید را ندارد. دوباره سعی کنید. (ارسال مجدد درخواست)" },
            { "24", "شناسه پیامک معتبر نمیباشد. (ممکن است شناسه پیامک اشتباه و یا متعلق به پیامکی باشد که بیش از یک روز از ارسال آن گذشته)" },
            { "25", "نام سرویس درخواستی معتبر نمیباشد. (بررسی نگارش نام متد با توجه به بخش 1 در این راهنما)" },
            { "27", "شماره گیرنده در لیست غیرفعال شرکت همراه اول قرار دارد. (ارسال پیامک هاي تبلیغاتی براي این شماره امکان پذیر نیست)" },
            { "28", "شماره گیرنده بر اساس پیش شماره در حال حاضر در مگفا مسدود می باشد" },
            { "29", "این آدرس IP اجازه دسترسی به سرویس مورد نظر را ندارد." },
            { "30", "تعداد بخشهاي پیامک بیش از حد مجاز استاندارد (256 (می باشد." },
            { "101", "طول آرایه پارامتر messageBodies با طول آرایه گیرندگان تطابق ندارد" },
            { "102", "طول آرایه پارامتر messageClass با طول آرایه گیرندگان تطابق ندارد" },
            { "103", "طول آرایه پارامتر senderNumbers با طول آرایه گیرندگان تطابق ندارد" },
            { "104", "طول آرایه پارامتر udhs با طول آرایه گیرندگان تطابق ندارد." },
            { "105", "طول آرایه پارامتر priorities با طول آرایه گیرندگان تطابق ندارد." },
            { "106", "آرایهي گیرندگان خالی میباشد." },
            { "107", "طول آرایه پارامتر گیرندگان بیشتر از طول مجاز است." },
            { "108", "آرایه ي فرستندگان خالی میباشد." },
            { "109", "طول آرایه پارامتر encoding با طول آرایه گیرندگان تطابق ندارد." },
            { "110", "طول آرایه پارامتر checkingMessageIds با طول آرایه گیرندگان تطابق ندارد."  }
        };
        readonly IHttpClientFactory HttpClientFactory = null;
        public MagfaSMSSenderService(IHttpClientFactory HttpClientFactory)
        {
            this.HttpClientFactory = HttpClientFactory;
        }

        public async Task<SmsResult> Send(Models.DB.SmsConfig config, string destinalNumber, string message)
        {
            SmsResult validationResult = validationSend(config, destinalNumber, message);
            if (validationResult != null)
                return validationResult;

            string parameters = getParameters(config, destinalNumber, message);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, SMSProviderSendApiUrl + parameters)
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/json" },
                    { HeaderNames.UserAgent, "OjeClient" }
                }
            };
            var httpResponseMessage = await HttpClientFactory.CreateClient().SendAsync(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                if (errors.Keys.Any(t => t == contentStream))
                    return new SmsResult() { isSuccess = false, message = errors[contentStream], cId = config?.Id };
                return new SmsResult() { isSuccess = true, message = "پیام با موفقیت فرستاده شد", traceCode = contentStream, cId = config?.Id };
            }

            return new SmsResult() { isSuccess = false, message = "خطای نا مشخص در ارسال پیامک", cId = config?.Id };
        }

        private string getParameters(Models.DB.SmsConfig config, string destinalNumber, string message)
        {
            StringBuilder data = new StringBuilder();
            data.Append("?service=enqueue");
            data.Append("&username=" + config.Username); //نام کاربري(درهنگام ایجاد حساب در اختیار کاربر قرار داده می شود) 
            data.Append("&password=" + config.Password.Decrypt()); // رمز عبور(درهنگام ایجاد حساب در اختیار کاربر قرار داده می شود) 
            data.Append("&from=" + config.PhoneNumber); // شماره فرستنده (شماره اختصاصی یا همان 3000xxxx(
            data.Append("&to=" + destinalNumber); // شماره/شماره هاي گیرنده/گیرندگان(موبایل مقصد)
            data.Append("&domain=" + config.Domain); //دامنه(درهنگام ایجاد حساب در اختیار کاربر قرار داده می شود) 
            data.Append("&message=" + HttpUtility.UrlEncode(message)); // متن پیامک

            return data.ToString();
        }

        private SmsResult validationSend(Models.DB.SmsConfig config, string destinalNumber, string message)
        {
            if (config == null)
                return new SmsResult { isSuccess = false, message = "تنظیمات یافت نشد", cId = config?.Id };
            if (config.Type != Infrastructure.Enums.SmsConfigType.Magfa)
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
            if (!config.PhoneNumber.StartsWith("3000") && !config.PhoneNumber.StartsWith("983000") && !config.PhoneNumber.StartsWith("+983000"))
                return new SmsResult { isSuccess = false, message = "شماره فرسنده اشتباه می باشد", cId = config?.Id };
            if (string.IsNullOrEmpty(config.Username))
                return new SmsResult { isSuccess = false, message = "نام کاربری نمی تواند خالی باشد", cId = config?.Id };
            if (string.IsNullOrEmpty(config.Password))
                return new SmsResult { isSuccess = false, message = "کلمه عبور نمی تواند خالی باشد", cId = config?.Id };
            if (string.IsNullOrEmpty(config.Domain))
                return new SmsResult() { isSuccess = false, message = "دامین نمی تواند خالی باشد", cId = config?.Id };

            return null;
        }
    }
}
