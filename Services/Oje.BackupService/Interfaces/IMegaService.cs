namespace Oje.BackupService.Interfaces
{
    public interface IMegaService
    {
        Task Delete(string fileId);
        Task uploadFile(string todayFileName, IGoogleBackupArchiveService GoogleBackupArchiveService);
    }
}
