using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum InsuranceContractUserFamilyRelation
    {
        [Display(Name = "نامشخص")]
        Unknown = 0,
        [Display(Name = "اصلی")]
        Self = 1,
        [Display(Name = "همسر")]
        Spouse = 2,
        [Display(Name = "فرزند پسر")]
        ChildBoy = 3,
        [Display(Name = "فرزند دختر")]
        ChildGirl = 4,
        [Display(Name = "پدر")]
        Father = 5,
        [Display(Name = "مادر")]
        Mother = 6,
        [Display(Name = "برادر")]
        Brother = 8,
        [Display(Name = "خواهر")]
        Sister = 9,
        [Display(Name = "سایر")]
        Other = 7,
    }
}
