using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("ProposalForms")]
    public class ProposalForm
    {
        public ProposalForm()
        {
            ProposalFilledForms = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("ProposalForm")]
        public List<ProposalFilledForm> ProposalFilledForms { get; set; }
    }
}
