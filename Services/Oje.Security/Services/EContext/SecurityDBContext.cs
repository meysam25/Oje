using Microsoft.EntityFrameworkCore;
using Oje.Security.Models.DB;

namespace Oje.Security.Services.EContext
{
    public class SecurityDBContext: DbContext
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

         

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlockAutoIp>().HasKey(t => new { t.Ip1, t.Ip2, t.Ip3, t.Ip4, t.CreateDate, t.BlockClientConfigType, t.BlockAutoIpAction });
            modelBuilder.Entity<BlockFirewallIp>().HasKey(t => new { t.Ip1, t.Ip2, t.Ip3, t.Ip4 });

            base.OnModelCreating(modelBuilder);
        }
    }
}
