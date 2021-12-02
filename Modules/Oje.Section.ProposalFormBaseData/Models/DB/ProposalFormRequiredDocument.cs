using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.DB
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
        public DateTime CreateDate { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId")]
        [InverseProperty("CreateUserProposalFormRequiredDocuments")]
        public User CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId")]
        [InverseProperty("UpdateUserProposalFormRequiredDocuments")]
        public User UpdateUser { get; set; }
    }
}
