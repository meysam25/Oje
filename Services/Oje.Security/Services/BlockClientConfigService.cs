using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.DB;
using Oje.Security.Models.View;
using Oje.Security.Services.EContext;

namespace Oje.Security.Services
{
    public class BlockClientConfigService : IBlockClientConfigService
    {
        readonly SecurityDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        static List<BlockClientConfig> BlockClientConfigs = null;

        public BlockClientConfigService
            (
                SecurityDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(BlockClientConfigCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new BlockClientConfig()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                MaxFirewall = input.maxFirewall.ToIntReturnZiro(),
                MaxSoftware = input.maxSoftware.ToIntReturnZiro(),
                MaxSuccessFirewall = input.maxSuccessFirewall.ToIntReturnZiro(),
                MaxSuccessSoftware = input.maxSuccessSoftware.ToIntReturnZiro(),
                SiteSettingId = siteSettingId.Value,
                Type = input.type.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            BlockClientConfigs = null;

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(BlockClientConfigCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.maxSoftware.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_MaxSoftware_Count);
            if (input.maxSuccessSoftware.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_MaxSoftware_Success_Count);
            if (input.maxFirewall.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_MaxFirewall_Count);
            if (input.maxSuccessFirewall.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_MaxFirewall_Success_Count);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (db.BlockClientConfigs.Any(t => t.SiteSettingId == siteSettingId && t.Id != input.id && t.Type == input.type))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);

        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.BlockClientConfigs
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            BlockClientConfigs = null;

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.BlockClientConfigs
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    type = (int)t.Type,
                    maxSoftware = t.MaxSoftware,
                    maxSuccessSoftware = t.MaxSuccessSoftware,
                    maxFirewall = t.MaxFirewall,
                    maxSuccessFirewall = t.MaxSuccessFirewall,
                    isActive = t.IsActive
                })
                .FirstOrDefault();
        }

        public GridResultVM<BlockClientConfigMainGridResultVM> GetList(BlockClientConfigMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new BlockClientConfigMainGrid();

            var qureResult = db.BlockClientConfigs
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<BlockClientConfigMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    type = t.Type,
                    isActive = t.IsActive,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new BlockClientConfigMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    type = t.type.GetEnumDisplayName(),
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(BlockClientConfigCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.BlockClientConfigs
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.MaxFirewall = input.maxFirewall.ToIntReturnZiro();
            foundItem.MaxSoftware = input.maxSoftware.ToIntReturnZiro();
            foundItem.MaxSuccessFirewall = input.maxSuccessFirewall.ToIntReturnZiro();
            foundItem.MaxSuccessSoftware = input.maxSuccessSoftware.ToIntReturnZiro();
            foundItem.Type = input.type.Value;
            
            db.SaveChanges();

            BlockClientConfigs = null;

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public BlockClientConfig GetBy(BlockClientConfigType type, int? siteSettingId)
        {
            if (BlockClientConfigs == null)
                BlockClientConfigs = db.BlockClientConfigs.AsNoTracking().ToList();

            return BlockClientConfigs.Where(t => t.Type == type && t.SiteSettingId == siteSettingId && t.IsActive == true).FirstOrDefault();
        }
    }
}
