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
        readonly IMegaService MegaService = null;

        public GoogleBackupService
            (
                IFileBackupService FileBackupService,
                ISqlService SqlService,
                IGoogleBackupArchiveService GoogleBackupArchiveService,
                IGoogleBackupArchiveLogService GoogleBackupArchiveLogService,
                IFileHelperService FileHelperService,
                IMegaService MegaService
            )
        {
            this.FileBackupService = FileBackupService;
            this.SqlService = SqlService;
            this.GoogleBackupArchiveService = GoogleBackupArchiveService;
            this.GoogleBackupArchiveLogService = GoogleBackupArchiveLogService;
            this.FileHelperService = FileHelperService;
            this.MegaService = MegaService;
        }

        public async Task CheckTimeAndCreateBackup(bool canCreateBackup = false)
        {
            bool canCreateBackupKnow = FileBackupService.CanCreateBackup();
            if (canCreateBackupKnow == true || canCreateBackup == true)
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
                    await MegaService.uploadFile(todayFileName, GoogleBackupArchiveService);
                    if (File.Exists(todayFileName))
                        try { File.Delete(todayFileName); } catch { };
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
            var targetDate = DateTime.Now.AddDays(lastDay * -1);
            var allExpiredIds = GoogleBackupArchiveService.GetIdList(targetDate);

            try
            {
                if (allExpiredIds != null && allExpiredIds.Count > 0)
                {
                    var allGoogleIds = allExpiredIds.Where(t => t.Type == null || t.Type == GoogleBackupArchiveType.Google).ToList();
                    if (allGoogleIds.Count > 0)
                    {
                        var credential = GoogleCredential.FromFile(getJsonConfig()).CreateScoped(DriveService.ScopeConstants.Drive);
                        using (var service = new DriveService(new BaseClientService.Initializer()
                        {
                            HttpClientInitializer = credential
                        }))
                        {
                            foreach (var file in allGoogleIds)
                            {
                                var request = service.Files.Delete(file.FileId);
                                var result = request.Execute();
                                if (string.IsNullOrEmpty(result))
                                {
                                    GoogleBackupArchiveLogService.Create(file.FileId + " " + BMessages.Delete_Successfull.GetEnumDisplayName(), GoogleBackupArchiveLogType.RemoveExpiredFile);
                                    GoogleBackupArchiveService.DeleteBy(file.FileId);
                                }
                                else
                                    GoogleBackupArchiveLogService.Create(result, GoogleBackupArchiveLogType.RemoveExpiredFile);

                            }
                        }
                    }
                   
                }
            }
            catch (Exception ex)
            {
                GoogleBackupArchiveLogService.Create(ex.Message, GoogleBackupArchiveLogType.RemoveExpiredFile);
            }

            try
            {
                if(allExpiredIds != null && allExpiredIds.Count > 0 )
                {
                    var allMegaIds = allExpiredIds.Where(t => t.Type == GoogleBackupArchiveType.MEGA).ToList();

                    if (allMegaIds.Count > 0)
                    {
                        foreach (var file in allMegaIds)
                        {
                            MegaService.Delete(file.FileId).GetAwaiter().GetResult();
                            GoogleBackupArchiveLogService.Create(file.FileId + " " + BMessages.Delete_Successfull.GetEnumDisplayName(), GoogleBackupArchiveLogType.RemoveExpiredFile);
                            GoogleBackupArchiveService.DeleteBy(file.FileId);
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
                            GoogleBackupArchiveService.Create(request.ResponseBody?.Id, fi.Length, GoogleBackupArchiveType.Google);
                            GoogleBackupArchiveLogService.Create(BMessages.Operation_Was_Successfull.GetEnumDisplayName(), GoogleBackupArchiveLogType.UploadSection);
                        }
                        else
                        {
                            if (results != null && results.Exception != null && !string.IsNullOrEmpty(results.Exception.Message))
                                GoogleBackupArchiveLogService.Create(BMessages.Upload_To_Google_Was_Not_Successfull.GetEnumDisplayName() + " " + results.Exception.Message, GoogleBackupArchiveLogType.UploadSection);
                            else
                                GoogleBackupArchiveLogService.Create(BMessages.Upload_To_Google_Was_Not_Successfull.GetEnumDisplayName(), GoogleBackupArchiveLogType.UploadSection);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception exTemp = ex;
                string message = exTemp.Message;

                while (exTemp.InnerException != null)
                {
                    exTemp = exTemp.InnerException;
                    message += Environment.NewLine + exTemp.Message;
                }

                GoogleBackupArchiveLogService.Create(message, GoogleBackupArchiveLogType.UploadSection);
            }

        }
    }
}
