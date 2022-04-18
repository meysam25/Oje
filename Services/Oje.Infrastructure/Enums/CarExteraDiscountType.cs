using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum CarExteraDiscountType
    {
        [Display(Name = "مبلغ")]
        Price = 1,
        [Display(Name = "تاریخ")]
        Date = 2
    }
}
