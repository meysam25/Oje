using Oje.Section.ProposalFormBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;

namespace Oje.Section.ProposalFormBaseData.Services.EContext
{
    public class ProposalFormBaseDataDBContext : MyBaseDbContext
    {
        public ProposalFormBaseDataDBContext(
            DbContextOptions<ProposalFormBaseDataDBContext> option
            ) : base(option)
        {

        }

        public DbSet<ProposalFormPostPrice> ProposalFormPostPrices { get; set; }
        public DbSet<ProposalForm> ProposalForms { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentMethodFile> PaymentMethodFiles { get; set; }
        public DbSet<SiteSetting> SiteSettings { get; set; }
        public DbSet<ProposalFormCategory> ProposalFormCategories { get; set; }
        public DbSet<ProposalFormRequiredDocument> ProposalFormRequiredDocuments { get; set; }
        public DbSet<ProposalFormRequiredDocumentType> ProposalFormRequiredDocumentTypes { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<ProposalFormPrintDescrption> ProposalFormPrintDescrptions { get; set; }
        public DbSet<AgentReffer> AgentReffers { get; set; }
        public DbSet<ProposalFormCommission> ProposalFormCommissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentMethodCompany>().HasKey(t => new { t.CompanyId, t.PaymentMethodId });
            modelBuilder.Entity<UserCompany>().HasKey(t => new { t.CompanyId, t.UserId });
            modelBuilder.Entity<ProposalFormCommission>().Property(e => e.Rate).HasPrecision(15, 12);

            base.OnModelCreating(modelBuilder);
        }
    }
}
