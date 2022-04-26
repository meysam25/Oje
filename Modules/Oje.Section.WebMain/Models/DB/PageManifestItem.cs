using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("PageManifestItems")]
    public class PageManifestItem
    {
        [Key]
        public long Id { get; set; }
        public long PageManifestId { get; set; }
        [ForeignKey("PageManifestId"), InverseProperty("PageManifestItems")]
        public PageManifest PageManifest { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
