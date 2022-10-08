using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.DB;
using Oje.Section.WebMain.Models.View;
using Oje.Section.WebMain.Services.EContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Services
{
    public class AutoAnswerOnlineChatMessageService : IAutoAnswerOnlineChatMessageService
    {
        readonly WebMainDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        readonly IAutoAnswerOnlineChatMessageLikeService AutoAnswerOnlineChatMessageLikeService = null;

        public AutoAnswerOnlineChatMessageService
            (
                WebMainDBContext db,
                IAutoAnswerOnlineChatMessageLikeService AutoAnswerOnlineChatMessageLikeService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.AutoAnswerOnlineChatMessageLikeService = AutoAnswerOnlineChatMessageLikeService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(AutoAnswerOnlineChatMessageCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            db.Entry(new AutoAnswerOnlineChatMessage()
            {
                Description = input.description,
                Link = input.link,
                Order = input.order.ToIntReturnZiro(),
                ParentId = input.pKey,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Title = input.title,
                IsMessage = input.isMessage.ToBooleanReturnFalse(),
                IsActive = input.isActive.ToBooleanReturnFalse(),
                HasLike = input.hasLike.ToBooleanReturnFalse()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(AutoAnswerOnlineChatMessageCreateUpdateVM input, int? siteSettingId, bool? canSetSiteSetting)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.description))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (!string.IsNullOrEmpty(input.link) && !input.link.IsWebsite())
                throw BException.GenerateNewException(BMessages.Invalid_Url);
            if (!string.IsNullOrEmpty(input.link) && string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.pKey.ToIntReturnZiro() > 0 && !db.AutoAnswerOnlineChatMessages.Any(t => t.Id == input.pKey && t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value)))
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.AutoAnswerOnlineChatMessages
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            try { db.Entry(foundItem).State = EntityState.Deleted; db.SaveChanges(); } catch { throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted); }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.AutoAnswerOnlineChatMessages
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    description = t.Description,
                    link = t.Link,
                    order = t.Order,
                    isActive = t.IsActive,
                    isMessage = t.IsMessage,
                    hasLike = t.HasLike,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                }).FirstOrDefault();
        }

        public GridResultVM<AutoAnswerOnlineChatMessageMainGridResultVM> GetList(AutoAnswerOnlineChatMessageMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new AutoAnswerOnlineChatMessageMainGrid();

            var quiryResult = db.AutoAnswerOnlineChatMessages
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (searchInput.pKey.ToIntReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.ParentId == searchInput.pKey);
            else
                quiryResult = quiryResult.Where(t => t.ParentId == null);

            if (!string.IsNullOrEmpty(searchInput.description))
                quiryResult = quiryResult.Where(t => t.Description.Contains(searchInput.description));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.isMessage != null)
                quiryResult = quiryResult.Where(t => t.IsMessage == searchInput.isMessage);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<AutoAnswerOnlineChatMessageMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderBy(t => t.Order)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    t.Description,
                    t.IsActive,
                    t.IsMessage,
                    likeCount = t.AutoAnswerOnlineChatMessageLikes.Count(tt => tt.IsLike == true),
                    dislikeCount = t.AutoAnswerOnlineChatMessageLikes.Count(tt => tt.IsLike == false),
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList().Select(t => new AutoAnswerOnlineChatMessageMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    description = t.Description,
                    isActive = t.IsActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    isMessage = t.IsMessage == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                    likeCount = t.likeCount,
                    dislikeCount = t.dislikeCount,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(AutoAnswerOnlineChatMessageCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            var foundItem = db.AutoAnswerOnlineChatMessages
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Description = input.description;
            foundItem.Link = input.link;
            foundItem.Order = input.order.ToIntReturnZiro();
            foundItem.Title = input.title;
            foundItem.IsMessage = input.isMessage.ToBooleanReturnFalse();
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.HasLike = input.hasLike.ToBooleanReturnFalse();
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetByParentId(int? parentId, int? siteSettingId)
        {
            return db.AutoAnswerOnlineChatMessages
                .OrderBy(t => t.Order)
                .Where(t => t.SiteSettingId == siteSettingId && t.ParentId == parentId && t.IsActive == true)
                .Select(t => new
                {
                    id = t.Id,
                    message = t.Description,
                    isAdmin = true,
                    hasLike = t.HasLike == true ? true : false,
                    title = t.Title,
                    cTime = DateTime.Now.ToString("HH:mm"),
                    link = t.Link,
                    type = t.IsMessage == true ? "text" : !string.IsNullOrEmpty(t.Link) ? "link" : "button"
                })
                .ToList();
        }

        public async Task LikeOrDislike(int? id, bool isLike, int? siteSettingId, IpSections clientIp)
        {
            var foundItemId = await db.AutoAnswerOnlineChatMessages.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).Select(t => t.Id).FirstOrDefaultAsync();
            if (foundItemId > 0)
                await AutoAnswerOnlineChatMessageLikeService.Create(foundItemId, isLike, siteSettingId, clientIp);
        }
    }
}
