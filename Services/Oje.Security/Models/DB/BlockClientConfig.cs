using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("BlockClientConfigs")]
    public class BlockClientConfig: IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        public BlockClientConfigType Type { get; set; }
        public int MaxSoftware { get; set; }
        public int MaxSuccessSoftware { get; set; }
        public int MaxFirewall { get; set; }
        public int MaxSuccessFirewall { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("BlockClientConfigs")]
        public SiteSetting SiteSetting { get; set; }

    }
}
