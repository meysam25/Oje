using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("GlobalInqueries")]
    public class GlobalInquery: SignatureEntity
    {
        public GlobalInquery()
        {
            ProposalFilledForms = new();
            GlobalInquiryItems = new();
        }

        [Key]
        public long Id { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreateDate { get; set; }
        public long GlobalInputInqueryId { get; set; }
        public int ProposalFormId { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("GlobalInquery")]
        public List<ProposalFilledForm> ProposalFilledForms { get; set; }
        [InverseProperty("GlobalInquery")]
        public List<GlobalInquiryItem> GlobalInquiryItems { get; set; }
       
    }
}
