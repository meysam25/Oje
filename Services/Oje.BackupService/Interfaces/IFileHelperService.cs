namespace Oje.BackupService.Interfaces
{
    public interface IFileHelperService
    {
        string GetTargetDirectory(bool ignoreTodayFolder);
        string GetTempImageFileName();
        string GetTodayFileName();
        string GetTempBackupFileName();
    }
}
