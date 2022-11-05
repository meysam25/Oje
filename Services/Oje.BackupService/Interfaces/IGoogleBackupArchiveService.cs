using Oje.BackupService.Models.DB;
using Oje.Infrastructure.Enums;

namespace Oje.BackupService.Interfaces
{
    public interface IGoogleBackupArchiveService
    {
        void Create(string fileId, long fileSize, GoogleBackupArchiveType type);
        void DeleteBy(string fileId);
        List<GoogleBackupArchive> GetIdList(DateTime targetDate);
    }
}
