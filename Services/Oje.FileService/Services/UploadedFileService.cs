using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using Oje.FileService.Services.EContext;
using Oje.FileService.Interfaces;
using Oje.FileService.Models.DB;
using Imazen.WebP;
using Microsoft.AspNetCore.StaticFiles;
using Oje.FileService.Models.View;
using Oje.Infrastructure.Models;
using Microsoft.AspNetCore.Rewrite;

namespace Oje.FileService.Services
{
    public class UploadedFileService : IUploadedFileService
    {
        readonly FileDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        public UploadedFileService
            (
                FileDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }
        public string UploadNewFile(FileType fileType, IFormFile userPic, long? loginUserId, int? siteSettingId, long? objectId, string extensions, bool isAccessRequired, string objectIdStr = null, string title = null, long? userId = null)
        {
            if (userPic == null || userPic.Length == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_File);

            UploadNewFileValidation(userPic, extensions);

            UploadedFile newFile = new UploadedFile()
            {
                CreateByUserId = loginUserId,
                FileType = fileType,
                IsFileAccessRequired = isAccessRequired,
                ObjectId = objectId,
                SiteSettingId = siteSettingId,
                FileName = "empty",
                ObjectIdStr = objectIdStr,
                Title = title,
                UserId = userId
            };
            db.Entry(newFile).State = EntityState.Added;
            db.SaveChanges();

            var fi = new FileInfo(userPic.FileName);

            string subFolder = "";
            string bitmapSizeStr = fileType.GetAttribute<DisplayAttribute>()?.Description + "";

            bool? dontCovertToWebP = false;

            try { dontCovertToWebP = fileType.GetAttribute<DisplayAttribute>()?.AutoGenerateFilter; } catch { }

            if (!string.IsNullOrEmpty(fileType.GetAttribute<DisplayAttribute>()?.Prompt))
                subFolder = fileType.GetAttribute<DisplayAttribute>()?.Prompt;

            string fn = newFile.Id + "_" + fi.Name.Replace(fi.Extension, "").Slugify() + (!string.IsNullOrEmpty(bitmapSizeStr) ? ("-" + bitmapSizeStr.Replace("*", "x")) : "") + ((isImage(fi.Extension) && dontCovertToWebP == false) ? ".webp" : fi.Extension);
            string tempFilePath = Path.Combine(GlobalConfig.GetUploadImageDirectory(subFolder));
            if (objectId != null)
                tempFilePath = Path.Combine(tempFilePath, objectId + "");
            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);

            using (var fs = new FileStream(Path.Combine(tempFilePath, fn), FileMode.Create))
            {
                if (isImage(fi.Extension) == true)
                {
                    if (OperatingSystem.IsWindows())
                    {
                        using (Bitmap bmp = new Bitmap(userPic.OpenReadStream()))
                        {
                            var bmpSize = GetBmpSize(bitmapSizeStr, bmp);
                            bool? isSizeValidationRequired = false;
                            try { isSizeValidationRequired = fileType.GetAttribute<DisplayAttribute>()?.AutoGenerateField; } catch { }
                            if (isSizeValidationRequired == true)
                            {
                                if (bmp.Width != bmpSize.Width || bmp.Height != bmpSize.Height)
                                {
                                    db.Entry(newFile).State = EntityState.Deleted;
                                    db.SaveChanges();
                                    throw BException.GenerateNewException(BMessages.Invalid_Image_Size);
                                }
                            }
                            using (var resultBmp = C_Image.ResizeBitmap(bmp, bmpSize))
                            {
                                saveImage(resultBmp, fs, fi.Extension, dontCovertToWebP);
                            }
                        }
                    }
                    else
                        throw new NotImplementedException("UploadNewFile");
                }
                else
                    userPic.CopyTo(fs);
            }

            string FileContentType = " ";

            newFile.FileName = GlobalConfig.GetUploadImageDirecotryRoot(subFolder) + (objectId != null ? "/" + objectId : "") + "/" + fn;
            newFile.FileSize = new FileInfo(newFile.FileNameOnServer).Length;
            new FileExtensionContentTypeProvider().TryGetContentType(new FileInfo(newFile.FileName).Name, out FileContentType);
            newFile.FileContentType = FileContentType;
            db.SaveChanges();

            return "?fn=" + fn;

        }

        void saveImage(Bitmap resultBmp, FileStream fs, string FileNameExtension, bool? dontCovertToWebP)
        {
            if (OperatingSystem.IsWindows())
            {
                if (dontCovertToWebP == true)
                {
                    ImageCodecInfo jpgEncoder = null;
                    if (!string.IsNullOrEmpty(FileNameExtension) && FileNameExtension.ToLower() == ".png")
                        jpgEncoder = GetEncoder(ImageFormat.Png);
                    else
                        jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    Encoder myEncoder = Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    resultBmp.Save(fs, jpgEncoder, myEncoderParameters);
                }
                else
                {
                    Imazen.WebP.Extern.LoadLibrary.LoadWebPOrFail();
                    var encoder = new SimpleEncoder();
                    encoder.Encode(resultBmp, fs, 90);
                }
            }
            else
                throw new NotImplementedException("saveImage");
        }

        static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            if (OperatingSystem.IsWindows())
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
            else
                throw new NotImplementedException("GetEncoder");

        }

        static Size GetBmpSize(string bmpSize, Bitmap bmp)
        {
            if (OperatingSystem.IsWindows())
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
            else
                throw new NotImplementedException("GetBmpSize");

        }

        static bool isImage(string extension)
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
                        .AsNoTracking()
                        .Where(t => t.Id == foundId)
                        .FirstOrDefault();
                    if (result != null && result.IsFileAccessRequired == true)
                    {
                        if (userId <= 0)
                            result = null;
                        else if (userId > 0)
                        {
                            if (
                                    (result.UserId != userId && result.CreateByUserId != userId && !db.FileAccessRoles.Any(t => t.FileType == result.FileType && t.Role.UserRoles.Any(tt => tt.UserId == userId)))
                                    &&
                                    !db.UploadedFiles.Where(t => t.Id == result.Id).getWhereCreateByUserMultiLevelForUserOwnerShip<UploadedFile, User>(userId, false).Any()

                               )
                            {
                                result = null;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public int GetCountBy(long objectId, FileType fileType)
        {
            return db.UploadedFiles.Count(t => t.ObjectId == objectId && t.FileType == fileType);
        }

        public object GetListBy(long objectId, FileType fileType, int skip, int take)
        {
            return db.UploadedFiles.OrderByDescending(t => t.Id)
                .Where(t => t.ObjectId == objectId && t.FileType == fileType)
                .Skip(skip).Take(take)
                .Select(t => new
                {
                    id = t.Id,
                    src = t.FileName,
                    title = t.Title
                })
                .ToList()
                .Select(t => new
                {
                    t.id,
                    src = !string.IsNullOrEmpty(t.src) && (isImage(Path.GetFileName(t.src)) || t.src.ToLower().EndsWith(".webp")) ? GlobalConfig.FileAccessHandlerUrl + "?fn=" + Path.GetFileName(t.src) : "/Modules/Images/unknown.svg",
                    title = string.IsNullOrEmpty(t.title) ? "" : t.title
                })
                .ToList()
                ;
        }

        public void Delete(long? uploadFileId, int? siteSettingId, long? objectId, FileType fileType)
        {
            var foundItem = db.UploadedFiles.Where(t => t.SiteSettingId == siteSettingId && t.Id == uploadFileId && t.FileType == fileType && t.ObjectId == objectId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();
        }

        public bool IsValidImageSize(IFormFile mainImage, bool isWidthCheck, decimal relatedRateStart, decimal relatedRateEnd)
        {
            if (OperatingSystem.IsWindows())
            {
                using (Bitmap bmp = new Bitmap(mainImage.OpenReadStream()))
                {
                    if (bmp.Width == 0 || bmp.Height == 0)
                        return false;
                    //if (isWidthCheck == true)
                    //{
                    //    if (bmp.Width < bmp.Height)
                    //        return false;
                    //}
                    //else if (bmp.Height < bmp.Width)
                    //    return false;
                    if (bmp.Width > bmp.Height)
                    {
                        var currRate = Convert.ToDecimal(bmp.Width) / Convert.ToDecimal(bmp.Height);
                        if (currRate > relatedRateEnd || currRate < relatedRateStart)
                            return false;
                    }
                    else
                    {
                        var currRate = Convert.ToDecimal(bmp.Height) / Convert.ToDecimal(bmp.Width);
                        if (currRate > relatedRateEnd || currRate < relatedRateStart)
                            return false;
                    }
                    return true;
                }
            }

            return false;
        }

        public int GetCountBy(long objectId, FileType fileType, int? siteSettingId)
        {
            return db.UploadedFiles.Count(t => t.ObjectId == objectId && t.FileType == fileType && t.SiteSettingId == siteSettingId);
        }

        public object GetListBy(long objectId, FileType fileType, int skip, int take, int? siteSettingId)
        {
            return db.UploadedFiles.OrderByDescending(t => t.Id)
               .Where(t => t.ObjectId == objectId && t.FileType == fileType && t.SiteSettingId == siteSettingId)
               .Skip(skip).Take(take)
               .Select(t => new
               {
                   id = t.Id,
                   src = t.FileName,
                   title = t.Title
               })
               .ToList()
               .Select(t => new
               {
                   t.id,
                   src = !string.IsNullOrEmpty(t.src) && (isImage(Path.GetFileName(t.src)) || t.src.ToLower().EndsWith(".webp")) ? GlobalConfig.FileAccessHandlerUrl + "?fn=" + Path.GetFileName(t.src) : "/Modules/Images/unknown.svg",
                   title = string.IsNullOrEmpty(t.title) ? "" : t.title

               })
               .ToList()
               ;
        }

        public object Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.UploadedFiles
                .getSiteSettingQuiryNullable(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetList(UploadedFileMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.UploadedFiles.getSiteSettingQuiryNullable(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            return new
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    fileSection = t.FileType,
                    fromUsername = t.CreateByUserId > 0 ? t.CreateByUser.Username : "",
                    fromFirstname = t.CreateByUserId > 0 ? t.CreateByUser.Firstname : "",
                    fromLastname = t.CreateByUserId > 0 ? t.CreateByUser.Lastname : "",
                    toUsername = t.UserId > 0 ? t.User.Username : "",
                    toFirstname = t.UserId > 0 ? t.User.Firstname : "",
                    toLastname = t.UserId > 0 ? t.User.Lastname : "",
                    t.IsFileAccessRequired,
                    t.FileName
                })
                .ToList()
                .Select(t => new
                {
                    t.id,
                    fileSection = t.fileSection.GetEnumDisplayName(),
                    user1Fullname1 = !string.IsNullOrEmpty((t.fromFirstname + " " + t.fromLastname).Trim()) ? t.fromFirstname + " " + t.fromLastname : t.fromUsername,
                    user1Fullname2 = !string.IsNullOrEmpty((t.toFirstname + " " + t.toLastname).Trim()) ? t.toFirstname + " " + t.toLastname : t.toUsername,
                    requiredAccess = t.IsFileAccessRequired == true ? BMessages.Need_Access.GetEnumDisplayName() : BMessages.Dont_Need_Access.GetEnumDisplayName(),
                    src = GlobalConfig.FileAccessHandlerUrl + "?fn=" + (new FileInfo(t.FileName).Name)
                })
                .ToList()
            };
        }

        public UploadedFile GetFile(string fn)
        {
            UploadedFile result = null;

            if (!string.IsNullOrEmpty(fn) && fn.IndexOf("_") > 0)
            {
                long foundId = fn.Split('_')[0].ToLongReturnZiro();
                if (foundId > 0)
                {
                    result = db.UploadedFiles
                        .AsNoTracking()
                        .Where(t => t.Id == foundId)
                        .FirstOrDefault();
                }
            }

            return result;
        }

        public List<string> GetFileUrlList(long objectId, FileType fileType, int skip, int take, int? siteSettingId)
        {
            return 
            db.UploadedFiles.OrderByDescending(t => t.Id)
               .Where(t => t.ObjectId == objectId && t.FileType == fileType && t.SiteSettingId == siteSettingId)
               .Skip(skip).Take(take)
               .Select(t => new
               {
                   src = t.FileName
               })
               .ToList()
               .Select(t => !string.IsNullOrEmpty(t.src) && (isImage(Path.GetFileName(t.src)) || t.src.ToLower().EndsWith(".webp")) ? GlobalConfig.FileAccessHandlerUrl + "?fn=" + Path.GetFileName(t.src) : "/Modules/Images/unknown.svg")
               .ToList()
               ;
        }
    }
}
