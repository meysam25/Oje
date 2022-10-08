using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Models.View;
using Oje.Section.InquiryBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.InquiryBaseData.Services.EContext;
using Microsoft.AspNetCore.Http;

namespace Oje.Section.InquiryBaseData.Services
{
    public class InquiryDurationService : IInquiryDurationService
    {
        readonly InquiryBaseDataDBContext db = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public InquiryDurationService(
                InquiryBaseDataDBContext db,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IProposalFormService ProposalFormService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.SiteSettingService = SiteSettingService;
            this.ProposalFormService = ProposalFormService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(CreateUpdateInquiryDurationVM input)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createValidation(input, siteSettingId, canSetSiteSetting);

            var newItem = new InquiryDuration()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Percent = input.percent.Value,
                ProposalFormId = input.formId.Value,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Title = input.title,
                Day = input.day.ToIntReturnZiro()
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            foreach (var cid in input.cIds)
                db.Entry(new InquiryDurationCompany() { CompanyId = cid, InquiryDurationId = newItem.Id }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateInquiryDurationVM input, int? siteSettingId, bool? canSetSiteSetting)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.percent == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Percent);
            if (input.percent.ToIntReturnZiro() <= 0 || input.percent >= 100)
                throw BException.GenerateNewException(BMessages.Invalid_Percent);
            if (input.day == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Day);
            if (input.day.ToIntReturnZiro() <= 0 || input.day.ToIntReturnZiro() > 3650)
                throw BException.GenerateNewException(BMessages.Invalid_Day);
            if (input.formId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (!ProposalFormService.Exist( input.formId.ToIntReturnZiro(), canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (input.cIds == null || input.cIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (db.InquiryDurations.Any(t => t.Id != input.id && t.Title == input.title && t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value) && t.ProposalFormId == input.formId))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            var foundItem = db.InquiryDurations
                .Include(t => t.InquiryDurationCompanies)
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.InquiryDurationCompanies != null)
                foreach (var item in foundItem.InquiryDurationCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateInquiryDurationVM GetById(int? id)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            return db.InquiryDurations
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new CreateUpdateInquiryDurationVM
                { 
                    cIds = t.InquiryDurationCompanies.Select(tt => tt.CompanyId).ToList(),
                    formId = t.ProposalFormId,
                    formId_Title = t.ProposalForm.Title,
                    isActive = t.IsActive,
                    percent = t.Percent,
                    title = t.Title,
                    id = t.Id,
                    day = t.Day,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<InquiryDurationMainGridResultVM> GetList(InquiryDurationMainGrid searchInput)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            if (searchInput == null)
                searchInput = new InquiryDurationMainGrid();

            var qureResult = db.InquiryDurations
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (searchInput.company.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.InquiryDurationCompanies.Any(tt => tt.CompanyId == searchInput.company));
            if (!string.IsNullOrEmpty(searchInput.ppfTitle))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.ppfTitle));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.percent > 0)
                qureResult = qureResult.Where(t => t.Percent == searchInput.percent);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.day.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.Day == searchInput.day);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            var row = searchInput.skip;

            return new GridResultVM<InquiryDurationMainGridResultVM> 
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new 
                {
                    id = t.Id,
                    company = t.InquiryDurationCompanies.Select(tt => tt.Company.Title).ToList(),
                    ppfTitle = t.ProposalForm.Title,
                    title = t.Title,
                    percent = t.Percent,
                    isActive = t.IsActive,
                    day = t.Day,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new InquiryDurationMainGridResultVM 
                {
                    row = ++row,
                    company = string.Join(",", t.company),
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    percent = t.percent,
                    ppfTitle = t.ppfTitle,
                    title = t.title,
                    day = t.day,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateInquiryDurationVM input)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createValidation(input, siteSettingId, canSetSiteSetting);

            var foundItem = db.InquiryDurations
                .Include(t => t.InquiryDurationCompanies)
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.InquiryDurationCompanies != null)
                foreach (var item in foundItem.InquiryDurationCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Percent = input.percent.Value;
            foundItem.ProposalFormId = input.formId.Value;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;
            foundItem.Title = input.title;
            foundItem.Day = input.day.ToIntReturnZiro();

            foreach (var cid in input.cIds)
                db.Entry(new InquiryDurationCompany() { CompanyId = cid, InquiryDurationId = foundItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
