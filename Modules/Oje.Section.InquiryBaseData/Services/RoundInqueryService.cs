using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Models.View;
using Oje.Section.InquiryBaseData.Models.DB;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.InquiryBaseData.Services.EContext;
using Microsoft.AspNetCore.Http;

namespace Oje.Section.InquiryBaseData.Services
{
    public class RoundInqueryService : IRoundInqueryService
    {
        readonly InquiryBaseDataDBContext db = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public RoundInqueryService(
            InquiryBaseDataDBContext db, 
            AccountService.Interfaces.ISiteSettingService SiteSettingService,
            IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.SiteSettingService = SiteSettingService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(CreateUpdateRoundInqueryVM input)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            db.Entry(new RoundInquery()
            {
                Format = input.format,
                ProposalFormId = input.formId.Value,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Type = input.type.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(CreateUpdateRoundInqueryVM input, int? siteSettingId, bool? canSetSiteSetting)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.formId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (string.IsNullOrEmpty(input.format))
                throw BException.GenerateNewException(BMessages.Please_Enter_Format);
            if (input.format != new string('0', input.format.Length))
                throw BException.GenerateNewException(BMessages.Please_Use_0_For_Format);
            if (input.format.Length > 20)
                throw BException.GenerateNewException(BMessages.Format_Can_Not_Be_More_Then_20_Chars);
            if (db.RoundInqueries.Any(t => t.ProposalFormId == input.formId && t.Id != input.id && t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId)))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
            if (!db.ProposalForms.Any(t => t.Id == input.formId && (t.SiteSettingId == null || t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId))))
                throw BException.GenerateNewException(BMessages.Not_Found);
        }

        public ApiResult Delete(int? id)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var deleteItem = db.RoundInqueries
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();
            if (deleteItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, Infrastructure.Enums.ApiResultErrorCode.NotFound);

            db.Entry(deleteItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            return
                db.RoundInqueries
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    format = t.Format,
                    formId = t.ProposalFormId,
                    formId_Title = t.ProposalForm.Title,
                    type = t.Type,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                })
                .OrderByDescending(t => t.id)
                .Take(1)
                .ToList()
                .Select(t => new
                {
                    t.id,
                    t.format,
                    t.formId,
                    t.formId_Title,
                    type = (int)t.type,
                    t.cSOWSiteSettingId,
                    t.cSOWSiteSettingId_Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<RoundInqueryMainGridResult> GetList(RoundInqueryMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new RoundInqueryMainGrid();

            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var qureResult = db.RoundInqueries.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.proposalFormTitle))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.proposalFormTitle));
            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.format))
                qureResult = qureResult.Where(t => t.Format == searchInput.format);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<RoundInqueryMainGridResult> 
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new 
                {
                    id = t.Id,
                    format = t.Format,
                    proposalFormTitle = t.ProposalForm.Title,
                    type = t.Type,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new RoundInqueryMainGridResult 
                {
                    row = ++row,
                    id = t.id,
                    format = t.format,
                    proposalFormTitle = t.proposalFormTitle,
                    type = t.type.GetAttribute<DisplayAttribute>()?.Name,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateRoundInqueryVM input)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            var foundItem = db.RoundInqueries
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, Infrastructure.Enums.ApiResultErrorCode.NotFound);

            foundItem.Format = input.format;
            foundItem.ProposalFormId = input.formId.Value;
            foundItem.Type = input.type.Value;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
