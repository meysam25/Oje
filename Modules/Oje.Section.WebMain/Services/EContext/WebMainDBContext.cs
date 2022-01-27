using Microsoft.EntityFrameworkCore;
using Oje.Section.WebMain.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Services.EContext
{
    public class WebMainDBContext : DbContext
    {
        public DbSet<TopMenu> TopMenus { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageLeftRightDesign> PageLeftRightDesigns { get; set; }
        public DbSet<PageLeftRightDesignItem> PageLeftRightDesignItems { get; set; }
        public DbSet<ProposalFormReminder> ProposalFormReminders { get; set; }
        public DbSet<FooterExteraLink> FooterExteraLinks { get; set; }
        public DbSet<FooterGroupExteraLink> FooterGroupExteraLinks { get; set; }

        public WebMainDBContext(DbContextOptions<WebMainDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProposalFormReminder>().HasKey(t => new { t.CreateDate, t.Ip1, t.Ip2, t.Ip3, t.Ip4 });

            base.OnModelCreating(modelBuilder);
        }
    }
}
