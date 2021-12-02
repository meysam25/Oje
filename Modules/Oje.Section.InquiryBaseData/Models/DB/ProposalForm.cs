using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.DB
{
    [Table("ProposalForms")]
    public class ProposalForm
    {
        public ProposalForm()
        {
            InquiryDurations = new ();
            CashPayDiscounts = new ();
            InquiryMaxDiscounts = new ();
            GlobalDiscounts = new ();
            InsuranceContracts = new();
            RoundInqueries = new();
            InqueryDescriptions = new();
            NoDamageDiscounts = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("ProposalForm")]
        public List<InquiryDuration> InquiryDurations { get; set; }
        [InverseProperty("ProposalForm")]
        public List<CashPayDiscount> CashPayDiscounts { get; set; }
        [InverseProperty("ProposalForm")]
        public List<InquiryMaxDiscount> InquiryMaxDiscounts { get; set; }
        [InverseProperty("ProposalForm")]
        public List<GlobalDiscount> GlobalDiscounts { get; set; }
        [InverseProperty("ProposalForm")]
        public List<InsuranceContract> InsuranceContracts { get; set; }
        [InverseProperty("ProposalForm")]
        public List<RoundInquery> RoundInqueries { get; set; }
        [InverseProperty("ProposalForm")]
        public List<InqueryDescription> InqueryDescriptions { get; set; }
        [InverseProperty("ProposalForm")]
        public List<NoDamageDiscount> NoDamageDiscounts { get; set; }
    }
}
