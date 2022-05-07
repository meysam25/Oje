using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum Gender : byte
    {
        [Display(Name = "نامشخص")]
        Unknown = 0,
        [Display(Name = "مرد")]
        Male = 1,
        [Display(Name = "زن")]
        Female = 2
    }
}
