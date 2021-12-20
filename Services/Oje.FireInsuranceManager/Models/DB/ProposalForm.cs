using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.FireInsuranceService.Models.DB
{
    [Table("ProposalForms")]
    public class ProposalForm
    {
        public ProposalForm()
        {
            FireInsuranceCoverages = new();
            InquiryDurations = new();
            InsuranceContracts = new();
            InquiryMaxDiscounts = new();
            GlobalDiscounts = new();
            GlobalInqueries = new();
            CashPayDiscounts = new();
            PaymentMethods = new();
            RoundInqueries = new();
            InqueryDescriptions = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public ProposalFormType Type { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("ProposalForm")]
        public List<FireInsuranceCoverage> FireInsuranceCoverages { get; set; }
        [InverseProperty("ProposalForm")]
        public List<InquiryDuration> InquiryDurations { get; set; }
        [InverseProperty("ProposalForm")]
        public List<InsuranceContract> InsuranceContracts { get; set; }
        [InverseProperty("ProposalForm")]
        public List<InquiryMaxDiscount> InquiryMaxDiscounts { get; set; }
        [InverseProperty("ProposalForm")]
        public List<GlobalDiscount> GlobalDiscounts { get; set; }
        [InverseProperty("ProposalForm")]
        public List<GlobalInquery> GlobalInqueries { get; set; }
        [InverseProperty("ProposalForm")]
        public List<CashPayDiscount> CashPayDiscounts { get; set; }
        [InverseProperty("ProposalForm")]
        public List<PaymentMethod> PaymentMethods { get; set; }
        [InverseProperty("ProposalForm")]
        public List<RoundInquery> RoundInqueries { get; set; }
        [InverseProperty("ProposalForm")]
        public List<InqueryDescription> InqueryDescriptions { get; set; }
    }
}
