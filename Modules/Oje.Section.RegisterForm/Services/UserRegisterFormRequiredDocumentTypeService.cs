using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public class UserRegisterFormRequiredDocumentTypeService : IUserRegisterFormRequiredDocumentTypeService
    {
        readonly RegisterFormDBContext db = null;
        readonly IUserRegisterFormService UserRegisterFormService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        public UserRegisterFormRequiredDocumentTypeService
            (
                RegisterFormDBContext db,
                IUserRegisterFormService UserRegisterFormService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.UserRegisterFormService = UserRegisterFormService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(UserRegisterFormRequiredDocumentTypeCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new UserRegisterFormRequiredDocumentType()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                SiteSettingId = siteSettingId.Value,
                Title = input.title,
                UserRegisterFormId = input.userRegisterFormId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(UserRegisterFormRequiredDocumentTypeCreateUpdateVM input, int? siteSettingId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.userRegisterFormId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (UserRegisterFormService.GetById(input.userRegisterFormId, siteSettingId) == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.UserRegisterFormRequiredDocumentTypes
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.UserRegisterFormRequiredDocumentTypes
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    isActive = t.IsActive,
                    title = t.Title,
                    userRegisterFormId = t.UserRegisterFormId
                })
                .FirstOrDefault();
        }

        public GridResultVM<UserRegisterFormRequiredDocumentTypeMainGridResultVM> GetList(UserRegisterFormRequiredDocumentTypeMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new();

            var qureResult = db.UserRegisterFormRequiredDocumentTypes.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.formTitle))
                qureResult = qureResult.Where(t => t.UserRegisterForm.Title.Contains(searchInput.formTitle));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<UserRegisterFormRequiredDocumentTypeMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                    .OrderByDescending(t => t.Id)
                    .Skip(searchInput.skip)
                    .Take(searchInput.take)
                    .Select(t => new
                    {
                        id = t.Id,
                        formTitle = t.UserRegisterForm.Title,
                        title = t.Title,
                        isActvie = t.IsActive
                    })
                    .ToList()
                    .Select(t => new UserRegisterFormRequiredDocumentTypeMainGridResultVM
                    {
                        row = ++row,
                        id = t.id,
                        title = t.title,
                        formTitle = t.formTitle,
                        isActive = t.isActvie == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                    })
                    .ToList()
            };
        }

        public ApiResult Update(UserRegisterFormRequiredDocumentTypeCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.UserRegisterFormRequiredDocumentTypes
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Title = input.title;
            foundItem.UserRegisterFormId = input.userRegisterFormId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(
                db.UserRegisterFormRequiredDocumentTypes
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
