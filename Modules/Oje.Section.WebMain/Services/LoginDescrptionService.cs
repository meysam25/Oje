using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.DB;
using Oje.Section.WebMain.Models.View;
using Oje.Section.WebMain.Services.EContext;
using System.Linq;

namespace Oje.Section.WebMain.Services
{
    public class LoginDescrptionService : ILoginDescrptionService
    {
        readonly WebMainDBContext db = null;
        public LoginDescrptionService
            (
                WebMainDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(LoginDescrptionCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new LoginDescrption()
            {
                ReturnUrl = input.url,
                Description = input.desc,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                SiteSettingId = siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(LoginDescrptionCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.desc))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (string.IsNullOrEmpty(input.url))
                throw BException.GenerateNewException(BMessages.Please_Enter_Link);
            if (db.LoginDescrptions.Any(t => t.SiteSettingId == siteSettingId && t.ReturnUrl == input.url && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);

        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.LoginDescrptions.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public LoginDescrptionCreateUpdateVM GetById(int? id, int? siteSettingId)
        {
            return db.LoginDescrptions
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
                .Select(t => new LoginDescrptionCreateUpdateVM
                {
                    id = t.Id,
                    desc = t.Description,
                    isActive = t.IsActive,
                    url = t.ReturnUrl
                })
                .FirstOrDefault();
        }

        public GridResultVM<LoginDescrptionMainGridResultVM> GetList(LoginDescrptionMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.LoginDescrptions.OrderByDescending(t => t.Id).Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.url))
                quiryResult = quiryResult.Where(t => t.ReturnUrl.Contains(searchInput.url));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);


            int row = searchInput.skip;

            return new GridResultVM<LoginDescrptionMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    isActive = t.IsActive,
                    url = t.ReturnUrl
                })
                .ToList()
                .Select(t => new LoginDescrptionMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    url = t.url,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                })
                .ToList()
            };
        }

        public ApiResult Update(LoginDescrptionCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.LoginDescrptions.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.ReturnUrl = input.url;
            foundItem.Description = input.desc;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
