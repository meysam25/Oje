using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("TopMenus")]
    public class TopMenu: IEntityWithSiteSettingId
    {
        [Key]
        public long Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Link { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey("ParentId"), InverseProperty("Childs")]
        public TopMenu Parent { get; set; }
        [InverseProperty("Parent")]
        public List<TopMenu> Childs { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("TopMenus")]
        public SiteSetting SiteSetting { get; set; }
    }
}
