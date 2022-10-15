using Microsoft.EntityFrameworkCore;
using Oje.Sms.Models.DB;

namespace Oje.Sms.Services.EContext
{
    public class SmsDBContext : DbContext
    {
        public SmsDBContext(DbContextOptions<SmsDBContext> options) : base(options)
        {

        }

        public DbSet<Models.DB.SmsConfig> SmsConfigs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SmsTriger> SmsTrigers { get; set; }
        public DbSet<SmsTemplate> SmsTemplates { get; set; }
        public DbSet<SmsSendingQueue> SmsSendingQueues { get; set; }
        public DbSet<SmsSendingQueueError> SmsSendingQueueErrors { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<SmsValidationHistory> SmsValidationHistories { get; set; }
        public DbSet<BlockLoginUser> BlockLoginUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SmsSendingQueueError>().HasKey(t => new { t.SmsSendingQueueId, t.CreateDate });
            modelBuilder.Entity<SmsValidationHistory>().HasKey(t => new { t.Ip1, t.Ip2, t.Ip3, t.Ip4, t.CreateDate, t.Type });

            base.OnModelCreating(modelBuilder);
        }
    }
}
