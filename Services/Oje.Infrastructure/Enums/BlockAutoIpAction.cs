using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum BlockAutoIpAction: byte
    {
        [Display(Name = "قبل از اجرا")]
        BeforeExecute = 1,
        [Display(Name = "بعد از اجرا")]
        AfterExecute = 2
    }
}
