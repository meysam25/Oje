using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum PersonType: byte
    {
        [Display(Name = "حقیقی")]
        Real = 1,
        [Display(Name = "حقوقی")]
        Legal = 2
    }
}
