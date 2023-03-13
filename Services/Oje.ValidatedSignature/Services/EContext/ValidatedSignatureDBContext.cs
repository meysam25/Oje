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


        public DbSet<TenderFilledForm> TenderFilledForms { get; set; }
        public DbSet<TenderFilledFormIssue> TenderFilledFormIssues { get; set; }
        public DbSet<TenderFilledFormJson> TenderFilledFormJsons { get; set; }
        public DbSet<TenderFilledFormKey> TenderFilledFormKeies { get; set; }
        public DbSet<TenderFilledFormPF> TenderFilledFormPFs { get; set; }
        public DbSet<TenderFilledFormPrice> TenderFilledFormPrices { get; set; }
        public DbSet<TenderFilledFormsValue> TenderFilledFormsValues { get; set; }
        public DbSet<TenderFilledFormValidCompany> TenderFilledFormValidCompanies { get; set; }


        public DbSet<UserFilledRegisterForm> UserFilledRegisterForms { get; set; }
        public DbSet<UserFilledRegisterFormCardPayment> UserFilledRegisterFormCardPayments { get; set; }
        public DbSet<UserFilledRegisterFormCompany> UserFilledRegisterFormCompanies { get; set; }
        public DbSet<UserFilledRegisterFormJson> UserFilledRegisterFormJsons { get; set; }
        public DbSet<UserFilledRegisterFormKey> UserFilledRegisterFormKeys { get; set; }
        public DbSet<UserFilledRegisterFormValue> UserFilledRegisterFormValues { get; set; }
        public DbSet<UserRegisterFormPrice> UserRegisterFormPrices { get; set; }


        public DbSet<Models.DB.Action> Actions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleAction> RoleActions { get; set; }
        public DbSet<SmsValidationHistory> SmsValidationHistories { get; set; }


        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankAccountSizpay> BankAccountSizpaies { get; set; }
        public DbSet<BankAccountSadad> BankAccountSadads { get; set; }
        public DbSet<BankAccountSep> BankAccountSeps { get; set; }
        public DbSet<BankAccountFactor> BankAccountFactors { get; set; }


        public DbSet<UploadedFile> UploadedFiles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProposalFilledFormCompany>().HasKey(t => new { t.CompanyId, t.ProposalFilledFormId });
            modelBuilder.Entity<ProposalFilledFormSiteSetting>().HasKey(t => new { t.SiteSettingId, t.ProposalFilledFormId });
            modelBuilder.Entity<ProposalFilledFormUser>().HasKey(t => new { t.ProposalFilledFormId, t.UserId, t.Type });
            modelBuilder.Entity<TenderFilledFormPF>().HasKey(t => new { t.TenderProposalFormJsonConfigId, t.TenderFilledFormId });
            modelBuilder.Entity<TenderFilledFormValidCompany>().HasKey(t => new { t.CompanyId, t.TenderFilledFormId });
            modelBuilder.Entity<UserFilledRegisterFormCompany>().HasKey(t => new { t.CompanyId, t.UserFilledRegisterFormId });
            modelBuilder.Entity<User>().Property(e => e.MapLat).HasPrecision(18, 15);
            modelBuilder.Entity<User>().Property(e => e.MapLon).HasPrecision(18, 15);
            modelBuilder.Entity<SmsValidationHistory>().HasKey(t => new { t.Ip1, t.Ip2, t.Ip3, t.Ip4, t.CreateDate, t.Type });
            modelBuilder.Entity<BankAccountFactor>().HasKey(t => new { t.BankAccountId, t.Type, t.ObjectId, t.CreateDate });


            base.OnModelCreating(modelBuilder);
        }
    }
}
