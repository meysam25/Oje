using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("CashPayDiscounts")]
    public class CashPayDiscount
    {
        public CashPayDiscount()
        {
            CashPayDiscountCompanies = new List<CashPayDiscountCompany>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public int Percent { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("CashPayDiscounts")]
        public ProposalForm ProposalForm { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("CashPayDiscount")]
        public List<CashPayDiscountCompany> CashPayDiscountCompanies { get; set; }
    }
}
