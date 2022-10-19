using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.ProposalFormBaseData.Models.DB
{
    [Table("ProposalFormCommissions")]
    public class ProposalFormCommission
    {
        [Key]
        public int Id { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId"), InverseProperty("ProposalFormCommissions")]
        public ProposalForm ProposalForm { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public long FromPrice { get; set; }
        public long ToPrice { get; set; }
        public long DefPrice { get; set; }
        public decimal Rate { get; set; }
    }
}
