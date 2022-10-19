using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.SalesNetworkBaseData.Interfaces;
using Oje.Section.SalesNetworkBaseData.Models.DB;
using Oje.Section.SalesNetworkBaseData.Models.View;
using Oje.Section.SalesNetworkBaseData.Services.EContext;
using System.Linq;

namespace Oje.Section.SalesNetworkBaseData.Services
{
    public class SalesNetworkCommissionLevelService : ISalesNetworkCommissionLevelService
    {
        readonly SalesNetworkBaseDataDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public SalesNetworkCommissionLevelService
            (
                SalesNetworkBaseDataDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(SalesNetworkCommissionLevelCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            db.Entry(new SalesNetworkCommissionLevel()
            {
                Rate = input.rate.Value,
                SalesNetworkId = input.snId.Value,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Step = input.step.ToIntReturnZiro(),
                CalceType = input.calceType.Value,
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(SalesNetworkCommissionLevelCreateUpdateVM input, int? siteSettingId, bool? canSetSiteSetting)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.snId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_SaleNetwork);
            if (!db.SalesNetworks.Any(t => t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value) && t.Id == input.snId))
                throw BException.GenerateNewException(BMessages.Please_Select_SaleNetwork);
            if (input.calceType == null)
                throw BException.GenerateNewException(BMessages.Please_Select_CalcType);
            if (input.step.ToIntReturnZiro() < 0)
                throw BException.GenerateNewException(BMessages.Invalid_Step);
            if (input.rate == null || input.rate <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Rate);
            if (db.SalesNetworkCommissionLevels.Any(t => t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value) && t.Id != input.id && t.Step == input.step && t.CalceType == input.calceType))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;

            var foundItem = db.SalesNetworkCommissionLevels
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(canSetSiteSetting, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;

            return db.SalesNetworkCommissionLevels
                        .Where(t => t.Id == id)
                        .getSiteSettingQuiry(canSetSiteSetting, siteSettingId)
                        .Select(t => new 
                        {
                            id = t.Id,
                            snId = t.SalesNetworkId,
                            step = t.Step,
                            rate = t.Rate,
                            cSOWSiteSettingId = t.SiteSettingId,
                            cSOWSiteSettingId_Title = t.SiteSetting.Title,
                            calceType =  t.CalceType
                        })
                        .FirstOrDefault();

        }

        public GridResultVM<SalesNetworkCommissionLevelMainGridResultVM> GetList(SalesNetworkCommissionLevelMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;

            var quiryResult = db.SalesNetworkCommissionLevels.getSiteSettingQuiry(canSetSiteSetting, siteSettingId);
            if (!string.IsNullOrEmpty( searchInput.snId))
                quiryResult = quiryResult.Where(t => t.SalesNetwork.Title.Contains(searchInput.snId));
            if (searchInput.step.ToIntReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Step == searchInput.step);
            if (searchInput.rate != null && searchInput.rate > 0)
                quiryResult = quiryResult.Where(t => t.Rate == searchInput.rate);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<SalesNetworkCommissionLevelMainGridResultVM>() 
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.SalesNetworkId)
                .ThenBy(t => t.Step)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new 
                { 
                    id = t.Id,
                    snId = t.SalesNetwork.Title,
                    step = t.Step,
                    rate = t.Rate,
                    siteTitleMN2 = t.SiteSetting.Title

                })
                .ToList()
                .Select(t => new SalesNetworkCommissionLevelMainGridResultVM 
                { 
                    row = ++row,
                    id = t.id,
                    rate = t.rate.ToString(),
                    siteTitleMN2 = t.siteTitleMN2,
                    snId = t.snId,
                    step = t.step.ToString()
                })
                .ToList()
            };
        }

        public ApiResult Update(SalesNetworkCommissionLevelCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            var foundItem = db.SalesNetworkCommissionLevels
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(canSetSiteSetting, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Rate = input.rate.Value;
            foundItem.SalesNetworkId = input.snId.Value;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;
            foundItem.Step = input.step.ToIntReturnZiro();
            foundItem.CalceType = input.calceType.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
