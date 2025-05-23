﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("ProposalFormRequiredDocumentTypes")]
    public class ProposalFormRequiredDocumentType
    {
        public ProposalFormRequiredDocumentType()
        {
            ProposalFormRequiredDocuments = new List<ProposalFormRequiredDocument>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("ProposalFormRequiredDocumentTypes")]
        public ProposalForm ProposalForm { get; set; }
        public bool IsActive { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("ProposalFormRequiredDocumentType")]
        public List<ProposalFormRequiredDocument> ProposalFormRequiredDocuments { get; set; }

    }
}
