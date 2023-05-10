using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oje.Infrastructure.Interfac;

namespace Oje.ProposalFormWorker.Models.DB
{
    [Table("ProposalFormReminders")]
    public class ProposalFormReminder : IEntityWithSiteSettingId
    {
        public ProposalFormReminder()
        {
            ProposalFormReminderNotifies = new();
        }

        [Key]
        public long Id { get; set; }
        [MaxLength(200)]
        public string Fullname { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey(nameof(ProposalFormId)), InverseProperty("ProposalFormReminders")]
        public ProposalForm ProposalForm { get; set; }
        public long Mobile { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime TargetDate { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        [MaxLength(100)]
        public string PrevInsuranceImage { get; set; }
        [MaxLength(100)]
        public string NationalCardImage { get; set; }
        public long? LoginUserId { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("ProposalFormReminder")]
        public List<ProposalFormReminderNotify> ProposalFormReminderNotifies { get; set; }
    }
}
