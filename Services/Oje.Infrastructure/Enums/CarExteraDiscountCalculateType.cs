using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum CarExteraDiscountCalculateType
    {
        [Display(Name = "مبلغ وارد شده توسط کاربر")]
        OnUserInputPrice = 1,
        [Display(Name = "بعد محاسبه نرخ اعمال شود")]
        OnFirstResult = 2,
        [Display(Name = "فقط رویه پایه و کهنگی ساخت")]
        OnBaseAndCreateYear = 3,
        [Display(Name = "ارزش لوازم جانبی")]
        OnUserInputAssessory = 4,
        [Display(Name = "فقط رویه پایه و کهنگی ساخت و عدم خسارت")]
        OnBaseAndCreateYearAndNoDamageDiscount = 5
    }
}
