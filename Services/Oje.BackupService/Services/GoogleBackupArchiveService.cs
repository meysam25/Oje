using Microsoft.EntityFrameworkCore;
using Oje.BackupService.Interfaces;
using Oje.BackupService.Models.DB;
using Oje.EmailService.Services.EContext;
using Oje.Infrastructure.Enums;

namespace Oje.BackupService.Services
{
    public class GoogleBackupArchiveService: IGoogleBackupArchiveService
    {
        readonly BackupServiceDBContext db = null;
        public GoogleBackupArchiveService(BackupServiceDBContext db)
        {
            this.db = db;
        }

        public void Create(string fileId, long fileSize, GoogleBackupArchiveType type)
        {
            if(!string.IsNullOrEmpty(fileId))
            {
                db.Entry(new GoogleBackupArchive() 
                {
                    CreateDate = DateTime.Now,
                    FileId = fileId,
                    FileSize = fileSize,
                    Type = type
                }).State = EntityState.Added;
                db.SaveChanges();
            }
        }

        public void DeleteBy(string fileId)
        {
            if(!string.IsNullOrEmpty(fileId))
            {
                var foundItem = db.GoogleBackupArchives.Where(t => t.FileId == fileId).FirstOrDefault();
                if(foundItem != null)
                {
                    db.Entry(foundItem).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
        }

        public List<GoogleBackupArchive> GetIdList(DateTime targetDate)
        {
            return db.GoogleBackupArchives.Where(t => t.CreateDate < targetDate).ToList();
        }
    }
}
