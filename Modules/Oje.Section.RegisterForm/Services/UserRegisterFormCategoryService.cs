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
    public class UserRegisterFormCategoryService : IUserRegisterFormCategoryService
    {
        readonly RegisterFormDBContext db = null;
        public UserRegisterFormCategoryService
            (
                RegisterFormDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(UserRegisterFormCategoryCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new UserRegisterFormCategory()
            {
                Title = input.title,
                Icon = input.icon,
                SiteSettingId = siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(UserRegisterFormCategoryCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);

            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (string.IsNullOrEmpty(input.icon))
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.UserRegisterFormCategories.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.UserRegisterFormCategories
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    icon = t.Icon
                })
                .FirstOrDefault();
        }

        public GridResultVM<UserRegisterFormCategoryMainGridResultVM> GetList(UserRegisterFormCategoryMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new UserRegisterFormCategoryMainGrid();

            var quiryResult = db.UserRegisterFormCategories.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));

            int row = searchInput.skip;

            return new GridResultVM<UserRegisterFormCategoryMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                    .OrderByDescending(t => t.Id)
                    .Skip(searchInput.skip)
                    .Take(searchInput.take)
                    .Select(t => new
                    {
                        id = t.Id,
                        title = t.Title
                    })
                    .ToList()
                    .Select(t => new UserRegisterFormCategoryMainGridResultVM
                    {
                        row = ++row,
                        id = t.id,
                        title = t.title
                    })
                    .ToList()
            };
        }

        public ApiResult Update(UserRegisterFormCategoryCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.UserRegisterFormCategories.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Title = input.title;
            foundItem.Icon = input.icon;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.UserRegisterFormCategories
                .Where(t => t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title
                })
                .ToList());

            return result;
        }
    }
}
