using Oje.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.WebMain.Models.DB
{
    [Table("OurObjects")]
    public class OurObject
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string Subtitle { get; set; }
        [Required, MaxLength(200)]
        public string ImageUrl { get; set; }
        [MaxLength(200)]
        public string Url { get; set; }
        public OurObjectType Type { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
