using Oje.Infrastructure.Interfac;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Ticket.Models.DB
{
    [Table("TicketUserAnswers")]
    public class TicketUserAnswer: IEntityWithSiteSettingId
    {
        [Key]
        public long Id { get; set; }
        public long TicketUserId { get; set; }
        [ForeignKey("TicketUserId"), InverseProperty("TicketUserAnswers")]
        public TicketUser TicketUser { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId"), InverseProperty("CreateUserTicketUserAnswers")]
        public User CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        [Required, MaxLength(4000)]
        public string Description { get; set; }
        [MaxLength(100)]
        public string FileUrl { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("TicketUserAnswers")]
        public SiteSetting SiteSetting { get; set; }
    }
}
