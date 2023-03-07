using Microsoft.EntityFrameworkCore;
using Oje.ValidatedSignature.Models.DB;

namespace Oje.ValidatedSignature.Services.EContext
{
    public class ValidatedSignatureDBContext : DbContext
    {
        public ValidatedSignatureDBContext(DbContextOptions<ValidatedSignatureDBContext> options) : base(options)
        {

        }

        public DbSet<ProposalFilledForm> ProposalFilledForms { get; set; }
        public DbSet<ProposalFilledFormCacheJson> ProposalFilledFormCacheJsons { get; set; }
        public DbSet<ProposalFilledFormCompany> ProposalFilledFormCompanies { get; set; }
        public DbSet<ProposalFilledFormDocument> ProposalFilledFormDocuments { get; set; }
        public DbSet<ProposalFilledFormJson> ProposalFilledFormJsons { get; set; }
        public DbSet<ProposalFilledFormKey> ProposalFilledFormKeies { get; set; }
        public DbSet<ProposalFilledFormSiteSetting> ProposalFilledFormSiteSettings { get; set; }
        public DbSet<ProposalFilledFormStatusLog> ProposalFilledFormStatusLogs { get; set; }
        public DbSet<ProposalFilledFormStatusLogFile> ProposalFilledFormStatusLogFiles { get; set; }
        public DbSet<ProposalFilledFormUser> ProposalFilledFormUsers { get; set; }
        public DbSet<ProposalFilledFormValue> ProposalFilledFormValues { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ProposalForm> ProposalForms { get; set; }
        public DbSet<SiteSetting> SiteSettings { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProposalFilledFormCompany>().HasKey(t => new { t.CompanyId, t.ProposalFilledFormId });
            modelBuilder.Entity<ProposalFilledFormSiteSetting>().HasKey(t => new { t.SiteSettingId, t.ProposalFilledFormId });
            modelBuilder.Entity<ProposalFilledFormUser>().HasKey(t => new { t.ProposalFilledFormId, t.UserId, t.Type });

            base.OnModelCreating(modelBuilder);
        }
    }
}
