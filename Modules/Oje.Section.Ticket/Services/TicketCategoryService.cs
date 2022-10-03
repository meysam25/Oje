using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Ticket.Interfaces;
using Oje.Section.Ticket.Models.DB;
using Oje.Section.Ticket.Models.View;
using Oje.Section.Ticket.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Ticket.Services
{
    public class TicketCategoryService : ITicketCategoryService
    {
        readonly TicketDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public TicketCategoryService(TicketDBContext db, IHttpContextAccessor HttpContextAccessor)
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(TicketCategoryCreateUpdateVM input, int? siteSettingId)
        {
            createValidation(input, siteSettingId);

            db.Entry(new TicketCategory()
            {
                Title = input.title,
                ParentId = input.pKey,
                Order = input.order.ToIntReturnZiro(),
                IsActive = input.isActive.ToBooleanReturnFalse(),
                SiteSettingId = siteSettingId.ToIntReturnZiro()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(TicketCategoryCreateUpdateVM input, int? siteSettingId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.pKey.ToLongReturnZiro() > 0 && !db.TicketCategories.Any(t => t.Id == input.pKey && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.TicketCategories
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;

            try { db.SaveChanges(); } catch { throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted); }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.TicketCategories
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                    order = t.Order
                })
                .FirstOrDefault();
        }

        public GridResultVM<TicketCategoryMainGridResultVM> GetList(TicketCategoryMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new TicketCategoryMainGrid();

            var qureResult = db.TicketCategories
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.ParentId == searchInput.pKey);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<TicketCategoryMainGridResultVM>()
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
                    isActive = t.IsActive,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new TicketCategoryMainGridResultVM
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

        public ApiResult Update(TicketCategoryCreateUpdateVM input, int? siteSettingId)
        {
            createValidation(input, siteSettingId);

            var foundItem = db.TicketCategories
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Title = input.title;
            foundItem.Order = input.order.ToIntReturnZiro();
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange
                    (
                        db.TicketCategories
                            .Where(t => t.IsActive == true && t.SiteSettingId == siteSettingId && t.ParentId == null)
                            .OrderBy(t => t.Order)
                            .Select(t => new { id = t.Id, title = t.Title })
                            .ToList()
                    );

            return result;
        }

        public object GetightListForSelect2(int ticketCategoryId, Select2SearchVM searchInput, int? siteSettingId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.TicketCategories.OrderBy(t => t.Order).Where(t => t.IsActive == true && t.Parent.SiteSettingId == siteSettingId && t.SiteSettingId == siteSettingId && t.ParentId == ticketCategoryId);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public bool Exist(int? id, int? siteSettingId)
        {
            return db.TicketCategories.Any(t => t.Id == id && t.SiteSettingId == siteSettingId && t.IsActive == true && t.ParentId > 0);
        }
    }
}
