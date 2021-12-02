using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.DB
{
    [Table("NoDamageDiscounts")]
    public class NoDamageDiscount
    {
        public NoDamageDiscount()
        {
            NoDamageDiscountCompanies = new List<NoDamageDiscountCompany>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("NoDamageDiscounts")]
        public ProposalForm ProposalForm { get; set; }
        public byte Percent { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("NoDamageDiscount")]
        public List<NoDamageDiscountCompany> NoDamageDiscountCompanies { get; set; }
    }
}
