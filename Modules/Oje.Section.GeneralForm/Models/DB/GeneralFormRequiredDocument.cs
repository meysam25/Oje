using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.GlobalForms.Models.DB
{
    [Table("GeneralFormRequiredDocuments")]
    public class GeneralFormRequiredDocument
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public long GeneralFormId { get; set; }
        [ForeignKey("GeneralFormId"), InverseProperty("GeneralFormRequiredDocuments")]
        public GeneralForm GeneralForm { get; set; }
        public bool IsActive { get; set; }
        public bool? IsRequired { get; set; }
        [Required, MaxLength(200)]
        public string SampleUrl { get; set; }
    }
}
