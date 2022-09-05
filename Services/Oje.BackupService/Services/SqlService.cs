using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Oje.BackupService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;

namespace Oje.BackupService.Services
{
    public class SqlService : ISqlService
    {
        readonly IGoogleBackupArchiveLogService GoogleBackupArchiveLogService = null;
        readonly IFileHelperService FileHelperService = null;
        public SqlService
            (
                IGoogleBackupArchiveLogService GoogleBackupArchiveLogService,
                IFileHelperService FileHelperService
            )
        {
            this.GoogleBackupArchiveLogService = GoogleBackupArchiveLogService;
            this.FileHelperService = FileHelperService;
        }

        public string CreateBackUp()
        {
            try
            {
                var tempBackupFilePath = FileHelperService.GetTempBackupFileName();
                if (File.Exists(tempBackupFilePath))
                    File.Delete(tempBackupFilePath);

                using (SqlConnection conn = new SqlConnection(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"]))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("BACKUP DATABASE Oje TO DISK = '" + tempBackupFilePath + "'", conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            GoogleBackupArchiveLogService.Create(BMessages.DB_Backup_Successfull.GetEnumDisplayName(), GoogleBackupArchiveLogType.SqlGenerateBackup);
                            return tempBackupFilePath;
                        }
                    }
                    finally
                    {
                        if (conn.State == System.Data.ConnectionState.Open)
                            conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                GoogleBackupArchiveLogService.Create(ex.Message, GoogleBackupArchiveLogType.SqlGenerateBackup);
            }
            return null;
        }
    }
}
