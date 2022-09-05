using Microsoft.EntityFrameworkCore;
using Oje.BackupService.Interfaces;
using Oje.BackupService.Models.DB;
using Oje.EmailService.Services.EContext;
using Oje.Infrastructure.Enums;

namespace Oje.BackupService.Services
{
    public class GoogleBackupArchiveLogService: IGoogleBackupArchiveLogService
    {
        readonly BackupServiceDBContext db = null;
        public GoogleBackupArchiveLogService
            (
                BackupServiceDBContext db
            )
        {
            this.db = db;
        }

        public void Create(string message, GoogleBackupArchiveLogType type)
        {
            if(!string.IsNullOrEmpty(message))
            {
                db.Entry(new GoogleBackupArchiveLog() 
                {
                    Message = message,
                    Type = type,
                    CreateDate = DateTime.Now
                }).State = EntityState.Added;
                db.SaveChanges();
            }
        }
    }
}
