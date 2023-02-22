using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Models.DB;

namespace Oje.Section.Tender.Services.EContext
{
    public class TenderDBContext : MyBaseDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TenderFilledForm> TenderFilledForms { get; set; }
        public DbSet<TenderConfig> TenderConfigs { get; set; }
        public DbSet<TenderProposalFormJsonConfig> TenderProposalFormJsonConfigs { get; set; }
        public DbSet<ProposalForm> ProposalForms { get; set; }
        public DbSet<ProposalFormCategory> ProposalFormCategories { get; set; }
        public DbSet<TenderFilledFormKey> TenderFilledFormKeys { get; set; }
        public DbSet<TenderFilledFormsValue> TenderFilledFormsValues { get; set; }
        public DbSet<TenderFilledFormJson> TenderFilledFormJsons { get; set; }
        public DbSet<TenderFilledFormPF> TenderFilledFormPFs { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<TenderFilledFormValidCompany> TenderFilledFormValidCompanies { get; set; }
        public DbSet<TenderFilledFormPrice> TenderFilledFormPrices { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<TenderFilledFormIssue> TenderFilledFormIssues { get; set; }
        public DbSet<TenderFile> TenderFiles { get; set; }
        public DbSet<UserRegisterForm> UserRegisterForms { get; set; }

        public TenderDBContext(DbContextOptions<TenderDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TenderFilledFormPF>().HasKey(t => new { t.TenderProposalFormJsonConfigId, t.TenderFilledFormId });
            modelBuilder.Entity<TenderFilledFormValidCompany>().HasKey(t => new { t.CompanyId, t.TenderFilledFormId });
            modelBuilder.Entity<UserCompany>().HasKey(t => new { t.CompanyId, t.UserId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
