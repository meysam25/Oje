using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum ExternalNotificationServiceConfigType: byte
    {
        [Display(Name = "VAPID")]
        VAPID = 1
    }
}
