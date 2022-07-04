using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Models.DB.Reports;

namespace Oje.ProposalFormService.Services.EContext
{
    public class ProposalFormReportDBContext : MyBaseDbContext
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
