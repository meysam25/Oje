using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.DB;
using Oje.Security.Models.View;
using Oje.Security.Services.EContext;

namespace Oje.Security.Services
{
    public class BlockLoginUserService : IBlockLoginUserService
    {
        readonly SecurityDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public BlockLoginUserService
            (
                SecurityDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(BlockLoginUserCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new BlockLoginUser()
            {
                EndDate = Convert.ToDateTime(input.endDate.ToEnDate().Value.ToString("yyyy/MM/dd") + " " + input.endTime),
                IsActive = input.isActive.ToBooleanReturnFalse(),
                SiteSettingId = siteSettingId.Value,
                StartDate = Convert.ToDateTime(input.startDate.ToEnDate().Value.ToString("yyyy/MM/dd") + " " + input.startTime)
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(BlockLoginUserCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.startDate) || input.startDate.ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Start_Date);
            if (string.IsNullOrEmpty(input.endDate) || input.endDate.ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_EndDate);
            try { Convert.ToDateTime(input.startDate.ToEnDate().Value.ToString("yyyy/MM/dd") + " " + input.startTime); } catch { throw BException.GenerateNewException(BMessages.Invalid_Time); }
            try { Convert.ToDateTime(input.endDate.ToEnDate().Value.ToString("yyyy/MM/dd") + " " + input.endTime); } catch { throw BException.GenerateNewException(BMessages.Invalid_Time); }
            if (input.startDate.ToEnDate().Value > input.endDate.ToEnDate().Value)
                throw BException.GenerateNewException(BMessages.StartDate_Should_Be_Less_Then_EndDate);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.BlockLoginUsers
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
            return db.BlockLoginUsers
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .OrderByDescending(t => t.Id)
                .Take(1)
                .Select(t => new
                {
                    id = t.Id,
                    startDate = t.StartDate,
                    endDate = t.EndDate,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new
                {
                    t.id,
                    t.isActive,
                    startDate = t.startDate.ToFaDate(),
                    endDate = t.endDate.ToFaDate(),
                    startTime = t.startDate.ToString("HH:mm:ss"),
                    endTime = t.endDate.ToString("HH:mm:ss")
                })
                .FirstOrDefault();
        }

        public GridResultVM<BlockLoginUserMainGridResultVM> GetList(BlockLoginUserMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new BlockLoginUserMainGrid();

            var quiryResult = db.BlockLoginUsers
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.startDate) && searchInput.startDate.ToEnDate() != null)
            {
                var targetDate = searchInput.startDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.StartDate.Year == targetDate.Year && t.StartDate.Month == targetDate.Month && t.StartDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.endDate) && searchInput.endDate.ToEnDate() != null)
            {
                var targetDate = searchInput.endDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.EndDate.Year == targetDate.Year && t.EndDate.Month == targetDate.Month && t.EndDate.Day == targetDate.Day);
            }
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<BlockLoginUserMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    startDate = t.StartDate,
                    endDate = t.EndDate,
                    isActive = t.IsActive,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new BlockLoginUserMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    startDate = t.startDate.ToFaDate() + " " + t.startDate.ToString("HH:mm"),
                    endDate = t.endDate.ToFaDate() + " " + t.endDate.ToString("HH:mm"),
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(BlockLoginUserCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.BlockLoginUsers
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.EndDate = Convert.ToDateTime(input.endDate.ToEnDate().Value.ToString("yyyy/MM/dd") + " " + input.endTime);
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.StartDate = Convert.ToDateTime(input.startDate.ToEnDate().Value.ToString("yyyy/MM/dd") + " " + input.startTime);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public bool IsValidDay(DateTime targetDate, int? siteSettingId)
        {
            return !db.BlockLoginUsers.Any(t => t.SiteSettingId == siteSettingId && t.IsActive == true && t.StartDate <= targetDate && t.EndDate >= targetDate);
        }
    }
}
