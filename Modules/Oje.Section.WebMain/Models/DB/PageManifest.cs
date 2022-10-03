using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("PageManifests")]
    public class PageManifest: IEntityWithSiteSettingId
    {
        public PageManifest()
        {
            PageManifestItems = new();
        }

        [Key]
        public long Id { get; set; }
        public long PageId { get; set; }
        [ForeignKey("PageId"), InverseProperty("PageManifests")]
        public Page Page { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("PageManifests")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("PageManifest")]
        public List<PageManifestItem> PageManifestItems { get; set; }
    }
}
