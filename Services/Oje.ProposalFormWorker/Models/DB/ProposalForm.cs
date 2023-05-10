using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormWorker.Models.DB
{
    [Table("ProposalForms")]
    public class ProposalForm
    {
        public ProposalForm()
        {
            ProposalFormReminders = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("ProposalForm")]
        public List<ProposalFormReminder> ProposalFormReminders { get; set; }
    }
}
