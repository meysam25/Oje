﻿using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("TenderFilledFormJsons")]
    public class TenderFilledFormJson : SignatureEntity
    {
        [Key]
        public long Id { get; set; }
        public long TenderFilledFormId { get; set; }
        [ForeignKey("TenderFilledFormId"), InverseProperty("TenderFilledFormJsons")]
        public TenderFilledForm TenderFilledForm { get; set; }
        public int? TenderProposalFormJsonConfigId { get; set; }
        [Required]
        public string JsonConfig { get; set; }
        public bool? IsConsultation { get; set; }
    }
}
