using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Models.DB
{
    [Table("RoundInqueries")]
    public class RoundInquery
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Format { get; set; }
        public RoundInqueryType Type { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId"), InverseProperty("RoundInqueries")]
        public ProposalForm ProposalForm { get; set; }
        public int SiteSettingId { get; set; }
    }
}
