using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum Sponsorship: byte
    {
        [Display(Name = "نامشخص")]
        Unknown = 0,
        [Display(Name = "تحت تکفل")]
        UnderTheTutelage = 1,
        [Display(Name = "غیر تحت تکفل")]
        NotUnderTheTutelage = 2
    }
}
