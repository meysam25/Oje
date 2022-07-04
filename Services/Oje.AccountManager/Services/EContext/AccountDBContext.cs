using Oje.AccountService.Models.DB;
using Oje.AccountService.Models.SP;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;

namespace Oje.AccountService.Services.EContext
{
    public class AccountDBContext : MyBaseDbContext
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
        public DbSet<SiteSetting> SiteSettings { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ChildUserId> ChildUserIds { get; set; }
        public DbSet<ProposalForm> ProposalForms { get; set; }
        public DbSet<RoleProposalForm> RoleProposalForms { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<UserNotificationTriger> UserNotificationTrigers { get; set; }
        public DbSet<UserNotificationTemplate> UserNotificationTemplates { get; set; }
        public DbSet<DashboardSection> DashboardSections { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<DashboardSectionCategory> DashboardSectionCategories { get; set; }
        public DbSet<SectionCategory> SectionCategories { get; set; }
        public DbSet<SectionCategorySection> SectionCategorySections { get; set; }
        public DbSet<ControllerCategory> ControllerCategories { get; set; }
        public DbSet<ControllerCategoryController> ControllerCategoryControllers { get; set; }
        public DbSet<ExternalNotificationServiceConfig> ExternalNotificationServiceConfigs { get; set; }
        public DbSet<ExternalNotificationServicePushSubscription> ExternalNotificationServicePushSubscriptions { get; set; }
        public DbSet<ExternalNotificationServicePushSubscriptionError> ExternalNotificationServicePushSubscriptionErrors { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }
        public DbSet<UserMessageReply> UserMessageReplies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleProposalForm>().HasKey(t => new { t.RoleId, t.ProposalFormId });
            modelBuilder.Entity<UserCompany>().HasKey(t => new { t.UserId, t.CompanyId });
            modelBuilder.Entity<UserNotification>().HasKey(t => new { t.UserId, t.CreateDate, t.Type });
            modelBuilder.Entity<Property>().HasKey(t => new { t.Name, t.SiteSettingId, t.Type });
            modelBuilder.Entity<SectionCategorySection>().HasKey(t => new { t.SectionId, t.SectionCategoryId });
            modelBuilder.Entity<ControllerCategoryController>().HasKey(t => new { t.ControllerId, t.ControllerCategoryId });

            modelBuilder.Entity<User>().Property(e => e.MapLat).HasPrecision(18, 15);
            modelBuilder.Entity<User>().Property(e => e.MapLon).HasPrecision(18, 15);

            base.OnModelCreating(modelBuilder);
        }
    }
}
