using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("GlobalDiscountUseds")]
    public class GlobalDiscountUsed
    {
        [Key]
        public long Id { get; set; }
        public long ProposalFilledFormId { get; set; }
        [ForeignKey("ProposalFilledFormId"), InverseProperty("GlobalDiscountUseds")]
        public ProposalFilledForm ProposalFilledForm { get; set; }
        public int GlobalDiscountId { get; set; }
        [ForeignKey("GlobalDiscountId"), InverseProperty("GlobalDiscountUseds")]
        public GlobalDiscount GlobalDiscount { get; set; }
    }
}
