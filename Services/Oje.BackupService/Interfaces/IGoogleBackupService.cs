namespace Oje.BackupService.Interfaces
{
    public interface IGoogleBackupService
    {
        Task CheckTimeAndCreateBackup(bool canCreateBackup = false);
    }
}
