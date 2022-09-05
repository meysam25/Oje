using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;
using Oje.Security.Models.DB;

namespace Oje.Security.Services.EContext
{
    public class SecurityDBContext : MyBaseDbContext
    {
        public SecurityDBContext(DbContextOptions<SecurityDBContext> options) : base(options)
        {

        }

        public DbSet<FileAccessRole> FileAccessRoles { get; set; }
        public DbSet<IpLimitationWhiteList> IpLimitationWhiteLists { get; set; }
        public DbSet<IpLimitationBlackList> IpLimitationBlackLists { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<BlockClientConfig> BlockClientConfigs { get; set; }
        public DbSet<BlockAutoIp> BlockAutoIps { get; set; }
        public DbSet<BlockFirewallIp> BlockFirewallIps { get; set; }
        public DbSet<UserLoginConfig> UserLoginConfigs { get; set; }
        public DbSet<UserAdminLogConfig> UserAdminLogConfigs { get; set; }
        public DbSet<Models.DB.Action> Actions { get; set; }
        public DbSet<Controller> Controllers { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAdminLog> UserAdminLogs { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<AdminBlockClientConfig> AdminBlockClientConfigs { get; set; }
        public DbSet<UserLoginLogoutLog> UserLoginLogoutLogs { get; set; }
        public DbSet<BlockLoginUser> BlockLoginUsers { get; set; }
        public DbSet<DebugEmail> DebugEmails { get; set; }
        public DbSet<DebugEmailReceiver> DebugEmailReceivers { get; set; }
        public DbSet<ErrorParameter> ErrorParameters { get; set; }
        public DbSet<ErrorFirewallManualAdd> ErrorFirewallManualAdds { get; set; }
        public DbSet<ValidRangeIp> ValidRangeIps { get; set; }
        public DbSet<InValidRangeIp> InValidRangeIps { get; set; }
        public DbSet<DebugInfo> DebugInfos { get; set; }
        public DbSet<GoogleBackupArchiveLog> GoogleBackupArchiveLogs { get; set; }
        public DbSet<GoogleBackupArchive> GoogleBackupArchives { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlockAutoIp>().HasKey(t => new { t.Ip1, t.Ip2, t.Ip3, t.Ip4, t.CreateDate, t.BlockClientConfigType, t.BlockAutoIpAction });
            modelBuilder.Entity<BlockFirewallIp>().HasKey(t => new { t.Ip1, t.Ip2, t.Ip3, t.Ip4 });
            modelBuilder.Entity<ErrorFirewallManualAdd>().HasKey(t => new { t.Ip1, t.Ip2, t.Ip3, t.Ip4 });
            modelBuilder.Entity<InValidRangeIp>().HasKey(t => new { t.Ip1, t.Ip2, t.Ip3, t.Ip4 });

            base.OnModelCreating(modelBuilder);
        }
    }
}
