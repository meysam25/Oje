using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum UserLoginLogoutLogType: byte
    {
        [Display(Name = "ورود با کلمه عبور")]
        LoginWithPassword = 1,
        [Display(Name = "ورود با کد پیامکی")]
        LoginWithPhoneNumber = 2,
        [Display(Name = "ورود با تغییر کلمه عبور")]
        LoginWithChangePassword = 3,
        [Display(Name = "خروج")]
        Logout = 4
    }
}
