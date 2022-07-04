using Microsoft.EntityFrameworkCore;
using Oje.EmailService.Models.DB;
using Oje.Infrastructure.Services;

namespace Oje.EmailService.Services.EContext
{
    public class EmailServiceDBContext : MyBaseDbContext
    {
        public EmailServiceDBContext(DbContextOptions<EmailServiceDBContext> options) : base(options)
        {

        }

        public DbSet<EmailConfig> EmailConfigs { get; set; }
        public DbSet<EmailTriger> EmailTrigers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<EmailSendingQueue> EmailSendingQueues { get; set; }
        public DbSet<EmailSendingQueueError> EmailSendingQueueErrors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailSendingQueueError>().HasKey(t => new { t.EmailSendingQueueId, t.CreateDate });

            base.OnModelCreating(modelBuilder);
        }
    }
}
