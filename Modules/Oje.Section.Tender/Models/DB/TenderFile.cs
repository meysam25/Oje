using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("TenderFiles")]
    public class TenderFile
    {
        [Key]
        public int Id { get; set; } 
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string FileUrl { get; set; }
        public bool IsActive { get; set; }
        public int? UserRegisterFormId { get; set; }
        [ForeignKey("UserRegisterFormId"), InverseProperty("TenderFiles")]
        public UserRegisterForm UserRegisterForm { get; set; }
        public int SiteSettingId { get; set; }
    }
}
