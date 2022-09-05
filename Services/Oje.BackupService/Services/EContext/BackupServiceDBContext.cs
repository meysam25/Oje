using Microsoft.EntityFrameworkCore;
using Oje.BackupService.Models.DB;
using Oje.Infrastructure.Services;

namespace Oje.EmailService.Services.EContext
{
    public class BackupServiceDBContext : MyBaseDbContext
    {
        public BackupServiceDBContext(DbContextOptions<BackupServiceDBContext> options) : base(options)
        {

        }

        public DbSet<GoogleBackupArchive> GoogleBackupArchives { get; set; }
        public DbSet<GoogleBackupArchiveLog> GoogleBackupArchiveLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
