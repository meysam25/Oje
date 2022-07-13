using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum BankAccountType
    {
        [Display(Name = "سیزپی")]
        sizpay = 1,
        [Display(Name = "تی تک")]
        titec = 2,
        [Display(Name = "نامشخص")]
        unknown = 999
    }
}
