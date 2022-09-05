namespace Oje.BackupService.Interfaces
{
    public interface IGoogleBackupArchiveService
    {
        void Create(string fileId, long fileSize);
        void DeleteBy(string fileId);
        List<string> GetIdList(DateTime targetDate);
    }
}
