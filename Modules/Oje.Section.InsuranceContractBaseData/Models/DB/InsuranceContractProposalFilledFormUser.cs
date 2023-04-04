using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractProposalFilledFormUsers")]
    public class InsuranceContractProposalFilledFormUser
    {
        public InsuranceContractProposalFilledFormUser()
        {
            InsuranceContractProposalFilledFormStatusLogs = new();
        }

        [Key]
        public long Id { get; set; }
        public long InsuranceContractProposalFilledFormId { get; set; }
        [ForeignKey("InsuranceContractProposalFilledFormId"), InverseProperty("InsuranceContractProposalFilledFormUsers")]
        public InsuranceContractProposalFilledForm InsuranceContractProposalFilledForm { get; set; }
        public long InsuranceContractUserId { get; set; }
        [ForeignKey("InsuranceContractUserId"), InverseProperty("InsuranceContractProposalFilledFormUsers")]
        public InsuranceContractUser InsuranceContractUser { get; set; }
        public int InsuranceContractTypeId { get; set; }
        [ForeignKey("InsuranceContractTypeId"), InverseProperty("InsuranceContractProposalFilledFormUsers")]
        public InsuranceContractType InsuranceContractType { get; set; }
        public InsuranceContractProposalFilledFormType Status { get; set; }
        public long? Price { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public string Description { get; set; }


        [InverseProperty("InsuranceContractProposalFilledFormUser")]
        public List<InsuranceContractProposalFilledFormStatusLog> InsuranceContractProposalFilledFormStatusLogs { get; set; }

    }
}
