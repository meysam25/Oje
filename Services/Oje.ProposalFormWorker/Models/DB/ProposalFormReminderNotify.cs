using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormWorker.Models.DB
{
    [Table("ProposalFormReminderNotifies")]
    public class ProposalFormReminderNotify
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public long ProposalFormReminderId { get; set; }
        [ForeignKey(nameof(ProposalFormReminderId)), InverseProperty("ProposalFormReminderNotifies")]
        public ProposalFormReminder ProposalFormReminder { get; set; }
    }
}
