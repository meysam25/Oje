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
    public class TopMenuService : ITopMenuService
    {
        readonly WebMainDBContext db = null;
        public TopMenuService(WebMainDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(TopMenuCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new TopMenu()
            {
                Title = input.title,
                ParentId = input.pKey,
                Order = input.order.ToIntReturnZiro(),
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Link = input.link,
                SiteSettingId = siteSettingId.ToIntReturnZiro()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(TopMenuCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (!string.IsNullOrEmpty(input.link) && input.link.Length > 1000)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input.pKey.ToLongReturnZiro() > 0 && !db.TopMenus.Any(t => t.Id == input.pKey && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.TopMenus.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;

            try { db.SaveChanges(); } catch { throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted); }
            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(long? id, int? siteSettingId)
        {
            return db.TopMenus
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    link = t.Link,
                    isActive = t.IsActive,
                    order = t.Order
                })
                .FirstOrDefault();
        }

        public GridResultVM<TopMenuMainGridResultVM> GetList(TopMenuMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new TopMenuMainGrid();

            var qureResult = db.TopMenus.Where(t => t.SiteSettingId == siteSettingId && t.ParentId == searchInput.pKey);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<TopMenuMainGridResultVM>()
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
                .Select(t => new TopMenuMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(TopMenuCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.TopMenus.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Title = input.title;
            foundItem.Order = input.order.ToIntReturnZiro();
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Link = input.link;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetListForWeb(int? siteSettingId)
        {
            return db.TopMenus
                .Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true && t.ParentId == null)
                .OrderBy(t => t.Order)
                .Select(t => new 
                {
                    title = t.Title,
                    link = t.Link,
                    childs = t.Childs
                        .Where(tt => tt.IsActive == true)
                        .OrderBy(tt => tt.Order)
                        .Select(tt => new 
                        {
                            title = tt.Title,
                            link = tt.Link,
                            childs = tt.Childs
                                .Where(ttt => ttt.IsActive == true)
                                .OrderBy(ttt => ttt.Order)
                                .Select(ttt => new 
                                {
                                    title = ttt.Title,
                                    link = ttt.Link
                                })
                                .ToList()
                        })
                        .ToList()

                })
                .ToList();
        }
    }
}
