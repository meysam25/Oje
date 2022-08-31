using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("ProposalFormRequiredDocuments")]
    public class ProposalFormRequiredDocument
    {
        [Key]
        public int Id { get; set; }
        public int ProposalFormRequiredDocumentTypeId { get; set; }
        [ForeignKey("ProposalFormRequiredDocumentTypeId")]
        [InverseProperty("ProposalFormRequiredDocuments")]
        public ProposalFormRequiredDocumentType ProposalFormRequiredDocumentType { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequired { get; set; }
        [MaxLength(100)]
        public string DownloadFile { get; set; }
    }
}
