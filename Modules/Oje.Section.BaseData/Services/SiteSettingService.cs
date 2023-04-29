using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.BaseData.Interfaces;
using Oje.Section.BaseData.Models.View;
using Oje.Section.BaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.BaseData.Services.EContext;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;

namespace Oje.Section.BaseData.Services
{
    public class SiteSettingService : ISiteSettingService
    {
        readonly BaseDataDBContext db = null;
        readonly AccountService.Interfaces.ISiteSettingService GlobalSiteSettingService = null;
        readonly IUploadedFileService UploadedFileService = null;
        public SiteSettingService(
                BaseDataDBContext db,
                AccountService.Interfaces.ISiteSettingService GlobalSiteSettingService,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.GlobalSiteSettingService = GlobalSiteSettingService;
            this.UploadedFileService = UploadedFileService;
        }

        public ApiResult Create(CreateUpdateSiteSettingVM input, long? userId)
        {
            createUpdateValidation(input, userId);

            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    SiteSetting newItem = new SiteSetting()
                    {
                        Title = input.title,
                        SubTitle = input.subTitle,
                        WebsiteUrl = input.websiteUrl,
                        PanelUrl = input.panelUrl,
                        UserId = input.userId.Value,
                        CreateDate = DateTime.Now,
                        CreateUserId = userId.Value,
                        IsHttps = input.isHttps.ToBooleanReturnFalse(),
                        IsActive = input.isActive.ToBooleanReturnFalse(),
                        SeoMainPage = input.seo,
                        ParentId = input.pKey,
                        WebsiteType = input.websiteType == null ? WebsiteType.Normal : input.websiteType.Value,
                        CopyRightTitle = input.copyRightTitle
                    };

                    db.Entry(newItem).State = EntityState.Added;
                    db.SaveChanges();

                    if (input.minPic != null && input.minPic.Length > 0)
                    {
                        newItem.Image96 = UploadedFileService.UploadNewFile(FileType.MainLogo96, input.minPic, userId, null, newItem.Id, ".jpg,.png,.jpeg,.png", false, null);
                        newItem.Image192 = UploadedFileService.UploadNewFile(FileType.MainLogo192, input.minPic, userId, null, newItem.Id, ".jpg,.png,.jpeg,.png", false, null);
                        newItem.Image512 = UploadedFileService.UploadNewFile(FileType.MainLogo512, input.minPic, userId, null, newItem.Id, ".jpg,.png,.jpeg,.png", false, null);
                    }
                    if (input.textPic != null && input.textPic.Length > 0)
                        newItem.ImageText = UploadedFileService.UploadNewFile(FileType.MainLogoWithText, input.textPic, userId, null, newItem.Id, ".jpg,.png,.jpeg,.png", false, null);

                    if (input.minPicInvert != null && input.minPicInvert.Length > 0)
                        newItem.Image512Invert = UploadedFileService.UploadNewFile(FileType.MainLogo512, input.minPicInvert, userId, null, newItem.Id, ".jpg,.png,.jpeg,.png", false, null);

                    var foundTargetUser = db.Users.Where(t => t.Id == input.userId).FirstOrDefault();
                    if (foundTargetUser == null)
                        throw BException.GenerateNewException(BMessages.User_Not_Found, ApiResultErrorCode.ValidationError);
                    foundTargetUser.SiteSettingId = newItem.Id;
                    foundTargetUser.UpdateSignature();

                    newItem.FilledSignature();

                    db.SaveChanges();

                    tr.Commit();

                    GlobalSiteSettingService.UpdateSiteSettings();

                    return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        void createUpdateValidation(CreateUpdateSiteSettingVM input, long? userId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (userId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First, ApiResultErrorCode.NeedLoginFist);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.websiteUrl))
                throw BException.GenerateNewException(BMessages.Please_Enter_Website, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.panelUrl))
                throw BException.GenerateNewException(BMessages.Please_Enter_Panel_Url, ApiResultErrorCode.ValidationError);
            if (input.userId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_One_User, ApiResultErrorCode.ValidationError);
            if (db.SiteSettings.Any(t => t.WebsiteUrl == input.websiteUrl && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Website, ApiResultErrorCode.ValidationError);
            if (db.SiteSettings.Any(t => t.PanelUrl == input.panelUrl && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Panel, ApiResultErrorCode.ValidationError);
            //if (input.panelUrl == input.websiteUrl)
            //    throw BException.GenerateNewException(BMessages.PanelUrl_And_Website_Should_Be_Deffrent, ApiResultErrorCode.ValidationError);
            if (db.SiteSettings.Any(t => t.UserId == input.userId && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.User_Is_Used_In_Another_Setting, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.seo) && input.seo.Length > 4000)
                throw BException.GenerateNewException(BMessages.Seo_Can_Not_Be_More_Then_4000_Chars);
            if (input.id.ToIntReturnZiro() <= 0 && (input.minPic == null || input.minPic.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_Main_Image);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.SiteSettings.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (!foundItem.IsSignature())
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public object GetById(int? id)
        {
            return db.SiteSettings.Where(t => t.Id == id).Select(t => new
            {
                id = t.Id,
                title = t.Title,
                websiteUrl = t.WebsiteUrl,
                panelUrl = t.PanelUrl,
                userId = t.UserId,
                userId_Title = t.User.Username + "(" + t.User.Firstname + " " + t.User.Lastname + ")",
                isHttps = t.IsHttps,
                isActive = t.IsActive,
                seo = t.SeoMainPage,
                minPic_address = !string.IsNullOrEmpty(t.Image512) ? GlobalConfig.FileAccessHandlerUrl + t.Image512 : "",
                textPic_address = !string.IsNullOrEmpty(t.ImageText) ? GlobalConfig.FileAccessHandlerUrl + t.ImageText : "",
                minPicInvert_address = !string.IsNullOrEmpty(t.Image512Invert) ? GlobalConfig.FileAccessHandlerUrl + t.Image512Invert : "",
                websiteType = t.WebsiteType == null ? WebsiteType.Normal : t.WebsiteType.Value,
                copyRightTitle = t.CopyRightTitle,
                subTitle = t.SubTitle
            }).FirstOrDefault();
        }

        public GridResultVM<SiteSettingGridList> GetList(SiteSettingMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new SiteSettingMainGrid();

            var qureResult = db.SiteSettings.Where(t => t.ParentId == searchInput.pKey);


            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.userfirstname))
                qureResult = qureResult.Where(t => t.User.Firstname.Contains(searchInput.userfirstname));
            if (!string.IsNullOrEmpty(searchInput.userlastname))
                qureResult = qureResult.Where(t => t.User.Lastname.Contains(searchInput.userlastname));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.isHttps != null)
                qureResult = qureResult.Where(t => t.IsHttps);
            if (!string.IsNullOrEmpty(searchInput.website))
                qureResult = qureResult.Where(t => t.WebsiteUrl.Contains(searchInput.website));

            int row = searchInput.skip;

            return new GridResultVM<SiteSettingGridList>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    website = t.WebsiteUrl,
                    title = t.Title,
                    userfirstname = t.User.Firstname,
                    userlastname = t.User.Lastname,
                    isHttps = t.IsHttps,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new SiteSettingGridList
                {
                    row = ++row,
                    id = t.id,
                    website = t.website,
                    title = t.title,
                    userfirstname = t.userfirstname,
                    userlastname = t.userlastname,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    isHttps = t.isHttps == true ? "بلی" : "خیر"
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateSiteSettingVM input, long? userId)
        {
            createUpdateValidation(input, userId);

            SiteSetting editItem = db.SiteSettings.Where(t => t.Id == input.id).FirstOrDefault();
            if (editItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (!editItem.IsSignature())
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    editItem.Title = input.title;
                    editItem.SubTitle = input.subTitle;
                    editItem.WebsiteUrl = input.websiteUrl;
                    editItem.PanelUrl = input.panelUrl;
                    editItem.UserId = input.userId.Value;
                    editItem.UpdateDate = DateTime.Now;
                    editItem.UpdateUserId = userId.Value;
                    editItem.IsHttps = input.isHttps.ToBooleanReturnFalse();
                    editItem.IsActive = input.isActive.ToBooleanReturnFalse();
                    editItem.SeoMainPage = input.seo;
                    editItem.WebsiteType = input.websiteType == null ? WebsiteType.Normal : input.websiteType.Value;
                    editItem.CopyRightTitle = input.copyRightTitle;

                    db.SaveChanges();

                    if (input.minPic != null && input.minPic.Length > 0)
                    {
                        editItem.Image96 = UploadedFileService.UploadNewFile(FileType.MainLogo96, input.minPic, userId, null, editItem.Id, ".jpg,.png,.jpeg,.png", false, null);
                        editItem.Image192 = UploadedFileService.UploadNewFile(FileType.MainLogo192, input.minPic, userId, null, editItem.Id, ".jpg,.png,.jpeg,.png", false, null);
                        editItem.Image512 = UploadedFileService.UploadNewFile(FileType.MainLogo512, input.minPic, userId, null, editItem.Id, ".jpg,.png,.jpeg,.png", false, null);
                    }
                    if (input.textPic != null && input.textPic.Length > 0)
                        editItem.ImageText = UploadedFileService.UploadNewFile(FileType.MainLogoWithText, input.textPic, userId, null, editItem.Id, ".jpg,.png,.jpeg,.png", false, null);
                    if (input.minPicInvert != null && input.minPicInvert.Length > 0)
                        editItem.Image512Invert = UploadedFileService.UploadNewFile(FileType.MainLogo512, input.minPicInvert, userId, null, editItem.Id, ".jpg,.png,.jpeg,.png", false, null);

                    var foundTargetUser = db.Users.Where(t => t.Id == input.userId).FirstOrDefault();
                    if (foundTargetUser == null)
                        throw BException.GenerateNewException(BMessages.User_Not_Found, ApiResultErrorCode.ValidationError);
                    if (!foundTargetUser.IsSignature())
                        throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

                    foundTargetUser.SiteSettingId = editItem.Id;
                    foundTargetUser.UpdateSignature();
                    editItem.FilledSignature();

                    db.SaveChanges();

                    tr.Commit();

                    GlobalSiteSettingService.UpdateSiteSettings();
                    return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.SiteSettings.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
