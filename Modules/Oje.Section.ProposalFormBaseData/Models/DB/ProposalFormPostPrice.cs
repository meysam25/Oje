using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.DB
{
    [Table("ProposalFormPostPrices")]
    public class ProposalFormPostPrice
    {
        [Key]
        public int Id { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("ProposalFormPostPrices")]
        public ProposalForm ProposalForm { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
