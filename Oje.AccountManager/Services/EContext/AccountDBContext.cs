using Oje.AccountManager.Models.DB;
using Oje.AccountManager.Models.SP;
using Microsoft.EntityFrameworkCore;

namespace Oje.AccountManager.Services.EContext
{
    public class AccountDBContext : DbContext
    {
        public AccountDBContext(DbContextOptions<AccountDBContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Controller> Controllers { get; set; }
        public DbSet<Models.DB.Action> Actions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleAction> RoleActions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<SiteSetting> SiteSettings { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<FileAccessRole> FileAccessRoles { get; set; }
        public DbSet<ChildUserId> ChildUserIds { get; set; }
        public DbSet<ProposalForm> ProposalForms { get; set; }
        public DbSet<RoleProposalForm> RoleProposalForms { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleProposalForm>().HasKey(t => new { t.RoleId, t.ProposalFormId });
            modelBuilder.Entity<UserCompany>().HasKey(t => new { t.UserId, t.CompanyId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
