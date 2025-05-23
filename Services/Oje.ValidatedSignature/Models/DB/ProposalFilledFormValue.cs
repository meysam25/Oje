﻿using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("ProposalFilledFormValues")]
    public class ProposalFilledFormValue: SignatureEntity
    {
        public ProposalFilledFormValue()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public long ProposalFilledFormId { get; set; }
        [ForeignKey("ProposalFilledFormId"), InverseProperty("ProposalFilledFormValues")]
        public ProposalFilledForm ProposalFilledForm { get; set; }
        public int ProposalFilledFormKeyId { get; set; }
        [ForeignKey("ProposalFilledFormKeyId"), InverseProperty("ProposalFilledFormValues")]
        public ProposalFilledFormKey ProposalFilledFormKey { get; set; }
        [Required, MaxLength(4000)]
        public string Value { get; set; }

    }
}
