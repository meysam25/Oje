namespace Oje.BackupService.Interfaces
{
    public interface IFileBackupService
    {
        bool CanCreateBackup();
        string CreateBackUp();
    }
}
