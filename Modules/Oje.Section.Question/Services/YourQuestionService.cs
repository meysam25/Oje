using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Question.Interfaces;
using Oje.Section.Question.Models.DB;
using Oje.Section.Question.Models.View;
using Oje.Section.Question.Services.EContext;
using System.Linq;

namespace Oje.Section.Question.Services
{
    public class YourQuestionService : IYourQuestionService
    {
        readonly QuestionDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        public YourQuestionService
            (
                QuestionDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(YourQuestionCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;

            createUpdateValidation(input, siteSettingId);

            db.Entry(new YourQuestion()
            {
                Answer = input.answer,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Title = input.title,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value :  siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {

            var foundItem = db.YourQuestions
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.YourQuestions
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    answer = t.Answer,
                    isActive = t.IsActive,
                    title = t.Title,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<YourQuestionMainGridResultVM> GetList(YourQuestionMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new YourQuestionMainGrid();

            var qureResult = db.YourQuestions.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<YourQuestionMainGridResultVM>()
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
                .Select(t => new YourQuestionMainGridResultVM
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

        public object GetListForWeb(int? siteSettingId)
        {
            return db.YourQuestions
                .Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true)
                .Select(t => new
                {
                    q = t.Title,
                    a = t.Answer
                })
                .ToList();
        }

        public ApiResult Update(YourQuestionCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;

            var foundItem = db.YourQuestions
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Answer = input.answer;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Title = input.title;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(YourQuestionCreateUpdateVM input, int? siteSettingId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 1000)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_1000_chars);
            if (string.IsNullOrEmpty(input.answer))
                throw BException.GenerateNewException(BMessages.Please_Enter_Answer);
            if (input.title.Length > 4000)
                throw BException.GenerateNewException(BMessages.Answer_Can_Not_Be_More_Then_4000_Chars);
        }
    }
}
