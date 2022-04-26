using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum PageWebItemType
    {
        [Display(Name = "دیزاین چپ و راست")]
        LeftAndRight = 1,
        [Display(Name = "فهرست صفحه")]
        Manifest = 2
    }
}
