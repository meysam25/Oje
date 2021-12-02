using Oje.AccountManager.Interfaces;
using Oje.AccountManager.Models.DB;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Oje.AccountManager.Services.EContext;

namespace Oje.AccountManager.Services
{
    public class UploadedFileManager : IUploadedFileManager
    {
        readonly AccountDBContext db = null;
        public UploadedFileManager(AccountDBContext db)
        {
            this.db = db;
        }
        public string UploadNewFile(FileType fileType, IFormFile userPic, long? loginUserId, int? siteSettingId, long? objectId, string extensions, bool isAccessRequired)
        {
            if (userPic == null || userPic.Length == 0)
                return "";

            UploadNewFileValidation(userPic, extensions);

            UploadedFile newFile = new UploadedFile()
            {
                CreateByUserId = loginUserId,
                FileType = fileType,
                IsFileAccessRequired = isAccessRequired,
                ObjectId = objectId,
                SiteSettingId = siteSettingId,
                FileName = "empty"
            };
            db.Entry(newFile).State = EntityState.Added;
            db.SaveChanges();

            var fi = new FileInfo(userPic.FileName);

            string subFolder = "";

            if (!string.IsNullOrEmpty(fileType.GetAttribute<DisplayAttribute>()?.Prompt))
                subFolder = fileType.GetAttribute<DisplayAttribute>()?.Prompt;

            string fn = newFile.Id + "_" + fi.Name.Replace(fi.Extension, "").Slugify() + fi.Extension;
            string tempFilePath = Path.Combine(GlobalConfig.GetUploadImageDirectory(subFolder));
            if (objectId != null)
                tempFilePath = Path.Combine(tempFilePath, objectId + "");
            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);

            using (var fs = new FileStream(Path.Combine(tempFilePath, fn), FileMode.Create))
            {
                if (isImage(fi.Extension) == true)
                {
                    using (Bitmap bmp = new Bitmap(userPic.OpenReadStream()))
                    {
                        var bmpSize = GetBmpSize(fileType.GetAttribute<DisplayAttribute>()?.Description, bmp);
                        using (var resultBmp = C_Image.ResizeBitmap(bmp, bmpSize))
                        {
                            saveImage(resultBmp, fs, fi.Extension);
                        }
                    }
                }
                else
                    userPic.CopyTo(fs);
            }

            newFile.FileName = GlobalConfig.GetUploadImageDirecotryRoot(subFolder) + "/" + fn;

            db.SaveChanges();


            return "?fn=" + fn;

        }

        void saveImage(Bitmap resultBmp, FileStream fs, string FileNameExtension)
        {
            ImageCodecInfo jpgEncoder = null;
            if (!string.IsNullOrEmpty(FileNameExtension) && FileNameExtension.ToLower() == ".png")
                jpgEncoder = GetEncoder(ImageFormat.Png);
            else
                jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            Encoder myEncoder = Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 70L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            resultBmp.Save(fs, jpgEncoder, myEncoderParameters);
        }

        ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        Size GetBmpSize(string bmpSize, Bitmap bmp)
        {
            Size result = new Size(300, 300);


            if (!string.IsNullOrEmpty(bmpSize) && bmpSize.IndexOf("*") > 0)
            {
                if (bmpSize.StartsWith("~"))
                {
                    bmpSize = bmpSize.Replace("~", "");
                    result = new Size(bmpSize.Split('*')[0].ToIntReturnZiro(), bmpSize.Split('*')[1].ToIntReturnZiro());
                    if (bmp.Width > result.Width)
                    {
                        var resultTemp = Convert.ToInt32(Convert.ToDecimal(bmp.Height * result.Width) / Convert.ToDecimal(bmp.Width));
                        result.Height = resultTemp;
                    }
                    else if (bmp.Height > result.Height)
                    {
                        var resultTemp = Convert.ToInt32(Convert.ToDecimal(bmp.Width * result.Height) / Convert.ToDecimal(bmp.Height));
                        result.Width = resultTemp;
                    }
                    else
                    {
                        result.Height = bmp.Height;
                        result.Width = bmp.Width;
                    }
                }
                else
                    result = new Size(bmpSize.Split('*')[0].ToIntReturnZiro(), bmpSize.Split('*')[1].ToIntReturnZiro());
            }

            return result;
        }

        bool isImage(string extension)
        {
            return extension.IndexOf(".png") > -1 || extension.IndexOf(".jpg") > -1 || extension.IndexOf(".jpeg") > -1;
        }

        void UploadNewFileValidation(IFormFile userPic, string extensions)
        {
            if (userPic.IsValidExtension(extensions) == false)
                throw BException.GenerateNewException(BMessages.File_Is_Not_Valid, ApiResultErrorCode.NotFound);
        }

        public UploadedFile GetFile(string fn, long? userId)
        {
            UploadedFile result = null;

            if (!string.IsNullOrEmpty(fn) && fn.IndexOf("_") > 0)
            {
                long foundId = fn.Split('_')[0].ToLongReturnZiro();
                if (foundId > 0)
                {
                    result = db.UploadedFiles
                        .Where(t => t.Id == foundId).FirstOrDefault();
                    if (result != null && result.IsFileAccessRequired == true)
                    {
                        if (userId <= 0)
                            result = null;
                        else if (userId > 0)
                        {
                            if (result.CreateByUserId != userId && !db.FileAccessRoles.Any(t => t.FileType == result.FileType && t.Role.UserRoles.Any(tt => tt.UserId == userId)))
                            {
                                result = null;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
