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
    public class UserRegisterFormPriceService : IUserRegisterFormPriceService
    {
        readonly RegisterFormDBContext db = null;
        public UserRegisterFormPriceService
            (
                RegisterFormDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(UserRegisterFormPriceCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var newItem = new UserRegisterFormPrice()
            {
                GroupPriceTitle = input.gp,
                GroupPriceTitle2 = input.gp2,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Price = input.price.ToLongReturnZiro(),
                Title = input.title,
                UserRegisterFormId = input.fid.Value
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            newItem.FilledSignature();
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(UserRegisterFormPriceCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.fid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.price.ToLongReturnZiro() < 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            if (!string.IsNullOrEmpty(input.gp) && input.gp.Length > 50)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (!db.UserRegisterForms.Any(t => t.Id == input.fid && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundnItem = db.UserRegisterFormPrices.Where(t => t.Id == id && t.UserRegisterForm.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundnItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundnItem.IsSignature())
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted);

            db.Entry(foundnItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.UserRegisterFormPrices
                .Where(t => t.Id == id && t.UserRegisterForm.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    fid = t.UserRegisterFormId,
                    title = t.Title,
                    price = t.Price,
                    isActive = t.IsActive,
                    gp = t.GroupPriceTitle,
                    gp2 = t.GroupPriceTitle2
                })
                .FirstOrDefault();
        }

        public GridResultVM<UserRegisterFormPriceMainGridResultVM> GetList(UserRegisterFormPriceMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.UserRegisterFormPrices.Where(t => t.UserRegisterForm.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.formTitle))
                quiryResult = quiryResult.Where(t => t.UserRegisterForm.Title.Contains(searchInput.formTitle));
            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.price != null)
                quiryResult = quiryResult.Where(t => t.Price == searchInput.price);

            int row = searchInput.skip;

            return new GridResultVM<UserRegisterFormPriceMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    formTitle = t.UserRegisterForm.Title,
                    title = t.Title,
                    price = t.Price,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new UserRegisterFormPriceMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    formTitle = t.formTitle,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    price = t.price.ToString("###,###"),
                    title = t.title
                })
                .ToList()
            };
        }

        public ApiResult Update(UserRegisterFormPriceCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundnItem = db.UserRegisterFormPrices.Where(t => t.Id == input.id && t.UserRegisterForm.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundnItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundnItem.IsSignature())
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

            foundnItem.GroupPriceTitle = input.gp;
            foundnItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundnItem.Price = input.price.ToLongReturnZiro();
            foundnItem.Title = input.title;
            foundnItem.UserRegisterFormId = input.fid.Value;
            foundnItem.GroupPriceTitle2 = input.gp2;
            foundnItem.FilledSignature();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList(int? siteSettingId, int? formId, string groupKey, string groupKey2)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.UserRegisterFormPrices.Where(t => t.IsActive == true && t.UserRegisterFormId == formId && t.UserRegisterForm.SiteSettingId == siteSettingId && t.GroupPriceTitle == groupKey && t.GroupPriceTitle2 == groupKey2).OrderBy(t => t.Price).Select(t => new
            {
                id = t.Id,
                title = t.Title
            }).ToList());

            return result;
        }

        public UserRegisterFormPrice GetPriceBy(int formId, int? siteSettingId, int id, string groupTitle, string groupTitle2)
        {
            if (!string.IsNullOrEmpty(groupTitle) && !string.IsNullOrEmpty(groupTitle2))
                return db.UserRegisterFormPrices.Where(t => t.UserRegisterFormId == formId && t.UserRegisterForm.SiteSettingId == siteSettingId && t.Id == id && t.IsActive == true && t.GroupPriceTitle == groupTitle && t.GroupPriceTitle2 == groupTitle2).AsNoTracking().FirstOrDefault();
            else if (!string.IsNullOrEmpty(groupTitle))
                return db.UserRegisterFormPrices.Where(t => t.UserRegisterFormId == formId && t.UserRegisterForm.SiteSettingId == siteSettingId && t.Id == id && t.IsActive == true && t.GroupPriceTitle == groupTitle).AsNoTracking().FirstOrDefault();
            else if (!string.IsNullOrEmpty(groupTitle2))
                return db.UserRegisterFormPrices.Where(t => t.UserRegisterFormId == formId && t.UserRegisterForm.SiteSettingId == siteSettingId && t.Id == id && t.IsActive == true && t.GroupPriceTitle2 == groupTitle2).AsNoTracking().FirstOrDefault();
            else
                return db.UserRegisterFormPrices.Where(t => t.UserRegisterFormId == formId && t.UserRegisterForm.SiteSettingId == siteSettingId && t.Id == id && t.IsActive == true).AsNoTracking().FirstOrDefault();
        }

        public UserRegisterFormPrice GetById(int id)
        {
            return db.UserRegisterFormPrices.Where(t => t.Id == id && t.IsActive == true).AsNoTracking().FirstOrDefault();
        }
    }
}
