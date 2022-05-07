using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum Custody : byte
    {
        [Display(Name = "نامشخص")]
        Unknown = 0,
        [Display(Name = "تحت تکفل")]
        Custody = 1,
        [Display(Name = "غیر تحت تکفل")]
        NotCustody = 2
    }
}
