using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Oje.BackupService.Interfaces;
using Oje.Infrastructure;

namespace Oje.BackupService.Services
{
    public class FileHelperService: IFileHelperService
    {
        public string GetTargetDirectory(bool ignoreTodayFolder)
        {
            string result = Path.Combine(GlobalConfig.Configuration.GetValue<string>(WebHostDefaults.ContentRootKey), "Backups");

            if (!Directory.Exists(result))
                Directory.CreateDirectory(result);

            if (ignoreTodayFolder == false)
            {
                result = Path.Combine(result, DateTime.Now.ToString("yyyyMMdd"));

                if (!Directory.Exists(result))
                    Directory.CreateDirectory(result);
            }

            return result;
        }

        public string GetTempImageFileName()
        {
            return Path.Combine(GetTargetDirectory(false), "imageTemp.zip");
        }

        public string GetTodayFileName()
        {
            return Path.Combine(GetTargetDirectory(true), DateTime.Now.ToString("yyyy-MM-dd HH-mm") + ".zip");
        }

        public string GetTempBackupFileName()
        {
            return Path.Combine(GetTargetDirectory(false), "tempBack.BAK");
        }
    }
}
