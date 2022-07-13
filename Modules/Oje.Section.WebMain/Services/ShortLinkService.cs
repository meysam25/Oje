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
    public class ShortLinkService : IShortLinkService
    {
        readonly WebMainDBContext db = null;
        public ShortLinkService(WebMainDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(ShortLinkCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new ShortLink()
            {
                Code = input.code,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                SiteSettingId = siteSettingId.Value,
                TargetLink = input.link
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(ShortLinkCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.code))
                throw BException.GenerateNewException(BMessages.Please_Enter_Code);
            if (input.code.Length > 50)
                throw BException.GenerateNewException(BMessages.Code_Can_Not_Be_More_Then_50_chars);
            if (string.IsNullOrEmpty(input.link))
                throw BException.GenerateNewException(BMessages.Please_Enter_Link);
            if (input.link.Length > 200)
                throw BException.GenerateNewException(BMessages.Link_Can_Not_Be_More_Then_200_Chars);
            if (db.ShortLinks.Any(t => t.Id != input.id && t.SiteSettingId == siteSettingId && t.Code == input.code))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.ShortLinks.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(long? id, int? siteSettingId)
        {
            return db.ShortLinks
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    code = t.Code,
                    isActive = t.IsActive,
                    link = t.TargetLink
                })
                .FirstOrDefault();
        }

        public GridResultVM<ShortLinkMainGridResultVM> GetList(ShortLinkMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.ShortLinks.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.code))
                quiryResult = quiryResult.Where(t => t.Code.Contains(searchInput.code));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.link))
                quiryResult = quiryResult.Where(t => t.TargetLink.Contains(searchInput.link));

            int row = searchInput.skip;

            return new()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    code = t.Code,
                    link = t.TargetLink,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new ShortLinkMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    code = t.code,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    link = t.link
                })
                .ToList()
            };
        }

        public ApiResult Update(ShortLinkCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.ShortLinks.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Code = input.code;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.TargetLink = input.link;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ShortLink GetBy(int? siteSettingId, string code)
        {
            return db.ShortLinks.Where(t => t.IsActive == true && t.SiteSettingId == siteSettingId && t.Code == code).FirstOrDefault();
        }
    }
}
