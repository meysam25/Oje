using Oje.Section.ProposalFormBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace Oje.Section.ProposalFormBaseData.Services.EContext
{
    public class ProposalFormBaseDataDBContext : DbContext
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentMethodCompany>().HasKey(t => new { t.CompanyId, t.PaymentMethodId });
            modelBuilder.Entity<UserCompany>().HasKey(t => new { t.CompanyId, t.UserId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
