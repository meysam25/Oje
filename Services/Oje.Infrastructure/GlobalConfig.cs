using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Oje.Models;
using Oje.Infrastructure.Models;

namespace Oje.Infrastructure
{
    public static class GlobalConfig
    {
        public static IConfiguration Configuration { get; set; }
        public static IWebHostEnvironment WebHostEnvironment { get; set; }
        public static List<Assembly> Moduals { get; set; }
        private static string UploadImageFolderName { get { return "UploadImages"; } }
        public static int siteMenuCache { get; set; } = 0;

        public static string FileAccessHandlerUrl { get { return "/Core/BaseData/GetFile"; } }

        public static WebAppSettings GetAppSettings()
        {
            return Configuration.GetSection("appSettings").Get<WebAppSettings>();
        }

        public static string GetSiteMapBaseFolder()
        {
            string result = Path.Combine(WebHostEnvironment.ContentRootPath, "SiteMaps");
            if (!Directory.Exists(result))
                Directory.CreateDirectory(result);
            return result;
        }

        public static string GetAppVersion()
        {
            return Configuration.GetSection("ver").Get<string>();
        }

        public static string GetJsonConfigFile(string area, string controller)
        {
            return WebHostEnvironment.WebRootPath + "\\Modules\\" + area + "JsonConfigs\\" + controller + ".json";
        }

        public static string GetUploadImageDirectory(string cPath)
        {
            var result = Path.Combine(WebHostEnvironment.ContentRootPath, UploadImageFolderName);
            if (!string.IsNullOrEmpty(cPath))
                result = Path.Combine(result, cPath);
            if (Directory.Exists(result) == false)
                Directory.CreateDirectory(result);

            return result;
        }

        public static string GetUploadImageDirecotryRoot(string cPath)
        {
            var result = "/" + UploadImageFolderName;

            if (!string.IsNullOrEmpty(cPath))
                result = result + "/" + cPath;

            return result;
        }

        private static List<SmsLimit> smsLImitsCache = null;

        public static List<SmsLimit> GetSmsLimitFromConfig()
        {
            try
            {
                if(smsLImitsCache == null)
                    smsLImitsCache = Configuration.GetSection("SmsLimits").Get<List<SmsLimit>>();
                return smsLImitsCache;
            }
            catch
            {
                return null;
            }
        }

        static List<string> supportRolesCache = null;
        public static List<string> GetValidRoleForSupports()
        {
            try
            {
                if (supportRolesCache == null)
                    supportRolesCache = Configuration.GetSection("SupportRoles").Get<List<string>>();
                return supportRolesCache;
            }
            catch
            {
                return null;
            }
        }

        public static string replaceInvalidChars(string input)
        {
            return System.Net.WebUtility.UrlEncode((input + "").Trim().Replace(" ", "-").Replace("--", "-"));
        }
    }
}
