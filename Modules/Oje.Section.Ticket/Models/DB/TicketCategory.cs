using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Ticket.Models.DB
{
    [Table("TicketCategories")]
    public class TicketCategory: IEntityWithSiteSettingId
    {
        public TicketCategory()
        {
            Childs = new();
            TicketUsers = new();
        }

        [Key]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId"), InverseProperty("Childs")]
        public TicketCategory Parent { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("TicketCategories")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("Parent")]
        public List<TicketCategory> Childs { get; set; }
        [InverseProperty("TicketCategory")]
        public List<TicketUser> TicketUsers { get; set; }
    }
}
