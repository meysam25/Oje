using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Oje.Section.Ticket.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            TicketCategories = new();
            TicketUsers = new();
            TicketUserAnswers = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<TicketCategory> TicketCategories { get; set; }
        [InverseProperty("SiteSetting")]
        public List<TicketUser> TicketUsers { get; set; }
        [InverseProperty("SiteSetting")]
        public List<TicketUserAnswer> TicketUserAnswers { get; set; }
    }
}
