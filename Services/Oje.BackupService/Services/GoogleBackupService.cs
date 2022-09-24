using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Oje.BackupService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System.IO.Compression;

namespace Oje.BackupService.Services
{
    public class GoogleBackupService : IGoogleBackupService
    {
        readonly IFileBackupService FileBackupService = null;
        readonly ISqlService SqlService = null;
        readonly IGoogleBackupArchiveService GoogleBackupArchiveService = null;
        readonly IGoogleBackupArchiveLogService GoogleBackupArchiveLogService = null;
        readonly IFileHelperService FileHelperService = null;

        public GoogleBackupService
            (
                IFileBackupService FileBackupService,
                ISqlService SqlService,
                IGoogleBackupArchiveService GoogleBackupArchiveService,
                IGoogleBackupArchiveLogService GoogleBackupArchiveLogService,
                IFileHelperService FileHelperService
            )
        {
            this.FileBackupService = FileBackupService;
            this.SqlService = SqlService;
            this.GoogleBackupArchiveService = GoogleBackupArchiveService;
            this.GoogleBackupArchiveLogService = GoogleBackupArchiveLogService;
            this.FileHelperService = FileHelperService;
        }

        public async Task CheckTimeAndCreateBackup()
        {
            bool canCreateBackupKnow = FileBackupService.CanCreateBackup();
            if (canCreateBackupKnow == true)
            {
                string zipFilePath = FileBackupService.CreateBackUp();
                string dbBackupFilePath = SqlService.CreateBackUp();
                if (File.Exists(zipFilePath) && File.Exists(dbBackupFilePath))
                {
                    string todayFileName = FileHelperService.GetTodayFileName();
                    ZipFile.CreateFromDirectory(FileHelperService.GetTargetDirectory(false), todayFileName, CompressionLevel.SmallestSize, true);
                    File.Delete(zipFilePath);
                    File.Delete(dbBackupFilePath);
                    Directory.Delete(FileHelperService.GetTargetDirectory(false));
                    await uploadFile(todayFileName);
                    if (File.Exists(todayFileName))
                        File.Delete(todayFileName);
                    deleteLastDayBackup(8);
                }
            }

        }

        string getJsonConfig()
        {
            return Path.Combine(GlobalConfig.Configuration.GetValue<string>(WebHostDefaults.ContentRootKey), "setadproject-3aa47afb3c07.json");
        }

        void deleteLastDayBackup(int lastDay)
        {
            try
            {
                var targetDate = DateTime.Now.AddDays(lastDay * -1);
                List<string> allExpiredIds = GoogleBackupArchiveService.GetIdList(targetDate);
                if (allExpiredIds != null && allExpiredIds.Count > 0)
                {
                    var credential = GoogleCredential.FromFile(getJsonConfig()).CreateScoped(DriveService.ScopeConstants.Drive);
                    using (var service = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential
                    }))
                    {
                        foreach (var fileId in allExpiredIds)
                        {
                            var request = service.Files.Delete(fileId);
                            var result = request.Execute();
                            if(string.IsNullOrEmpty(result))
                            {
                                GoogleBackupArchiveLogService.Create(fileId + " " + BMessages.Delete_Successfull.GetEnumDisplayName(), GoogleBackupArchiveLogType.RemoveExpiredFile);
                                GoogleBackupArchiveService.DeleteBy(fileId);
                            }
                            else
                                GoogleBackupArchiveLogService.Create(result, GoogleBackupArchiveLogType.RemoveExpiredFile);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoogleBackupArchiveLogService.Create(ex.Message, GoogleBackupArchiveLogType.RemoveExpiredFile);
            }
            
        }

        private async Task uploadFile(string todayFileName)
        {
            try
            {
                var credential = GoogleCredential.FromFile(getJsonConfig()).CreateScoped(DriveService.ScopeConstants.Drive);
                using (var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential
                }))
                {
                    var fi = new FileInfo(todayFileName);
                    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = fi.Name
                    };
                    using (var fsSource = new FileStream(todayFileName, FileMode.Open, FileAccess.Read))
                    {
                        var request = service.Files.Create(fileMetadata, fsSource, "application/zip");
                        request.Fields = "*";
                        var results = await request.UploadAsync(CancellationToken.None);

                        if (results.Status != UploadStatus.Failed && !string.IsNullOrEmpty(request.ResponseBody?.Id))
                        {
                            GoogleBackupArchiveService.Create(request.ResponseBody?.Id, fi.Length);
                            GoogleBackupArchiveLogService.Create(BMessages.Operation_Was_Successfull.GetEnumDisplayName(), GoogleBackupArchiveLogType.UploadSection);
                        }
                        else
                        {
                            if(results != null && results.Exception != null && !string.IsNullOrEmpty(results.Exception.Message))
                                GoogleBackupArchiveLogService.Create(BMessages.Upload_To_Google_Was_Not_Successfull.GetEnumDisplayName() + " " + results.Exception.Message, GoogleBackupArchiveLogType.UploadSection);
                            else
                                GoogleBackupArchiveLogService.Create(BMessages.Upload_To_Google_Was_Not_Successfull.GetEnumDisplayName(), GoogleBackupArchiveLogType.UploadSection);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoogleBackupArchiveLogService.Create(ex.Message, GoogleBackupArchiveLogType.UploadSection);
            }

        }
    }
}
