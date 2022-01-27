using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.DB;
using Oje.Section.WebMain.Models.View;
using Oje.Section.WebMain.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Services
{
    public class FooterGroupExteraLinkService : IFooterGroupExteraLinkService
    {
        readonly WebMainDBContext db = null;
        public FooterGroupExteraLinkService
            (
                WebMainDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(FooterGroupExteraLinkCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new FooterGroupExteraLink()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Link = input.link,
                ParentId = input.pKey,
                SiteSettingId = siteSettingId.Value,
                Title = input.title,
                Order = input.order.ToIntReturnZiro()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(FooterGroupExteraLinkCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.pKey.ToIntReturnZiro() > 0 && input.pKey.ToIntReturnZiro() != input.id && string.IsNullOrEmpty(input.link))
                throw BException.GenerateNewException(BMessages.Please_Enter_Link);
            if (!string.IsNullOrEmpty(input.link) && input.link.Length > 200)
                throw BException.GenerateNewException(BMessages.Link_Can_Not_Be_More_Then_200_Chars);
            if (input.pKey.ToIntReturnZiro() > 0 && !db.FooterGroupExteraLinks.Any(t => t.SiteSettingId == siteSettingId && t.Id == input.pKey))
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.FooterGroupExteraLinks.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.FooterGroupExteraLinks.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).Select(t => new
            {
                id = t.Id,
                title = t.Title,
                isActive = t.IsActive,
                link = t.Link,
                order = t.Order
            }).FirstOrDefault();
        }

        public GridResultVM<FooterGroupExteraLinkMainGridResultVM> GetList(FooterGroupExteraLinkMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new FooterGroupExteraLinkMainGrid();

            var qureResult = db.FooterGroupExteraLinks.Where(t => t.SiteSettingId == siteSettingId && t.ParentId == searchInput.pKey);

            int row = searchInput.skip;

            return new GridResultVM<FooterGroupExteraLinkMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                .OrderBy(t => t.Order)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new FooterGroupExteraLinkMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    title = t.title
                })
                .ToList()
            };
        }

        public ApiResult Update(FooterGroupExteraLinkCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.FooterGroupExteraLinks.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Link = input.link;
            foundItem.Title = input.title;
            foundItem.Order = input.order.ToIntReturnZiro();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList(int? siteSettingId)
        {
            return db.FooterGroupExteraLinks
                .OrderBy(t => t.Order)
                .Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true && t.ParentId == null)
                .Select(t => new
                {
                    t = t.Title,
                    chi = t.Childs.OrderBy(t => t.Order).Where(t => t.IsActive == true).Select(tt => new
                    {
                        t = tt.Title,
                        l = tt.Link
                    }).ToList()
                })
                .ToList();
        }
    }
}
