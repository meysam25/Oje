using Microsoft.EntityFrameworkCore;
using Oje.Section.Ticket.Models.DB;

namespace Oje.Section.Ticket.Services.EContext
{
    public class TicketDBContext : DbContext
    {
        public TicketDBContext
            (
                DbContextOptions<TicketDBContext> options
            ) : base(options)
        {

        }

        public DbSet<TicketCategory> TicketCategories { get; set; }
        public DbSet<TicketUser> TicketUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TicketUserAnswer> TicketUserAnswers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
