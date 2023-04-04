using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum SmsValidationHistoryType
    {
        [Display(Name = "ثبت نام یا لاگین")]
        RegisterLogin = 1,
        [Display(Name = "فراموشی کلمه عبور")]
        ForgetPassword = 2,
        [Display(Name = "ورود برای ثبت خسارت")]
        LoginWithSmsForContract = 3,
        [Display(Name = "کد تایید جهت ایجاد تفاهم نامه")]
        SMSForCreateContract = 4
    }
}
