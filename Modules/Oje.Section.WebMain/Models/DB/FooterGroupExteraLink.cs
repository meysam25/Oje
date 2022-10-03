using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("FooterGroupExteraLinks")]
    public class FooterGroupExteraLink: IEntityWithSiteSettingId
    {
        public FooterGroupExteraLink()
        {
            Childs = new();
        }

        [Key]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId"), InverseProperty("Childs")]
        public FooterGroupExteraLink Parent { get; set; }
        [InverseProperty("Parent")]
        public List<FooterGroupExteraLink> Childs { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Link { get; set; }
        public bool? IsActive { get; set; }
        public int Order { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("FooterGroupExteraLinks")]
        public SiteSetting SiteSetting { get; set; }
    }
}
