using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;
using Oje.Sanab.Models.DB;

namespace Oje.Sms.Services.EContext
{
    public class SanabDBContext : MyBaseDbContext
    {
        public SanabDBContext(DbContextOptions<SanabDBContext> options) : base(options)
        {

        }

        public DbSet<SanabUser> SanabUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
