using Oje.Section.SalesNetworkBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.SalesNetworkBaseData.Services.EContext
{
    public class SalesNetworkBaseDataDBContext : DbContext
    {
        public SalesNetworkBaseDataDBContext(
            DbContextOptions<SalesNetworkBaseDataDBContext> option
            ): base(option)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<SalesNetwork> SalesNetworks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ProposalForm> ProposalForms { get; set; }
        public DbSet<SalesNetworkMarketer> SalesNetworkMarketers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
