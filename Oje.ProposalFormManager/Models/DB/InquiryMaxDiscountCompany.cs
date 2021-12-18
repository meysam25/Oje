using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("InquiryMaxDiscountCompanies")]
    public class InquiryMaxDiscountCompany
    {
        [Key]
        public long Id { get; set; }
        public int InquiryMaxDiscountId { get; set; }
        [ForeignKey("InquiryMaxDiscountId")]
        [InverseProperty("InquiryMaxDiscountCompanies")]
        public InquiryMaxDiscount InquiryMaxDiscount { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("InquiryMaxDiscountCompanies")]
        public Company Company { get; set; }
    }
}
