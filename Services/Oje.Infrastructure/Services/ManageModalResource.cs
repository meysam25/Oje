using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Services
{
    public static class ManageModalResource
    {
        public static void Copy()
        {
            if (GlobalConfig.GetAppSettings()?.CopyModalResource == true)
            {
                string wrp = GlobalConfig.WebHostEnvironment.WebRootPath;
                string distinationPath = wrp + "\\Modules";
                if (Directory.Exists(distinationPath) == false)
                {
                    Directory.CreateDirectory(distinationPath);
                }
                else
                {
                    Directory.Delete(distinationPath, true);
                    Directory.CreateDirectory(distinationPath);
                }
                string crp = GlobalConfig.WebHostEnvironment.ContentRootPath;
                var allParts = crp.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                allParts.Remove(allParts.Last());
                string basePath = string.Join('\\', allParts) + "\\Modules";
                var allDreictory = Directory.GetDirectories(basePath);
                foreach (var pathItem in allDreictory)
                {
                    string baseModPath = pathItem + "\\wwwroot";
                    var allModDirectory = Directory.GetDirectories(baseModPath);
                    if (allModDirectory.Length > 0)
                    {
                        foreach (var pathItem2 in allModDirectory)
                        {
                            string baseModPathWithBaseFolderName = pathItem2;
                            var allPartX = baseModPathWithBaseFolderName.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (Char.IsUpper(allPartX.Last()[0]))
                            {
                                var newPartX = distinationPath + "\\" + allPartX.Last();
                                if (Directory.Exists(newPartX) == false)
                                {
                                    Directory.CreateDirectory(newPartX);
                                }
                                CopyAll(new DirectoryInfo(baseModPathWithBaseFolderName), new DirectoryInfo(newPartX));
                            }
                        }


                    }
                }

            }
        }

        static void Compress(FileInfo fileToCompress)
        {
            using (FileStream originalFileStream = fileToCompress.OpenRead())
            {
                if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                {
                    using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressionStream);
                        }
                    }
                }
            }
        }

        static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                string targetPath = Path.Combine(target.FullName, fi.Name);
                fi.CopyTo(targetPath, true);
                Compress(new FileInfo(targetPath));
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
