using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum SecretariatLetterUserType: byte
    {
        [Display(Name = "مالک")]
        Owner = 1,
        [Display(Name = "ارجاع")]
        Reffer = 2
    }
}
