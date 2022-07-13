using System;
using System.IO;

namespace Oje.Infrastructure.Services
{
    public static class MySession
    {
        public static void Create(string name)
        {
            string basePath = CreateDirecotryIfNotExist();
            string fileName = Path.Combine(basePath, name);
            if (!File.Exists(fileName))
                using (File.Create(fileName)) { }
        }

        static string CreateDirecotryIfNotExist()
        {
            string directoryPath = Path.Combine(GlobalConfig.WebHostEnvironment.ContentRootPath, "KeyValues");
            directoryPath = Path.Combine(directoryPath, "mySessionDirecotry");
            directoryPath = Path.Combine(directoryPath, DateTime.Now.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            return directoryPath;
        }

        public static void Clean(string name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                string basePath = CreateDirecotryIfNotExist();
                string fileName = Path.Combine(basePath, name);
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
        }

        public static bool IsFileExist(string name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                string basePath = CreateDirecotryIfNotExist();
                string fileName = Path.Combine(basePath, name);
                return File.Exists(fileName);
            }
            return false;
        }

        public static void WriteData(string name, string data)
        {
            if(!string.IsNullOrEmpty(name))
            {
                string basePath = CreateDirecotryIfNotExist();
                string fileName = Path.Combine(basePath, name);
                if (File.Exists(fileName))
                    File.WriteAllText(fileName, data);
            }
        }

        public static void ReadData(string name, string data)
        {
            if(!string.IsNullOrEmpty(name))
            {
                string basePath = CreateDirecotryIfNotExist();
                string fileName = Path.Combine(basePath, name);
                if (File.Exists(fileName))
                    File.WriteAllText(fileName, data);
            }
        }
    }
}
