using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractProposalFilledFormStatusLogs")]
    public class InsuranceContractProposalFilledFormStatusLog
    {
        public long InsuranceContractProposalFilledFormId { get; set; }
        [ForeignKey("InsuranceContractProposalFilledFormId"), InverseProperty("InsuranceContractProposalFilledFormStatusLogs")]
        public InsuranceContractProposalFilledForm InsuranceContractProposalFilledForm { get; set; }
        public InsuranceContractProposalFilledFormType Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("InsuranceContractProposalFilledFormStatusLogs")]
        public User User { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
    }
}
