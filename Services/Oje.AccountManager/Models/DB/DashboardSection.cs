using Oje.Infrastructure.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("DashboardSections")]
    public class DashboardSection
    {
        public DashboardSection()
        {
            DashboardSectionUserNotificationTypes = new();
        }

        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId"), InverseProperty("DashboardSections")]
        public Role Role { get; set; }
        public long ActionId { get; set; }
        [ForeignKey("ActionId"), InverseProperty("DashboardSections")]
        public Action Action { get; set; }
        public DashboardSectionType Type { get; set; }
        [Required, MaxLength(50)]
        public string Class { get; set; }
        public int? DashboardSectionCategoryId { get; set; }
        [ForeignKey("DashboardSectionCategoryId"), InverseProperty("DashboardSections")]
        public DashboardSectionCategory DashboardSectionCategory { get; set; }
        public int? Order { get; set; }
        [MaxLength(20)]
        public string Color { get; set; }

        [InverseProperty("DashboardSection")]
        public List<DashboardSectionUserNotificationType> DashboardSectionUserNotificationTypes { get; set; }

    }
}
