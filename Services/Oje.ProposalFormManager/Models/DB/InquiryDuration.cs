using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("InquiryDurations")]
    public class InquiryDuration
    {
        public InquiryDuration()
        {
            InquiryDurationCompanies = new List<InquiryDurationCompany>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public int Percent { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("InquiryDurations")]
        public ProposalForm ProposalForm { get; set; }
        public int Day { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("InquiryDuration")]
        public List<InquiryDurationCompany> InquiryDurationCompanies { get; set; }
    }
}
