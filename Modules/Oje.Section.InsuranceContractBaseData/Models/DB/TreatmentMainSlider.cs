using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table(nameof(TreatmentMainSlider) + "s")]
    public class TreatmentMainSlider
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string ImageSrc { get; set; }
        [MaxLength(200)]
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
