using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.SalesNetworkBaseData.Models.DB
{
    [Table("SalesNetworkCommissionLevels")]
    public class SalesNetworkCommissionLevel : IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        public int SalesNetworkId { get; set; }
        [ForeignKey("SalesNetworkId"), InverseProperty("SalesNetworkCommissionLevels")]
        public SalesNetwork SalesNetwork { get; set; }
        public int Step { get; set; }
        public decimal Rate { get; set; }
        public PersonType? CalceType { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("SalesNetworkCommissionLevels")]
        public SiteSetting SiteSetting { get; set; }
    }
}
