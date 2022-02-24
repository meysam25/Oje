using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormService.Models.DB.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Services.EContext
{
    public class ProposalFormReportDBContext : DbContext
    {
        public ProposalFormReportDBContext(DbContextOptions<ProposalFormReportDBContext> options) : base(options)
        {

        }

        public DbSet<ProposalFilledForm> ProposalFilledForms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProposalFilledFormCompany>().HasKey(t => new { t.CompanyId, t.ProposalFilledFormId });
            modelBuilder.Entity<ProposalFilledFormUser>().HasKey(t => new { t.ProposalFilledFormId, t.UserId, t.Type });

            base.OnModelCreating(modelBuilder);
        }
    }
}
