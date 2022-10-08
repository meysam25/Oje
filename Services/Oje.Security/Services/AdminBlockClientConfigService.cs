using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Triangulate.QuadEdge;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.DB;
using Oje.Security.Models.View;
using Oje.Security.Services.EContext;

namespace Oje.Security.Services
{
    public class AdminBlockClientConfigService : IAdminBlockClientConfigService
    {
        readonly SecurityDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        static List<AdminBlockClientConfig> AdminBlockClientConfigs = null;

        public AdminBlockClientConfigService
            (
                SecurityDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(AdminBlockClientConfigCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId);

            db.Entry(new AdminBlockClientConfig()
            {
                ActionId = input.aid.ToLongReturnZiro(),
                MaxValue = input.value.ToIntReturnZiro(),
                IsActive = input.isActive.ToBooleanReturnFalse(),
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            AdminBlockClientConfigs = null;

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(AdminBlockClientConfigCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.aid.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.value.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (db.AdminBlockClientConfigs.Any(t => t.Id != input.id && t.SiteSettingId == siteSettingId && t.ActionId == input.aid))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.AdminBlockClientConfigs
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            AdminBlockClientConfigs = null;

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.AdminBlockClientConfigs
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    aid = t.ActionId,
                    aid_Title = t.Action.Controller.Section.Title + "/" + t.Action.Controller.Title + "/" + t.Action.Title,
                    value = t.MaxValue,
                    isActive = t.IsActive,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<AdminBlockClientConfigMainGridResultVM> GetList(AdminBlockClientConfigMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.AdminBlockClientConfigs
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.action))
                quiryResult = quiryResult.Where(t => (t.Action.Controller.Section.Title + "/" + t.Action.Controller.Title + "/" + t.Action.Title).Contains(searchInput.action));
            if (searchInput.value.ToIntReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.MaxValue == searchInput.value);
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<AdminBlockClientConfigMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    action = t.Action.Controller.Section.Title + "/" + t.Action.Controller.Title + "/" + t.Action.Title,
                    isActive = t.IsActive,
                    value = t.MaxValue,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new AdminBlockClientConfigMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    action = t.action,
                    value = t.value.ToString(),
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(AdminBlockClientConfigCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.AdminBlockClientConfigs
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.ActionId = input.aid.ToLongReturnZiro();
            foundItem.MaxValue = input.value.ToIntReturnZiro();
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            db.SaveChanges();

            AdminBlockClientConfigs = null;

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public AdminBlockClientConfig GetByFromCache(long actionId, int siteSettingId)
        {
            if (AdminBlockClientConfigs == null)
                AdminBlockClientConfigs = db.AdminBlockClientConfigs.AsNoTracking().ToList();


            return AdminBlockClientConfigs.Where(t => t.SiteSettingId == siteSettingId && t.ActionId == actionId && t.IsActive == true).FirstOrDefault();
        }
    }
}
