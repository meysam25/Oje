using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("ShortLinks")]
    public class ShortLink: IEntityWithSiteSettingId
    {
        [Key]
        public long Id { get; set; }
        [Required, MaxLength(50)]
        public string Code { get; set; }
        [Required, MaxLength(200)]
        public string TargetLink { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("ShortLinks")]
        public SiteSetting SiteSetting { get; set; }
    }
}
