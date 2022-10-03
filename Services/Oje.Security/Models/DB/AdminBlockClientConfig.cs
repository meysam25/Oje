using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("AdminBlockClientConfigs")]
    public class AdminBlockClientConfig: IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        public long ActionId { get; set; }
        [ForeignKey("ActionId"), InverseProperty("AdminBlockClientConfigs")]
        public Action Action { get; set; }
        public int MaxValue { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("AdminBlockClientConfigs")]
        public SiteSetting SiteSetting { get; set; }
    }
}
