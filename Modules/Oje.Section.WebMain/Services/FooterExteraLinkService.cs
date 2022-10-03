using Microsoft.AspNetCore.Http;
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
    public class FooterExteraLinkService : IFooterExteraLinkService
    {
        readonly WebMainDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        public FooterExteraLinkService
            (
                WebMainDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(FooterExteraLinkCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new FooterExteraLink()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Link = input.link,
                SiteSettingId = siteSettingId.Value,
                Title = input.title
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(FooterExteraLinkCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (string.IsNullOrEmpty(input.link))
                throw BException.GenerateNewException(BMessages.Please_Enter_Link);
            if (input.link.Length > 200)
                throw BException.GenerateNewException(BMessages.Link_Can_Not_Be_More_Then_200_Chars);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.FooterExteraLinks
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.FooterExteraLinks
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    link = t.Link,
                    isActive = t.IsActive
                })
                .FirstOrDefault();
        }

        public GridResultVM<FooterExteraLinkMainGridResultVM> GetList(FooterExteraLinkMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new FooterExteraLinkMainGrid();

            var qureResult = db.FooterExteraLinks
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<FooterExteraLinkMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                    .OrderByDescending(t => t.Id)
                    .Skip(searchInput.skip)
                    .Take(searchInput.take)
                    .Select(t => new
                    {
                        id = t.Id,
                        title = t.Title,
                        isActive = t.IsActive,
                        siteTitleMN2 = t.SiteSetting.Title
                    })
                    .ToList()
                    .Select(t => new FooterExteraLinkMainGridResultVM
                    {
                        row = ++row,
                        id = t.id,
                        title = t.title,
                        isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                        siteTitleMN2 = t.siteTitleMN2
                    })
                    .ToList()
            };
        }

        public ApiResult Update(FooterExteraLinkCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.FooterExteraLinks
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Link = input.link;
            foundItem.Title = input.title;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList(int? siteSettingId)
        {
            return db.FooterExteraLinks
                .Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true)
                .Select(t => new
                {
                    t = t.Title,
                    l = t.Link
                })
                .ToList();
        }
    }
}
