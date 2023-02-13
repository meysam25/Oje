using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum WebsiteType: byte
    {
        [Display(Name = "عادی")]
        Normal = 1,
        [Display(Name = "مناقصه")]
        Tender = 2
    }
}
