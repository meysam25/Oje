using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("PageSliders")]
    public class PageSlider
    {
        [Key]
        public long Id { get; set; }
        public long PageId { get; set; }
        [ForeignKey("PageId"), InverseProperty("PageSliders")]
        public Page Page { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(200)]
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
