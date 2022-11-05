using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum GoogleBackupArchiveType: byte
    {
        [Display(Name = "گوگل")]
        Google = 1,
        [Display(Name = "مگا")]
        MEGA = 2
    }
}
