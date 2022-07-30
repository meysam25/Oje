using Oje.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("DashboardSectionUserNotificationTypes")]
    public class DashboardSectionUserNotificationType
    {
        public int DashboardSectionId { get; set; }
        [ForeignKey("DashboardSectionId"), InverseProperty("DashboardSectionUserNotificationTypes")]
        public DashboardSection DashboardSection { get; set; }
        public UserNotificationType Type { get; set; }
    }
}
