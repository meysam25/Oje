using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Models.View;
using Oje.Section.RegisterForm.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.RegisterForm.Services
{
    public class UserRegisterFormService : IUserRegisterFormService
    {
        readonly RegisterFormDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public UserRegisterFormService
            (
                RegisterFormDBContext db,
                IUploadedFileService UploadedFileService,
                IHttpContextAccessor HttpContextAccessor)
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(UserRegisterFormCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;

            createValidation(input, siteSettingId, canSetSiteSetting);

            var newItem = new UserRegisterForm()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                JsonConfig = input.jConfig,
                Name = input.name,
                PaymentUserId = input.userId,
                SeoDescription = input.description,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Title = input.title,
                TermDescription = input.termT,
                Icon = input.icon
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.rules != null && input.rules.Length > 0)
                newItem.RuleFile = UploadedFileService.UploadNewFile(FileType.UserRegisterFormRules, input.rules, null, null, newItem.Id, ".pdf,.doc,.docx", false);
            if (input.secoundFile != null && input.secoundFile.Length > 0)
                newItem.SecountFile = UploadedFileService.UploadNewFile(FileType.UserRegisterFormRules, input.secoundFile, null, null, newItem.Id, ".pdf,.doc,.docx", false);
            if (input.anotherFile != null && input.anotherFile.Length > 0)
                newItem.AnotherFile = UploadedFileService.UploadNewFile(FileType.UserRegisterFormRules, input.anotherFile, null, null, newItem.Id, ".pdf,.doc,.docx", false);
            if (input.anotherFile2 != null && input.anotherFile2.Length > 0)
                newItem.AnotherFile2 = UploadedFileService.UploadNewFile(FileType.UserRegisterFormRules, input.anotherFile2, null, null, newItem.Id, ".pdf,.doc,.docx", false);
            if (input.roleIds != null)
                foreach (var roleid in input.roleIds)
                    db.Entry(new UserRegisterFormRole() { RoleId = roleid, UserRegisterFormId = newItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(UserRegisterFormCreateUpdateVM input, int? siteSettingId, bool? canSetSiteSetting)
        {


            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 300)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_300_chars);
            if (string.IsNullOrEmpty(input.name))
                throw BException.GenerateNewException(BMessages.Please_Enter_Name);
            if (input.name.Length > 300)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (string.IsNullOrEmpty(input.jConfig))
                throw BException.GenerateNewException(BMessages.Please_Enter_Json_Config);
            if (string.IsNullOrEmpty(input.description))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (string.IsNullOrEmpty(input.termT))
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input.termT.Length > 4000)
                throw BException.GenerateNewException(BMessages.Validation_Error);

            if (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0)
                siteSettingId = input.cSOWSiteSettingId;
            if (input.userId.ToLongReturnZiro() > 0 && !db.Users.Any(t => t.SiteSettingId == siteSettingId && t.Id == input.userId))
                throw BException.GenerateNewException(BMessages.User_Not_Found);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.UserRegisterForms
                .Include(t => t.UserRegisterFormRoles)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.UserRegisterFormRoles != null)
                foreach (var role in foundItem.UserRegisterFormRoles)
                    db.Entry(role).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public UserRegisterFormCreateUpdateVM GetById(int? id, int? siteSettingId)
        {
            return db.UserRegisterForms
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new UserRegisterFormCreateUpdateVM
                {
                    id = t.Id,
                    isActive = t.IsActive,
                    jConfig = t.JsonConfig,
                    name = t.Name,
                    userId = t.PaymentUserId,
                    userId_Title = t.PaymentUserId > 0 ? t.PaymentUser.Username + " (" + t.PaymentUser.Firstname + " " + t.PaymentUser.Lastname + ")" : "",
                    description = t.SeoDescription,
                    title = t.Title,
                    termT = t.TermDescription,
                    roleIds = t.UserRegisterFormRoles.Select(tt => tt.RoleId).ToList(),
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title,
                    icon = t.Icon
                }).FirstOrDefault();
        }

        public GridResultVM<UserRegisterFormMainGridResultVM> GetList(UserRegisterFormMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new();

            var queryResult = db.UserRegisterForms.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                queryResult = queryResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.name))
                queryResult = queryResult.Where(t => t.Name.Contains(searchInput.name));
            if (searchInput.isActive != null)
                queryResult = queryResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.userfullname))
                queryResult = queryResult.Where(t => t.PaymentUserId != null && (t.PaymentUser.Firstname + " " + t.PaymentUser.Lastname).Contains(searchInput.userfullname));
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                queryResult = queryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));
            int row = searchInput.skip;

            return new GridResultVM<UserRegisterFormMainGridResultVM>()
            {
                total = queryResult.Count(),
                data = queryResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    name = t.Name,
                    isActive = t.IsActive,
                    t.PaymentUser.Firstname,
                    t.PaymentUser.Lastname,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new UserRegisterFormMainGridResultVM
                {
                    id = t.id,
                    row = ++row,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    name = t.name,
                    title = t.title,
                    userfullname = t.Firstname + " " + t.Lastname,
                    url = "/Register/Users/" + t.name + "?fid=" + t.id,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(UserRegisterFormCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;

            createValidation(input, siteSettingId, canSetSiteSetting);

            var foundItem = db.UserRegisterForms
                .Include(t => t.UserRegisterFormRoles)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.JsonConfig = input.jConfig;
            foundItem.Name = input.name;
            foundItem.PaymentUserId = input.userId;
            foundItem.SeoDescription = input.description;
            foundItem.Title = input.title;
            foundItem.TermDescription = input.termT;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;
            foundItem.Icon = input.icon;

            if (foundItem.UserRegisterFormRoles != null)
                foreach (var role in foundItem.UserRegisterFormRoles)
                    db.Entry(role).State = EntityState.Deleted;

            if (input.rules != null && input.rules.Length > 0)
                foundItem.RuleFile = UploadedFileService.UploadNewFile(FileType.UserRegisterFormRules, input.rules, null, null, foundItem.Id, ".pdf,.doc,.docx", false);
            if (input.secoundFile != null && input.secoundFile.Length > 0)
                foundItem.SecountFile = UploadedFileService.UploadNewFile(FileType.UserRegisterFormRules, input.secoundFile, null, null, foundItem.Id, ".pdf,.doc,.docx", false);
            if (input.anotherFile != null && input.anotherFile.Length > 0)
                foundItem.AnotherFile = UploadedFileService.UploadNewFile(FileType.UserRegisterFormRules, input.anotherFile, null, null, foundItem.Id, ".pdf,.doc,.docx", false);
            if (input.anotherFile2 != null && input.anotherFile2.Length > 0)
                foundItem.AnotherFile2 = UploadedFileService.UploadNewFile(FileType.UserRegisterFormRules, input.anotherFile2, null, null, foundItem.Id, ".pdf,.doc,.docx", false);

            if (input.roleIds != null)
                foreach (var roleid in input.roleIds)
                    db.Entry(new UserRegisterFormRole() { RoleId = roleid, UserRegisterFormId = foundItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(
                db.UserRegisterForms
                .Where(t => t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title
                })
                .ToList());

            return result;
        }

        public bool Exist(int? id, int? siteSettingId)
        {
            return db.UserRegisterForms.Any(t => t.Id == id && t.SiteSettingId == siteSettingId && t.IsActive == true);
        }

        public string GetConfigJson(int? id, int? isiteSettingId)
        {
            return db.UserRegisterForms.Where(t => t.Id == id && t.SiteSettingId == isiteSettingId && t.IsActive == true).Select(t => t.JsonConfig).FirstOrDefault();
        }

        public UserRegisterForm GetBy(string formName, int? id, int? iisiteSettingId)
        {
            return db.UserRegisterForms.Where(t => t.Id == id && t.SiteSettingId == iisiteSettingId && t.Name == formName && t.IsActive == true).Select(t => new UserRegisterForm { Title = t.Title, SeoDescription = t.SeoDescription }).FirstOrDefault();
        }

        public UserRegisterForm GetTermInfo(int id, int? siteSettingId)
        {
            return db.UserRegisterForms.Where(t => t.Id == id && t.SiteSettingId == siteSettingId && t.IsActive == true).Select(t => new UserRegisterForm
            {
                TermDescription = t.TermDescription,
                RuleFile = t.RuleFile,

            }).FirstOrDefault();
        }

        public string GetSecoundFileUrl(int? id, int? siteSettingId)
        {
            var tempStrResult = db.UserRegisterForms.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).Select(t => t.SecountFile).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempStrResult))
                tempStrResult = GlobalConfig.FileAccessHandlerUrl + tempStrResult;
            return tempStrResult;
        }

        public string GetAnotherFileUrl(int? id, int? siteSettingId)
        {
            var tempStrResult = db.UserRegisterForms.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).Select(t => t.AnotherFile).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempStrResult))
                tempStrResult = GlobalConfig.FileAccessHandlerUrl + tempStrResult;
            return tempStrResult;
        }

        public object GetLightList2(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(
                db.UserRegisterForms
                .Where(t => t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = "/" + t.Name + "?fid=" + t.Id,
                    title = t.Title
                }).ToList());

            return result;
        }

        public string GetAnotherFile2Url(int? id, int? siteSettingId)
        {
            var tempStrResult = db.UserRegisterForms.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).Select(t => t.AnotherFile2).FirstOrDefault();
            if (!string.IsNullOrEmpty(tempStrResult))
                tempStrResult = GlobalConfig.FileAccessHandlerUrl + tempStrResult;
            return tempStrResult;
        }

        public List<KeyValue> GetAllConfig(int? siteSettingId)
        {
            return db.UserRegisterForms
                .Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true)
                .Select(t => new KeyValue
                {
                    key = t.Id + "",
                    value = t.Title,
                    extera = t.Icon
                }).AsNoTracking().ToList();
        }
    }
}
