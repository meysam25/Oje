using Oje.Infrastructure.Interfac;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Ticket.Models.DB
{
    [Table("TicketUsers")]
    public class TicketUser: IEntityWithSiteSettingId
    {
        public TicketUser()
        {
            TicketUserAnswers = new();
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(300)]
        public string Title { get; set; }
        public int TicketCategoryId { get; set; }
        [ForeignKey("TicketCategoryId"), InverseProperty("TicketUsers")]
        public TicketCategory TicketCategory { get; set; }
        [Required, MaxLength(4000)]
        public string Description { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId"), InverseProperty("CreateUserTicketUsers")]
        public User CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId"), InverseProperty("UpdateUserTicketUsers")]
        public User UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        [MaxLength(100)]
        public string FileUrl { get; set; }
        public bool IsDelete { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("TicketUser")]
        public List<TicketUserAnswer> TicketUserAnswers { get; set; }
    }
}
