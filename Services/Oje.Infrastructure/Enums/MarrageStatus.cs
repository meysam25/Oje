using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum MarrageStatus : byte
    {
        [Display(Name = "نامشخص")]
        Unknown = 0,
        [Display(Name = "مجرد")]
        Single = 1,
        [Display(Name = "متاهل")]
        Married = 2,
        [Display(Name = "مطلقه")]
        Divorced = 3,
        [Display(Name = "معیل")]
        Moayal = 4
    }
}
