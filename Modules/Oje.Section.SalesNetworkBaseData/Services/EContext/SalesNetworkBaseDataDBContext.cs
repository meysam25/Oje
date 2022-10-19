using Oje.Section.SalesNetworkBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;
using Oje.Section.SalesNetworkBaseData.Models.SP;

namespace Oje.Section.SalesNetworkBaseData.Services.EContext
{
    public class SalesNetworkBaseDataDBContext : MyBaseDbContext
    {
        public SalesNetworkBaseDataDBContext(
            DbContextOptions<SalesNetworkBaseDataDBContext> option
            ) : base(option)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<SalesNetwork> SalesNetworks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ProposalForm> ProposalForms { get; set; }
        public DbSet<SalesNetworkMarketer> SalesNetworkMarketers { get; set; }
        public DbSet<SalesNetworkCommissionLevel> SalesNetworkCommissionLevels { get; set; }
        public DbSet<UserCommission> UserCommissions { get; set; }
        public DbSet<UserLevel> UserLevels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesNetworkCommissionLevel>().Property(t => t.Rate).HasPrecision(15, 10);

            base.OnModelCreating(modelBuilder);
        }
    }
}
