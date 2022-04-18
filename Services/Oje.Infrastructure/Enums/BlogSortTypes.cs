using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum BlogSortTypes
    {
        [Display(Name ="جدید")]
        New = 1,
        [Display(Name = "پربازدید ترین")]
        Viewed = 2,
        [Display(Name = "پر دیدگاه ترین")]
        Commented = 3
    }
}
