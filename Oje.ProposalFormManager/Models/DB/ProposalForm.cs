using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.DB
{
    [Table("ProposalForms")]
    public class ProposalForm
    {
        public ProposalForm()
        {
            GlobalInqueries = new();
            ProposalFilledForms = new();
            GlobalDiscounts = new();
            CarExteraDiscounts = new();
            NoDamageDiscounts = new();
            CashPayDiscounts = new();
            RoundInqueries = new();
            InquiryMaxDiscounts = new();
            InsuranceContracts = new();
            InquiryDurations = new();
            InqueryDescriptions = new();
            PaymentMethods = new();
            ProposalFormRequiredDocumentTypes = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public string JsonConfig { get; set; }
        public bool? IsActive { get; set; }
        [MaxLength(100)]
        public string RulesFile { get; set; }
        public ProposalFormType? Type { get; set; }
        public int ProposalFormCategoryId { get; set; }
        [ForeignKey("ProposalFormCategoryId"), InverseProperty("ProposalForms")]
        public ProposalFormCategory ProposalFormCategory { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("ProposalForm")]
        public List<GlobalInquery> GlobalInqueries { get; set; }
        [InverseProperty("ProposalForm")]
        public List<ProposalFilledForm> ProposalFilledForms { get; set; }
        [InverseProperty("ProposalForm")]
        public List<GlobalDiscount> GlobalDiscounts { get; set; }
        [InverseProperty("ProposalForm")]
        public List<CarExteraDiscount> CarExteraDiscounts { get; set; }
        [InverseProperty("ProposalForm")]
        public List<NoDamageDiscount> NoDamageDiscounts { get; set; }
        [InverseProperty("ProposalForm")]
        public List<CashPayDiscount> CashPayDiscounts { get; set; }
        [InverseProperty("ProposalForm")]
        public List<RoundInquery> RoundInqueries { get; set; }
        [InverseProperty("ProposalForm")]
        public List<InquiryMaxDiscount> InquiryMaxDiscounts { get; set; }
        [InverseProperty("ProposalForm")]
        public List<InsuranceContract> InsuranceContracts { get; set; }
        [InverseProperty("ProposalForm")]
        public List<InquiryDuration> InquiryDurations { get; set; }
        [InverseProperty("ProposalForm")]
        public List<InqueryDescription> InqueryDescriptions { get; set; }
        [InverseProperty("ProposalForm")]
        public List<PaymentMethod> PaymentMethods { get; set; }
        [InverseProperty("ProposalForm")]
        public List<ProposalFormRequiredDocumentType> ProposalFormRequiredDocumentTypes { get; set; }
    }
}
