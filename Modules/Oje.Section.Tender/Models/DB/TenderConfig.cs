using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("TenderConfigs")]
    public class TenderConfig
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(4000)]
        public string GeneralRoles { get; set; }
        [Required, MaxLength(100)]
        public string PrivateDocumentUrl { get; set; }
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string SubTitle { get; set; }
        public int SiteSettingId { get; set; }
    }
}
