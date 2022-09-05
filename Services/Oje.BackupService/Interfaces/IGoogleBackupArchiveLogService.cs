using Oje.Infrastructure.Enums;

namespace Oje.BackupService.Interfaces
{
    public interface IGoogleBackupArchiveLogService
    {
        void Create(string message, GoogleBackupArchiveLogType type);
    }
}
