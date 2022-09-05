using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Oje.BackupService.Interfaces;
using Oje.BackupService.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System.IO.Compression;

namespace Oje.BackupService.Services
{
    public class FileBackupService: IFileBackupService
    {
        readonly IGoogleBackupArchiveLogService GoogleBackupArchiveLogService = null;
        readonly IFileHelperService FileHelperService = null;
        public FileBackupService
            (
                IGoogleBackupArchiveLogService GoogleBackupArchiveLogService,
                IFileHelperService FileHelperService
            )
        {
            this.GoogleBackupArchiveLogService = GoogleBackupArchiveLogService;
            this.FileHelperService = FileHelperService;
        }

        public bool CanCreateBackup()
        {
            return DateTime.Now.Hour == 22 && !File.Exists(FileHelperService.GetTodayFileName());
        }

        public string CreateBackUp()
        {
            try
            {
                var imageTempFile = FileHelperService.GetTempImageFileName();
                if (File.Exists(imageTempFile))
                    File.Delete(imageTempFile);

                var config = GlobalConfig.Configuration.GetSection("bConfig").Get<bConfig>();
                if (config != null && !string.IsNullOrEmpty(config.image) && Directory.Exists(config.image))
                {
                    ZipFile.CreateFromDirectory(config.image, imageTempFile, CompressionLevel.SmallestSize, true);
                    GoogleBackupArchiveLogService.Create(BMessages.Generate_Image_Zip_Was_Successfull.GetEnumDisplayName(), GoogleBackupArchiveLogType.GenerateImageZip);
                    return imageTempFile;
                }
            }
            catch (Exception ex)
            {
                GoogleBackupArchiveLogService.Create(ex.Message, GoogleBackupArchiveLogType.GenerateImageZip);
            }
            

            return null;
        }
    }
}
