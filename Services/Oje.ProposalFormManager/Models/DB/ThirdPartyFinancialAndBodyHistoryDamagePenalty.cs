using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("ThirdPartyFinancialAndBodyHistoryDamagePenalties")]
    public class ThirdPartyFinancialAndBodyHistoryDamagePenalty
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
        public bool? IsFinancial { get; set; }
    }
}
