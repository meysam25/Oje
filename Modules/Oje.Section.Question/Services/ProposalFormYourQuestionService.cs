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
    public class ProposalFormYourQuestionService : IProposalFormYourQuestionService
    {
        readonly QuestionDBContext db = null;
        readonly IProposalFormService ProposalFormService;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public ProposalFormYourQuestionService
            (
                QuestionDBContext db,
                IProposalFormService ProposalFormService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.ProposalFormService = ProposalFormService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(ProposalFormYourQuestionCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            db.Entry(new ProposalFormYourQuestion()
            {
                Answer = input.answer,
                ProposalFormId = input.fid.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Title = input.title,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                IsInquiry = input.isInquiry
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.ProposalFormYourQuestions
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
            return db.ProposalFormYourQuestions
               .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
               .Where(t => t.Id == id)
               .Select(t => new
               {
                   id = t.Id,
                   answer = t.Answer,
                   isActive = t.IsActive,
                   title = t.Title,
                   fid = t.ProposalFormId,
                   fid_Title = t.ProposalForm.Title,
                   isInquiry = t.IsInquiry,
                   cSOWSiteSettingId = t.SiteSettingId,
                   cSOWSiteSettingId_Title = t.SiteSetting.Title
               })
               .FirstOrDefault();
        }

        public GridResultVM<ProposalFormYourQuestionMainGridResultVM> GetList(ProposalFormYourQuestionMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new ProposalFormYourQuestionMainGrid();

            var qureResult = db.ProposalFormYourQuestions.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.form))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.form));
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<ProposalFormYourQuestionMainGridResultVM>()
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
                    form = t.ProposalForm.Title,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new ProposalFormYourQuestionMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    form = t.form,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(ProposalFormYourQuestionCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            var foundItem = db.ProposalFormYourQuestions
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Answer = input.answer;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Title = input.title;
            foundItem.ProposalFormId = input.fid.Value;
            foundItem.IsInquiry = input.isInquiry;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(ProposalFormYourQuestionCreateUpdateVM input, int? siteSettingId, bool? canSetSiteSetting)
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
            if (input.fid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);
            if (!ProposalFormService.Exist(canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId, input.fid))
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);
        }

        public object GetListForWeb(int? siteSettingId, int? formid, bool isInquiry)
        {
            var quiryResult = db.ProposalFormYourQuestions
                .Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true && t.ProposalFormId == formid);

            if (isInquiry == true)
                quiryResult = quiryResult.Where(t => t.IsInquiry == true);
            else
                quiryResult = quiryResult.Where(t => t.IsInquiry == null || t.IsInquiry == false);

            return 
                quiryResult
                .Select(t => new
                {
                    q = t.Title,
                    a = t.Answer
                })
                .ToList();
        }
    }
}
