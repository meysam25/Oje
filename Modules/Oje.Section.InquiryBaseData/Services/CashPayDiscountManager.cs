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

namespace Oje.Section.InquiryBaseData.Services
{
    public class CashPayDiscountService : ICashPayDiscountService
    {
        readonly InquiryBaseDataDBContext db = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IProposalFormService ProposalFormService = null;
        public CashPayDiscountService(
                InquiryBaseDataDBContext db,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IProposalFormService ProposalFormService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.db = db;
            this.ProposalFormService = ProposalFormService;
        }

        public ApiResult Create(CreateUpdateCashPayDiscountVM input)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            createValidation(input, siteSettingId);

            var newItem = new CashPayDiscount()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Percent = input.percent.Value,
                ProposalFormId = input.formId.Value,
                SiteSettingId = siteSettingId.Value,
                Title = input.title
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            foreach (var cid in input.cIds)
                db.Entry(new CashPayDiscountCompany() { CompanyId = cid, CashPayDiscountId = newItem.Id }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateCashPayDiscountVM input, int? siteSettingId)
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
            if (input.formId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if(!ProposalFormService.Exist(input.formId.ToIntReturnZiro(), siteSettingId))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (input.cIds == null || input.cIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (db.CashPayDiscounts.Any(t => t.Id != input.id && t.Title == input.title && t.SiteSettingId == siteSettingId && t.ProposalFormId == input.formId))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            var foundItem = db.CashPayDiscounts.Include(t => t.CashPayDiscountCompanies).Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.CashPayDiscountCompanies != null)
                foreach (var item in foundItem.CashPayDiscountCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateCashPayDiscountVM GetById(int? id)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            return db.CashPayDiscounts
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .Select(t => new CreateUpdateCashPayDiscountVM
                {
                    cIds = t.CashPayDiscountCompanies.Select(tt => tt.CompanyId).ToList(),
                    formId = t.ProposalFormId,
                    formId_Title = t.ProposalForm.Title,
                    isActive = t.IsActive,
                    percent = t.Percent,
                    title = t.Title,
                    id = t.Id
                })
                .FirstOrDefault();
        }

        public GridResultVM<CashPayDiscountMainGridResultVM> GetList(CashPayDiscountMainGrid searchInput)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            if (searchInput == null)
                searchInput = new CashPayDiscountMainGrid();

            var qureResult = db.CashPayDiscounts.Where(t => t.SiteSettingId == siteSettingId);

            if (searchInput.company.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.CashPayDiscountCompanies.Any(tt => tt.CompanyId == searchInput.company));
            if (!string.IsNullOrEmpty(searchInput.ppfTitle))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.ppfTitle));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.percent > 0)
                qureResult = qureResult.Where(t => t.Percent == searchInput.percent);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            var row = searchInput.skip;

            return new GridResultVM<CashPayDiscountMainGridResultVM>
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    company = t.CashPayDiscountCompanies.Select(tt => tt.Company.Title).ToList(),
                    ppfTitle = t.ProposalForm.Title,
                    title = t.Title,
                    percent = t.Percent,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new CashPayDiscountMainGridResultVM
                {
                    row = ++row,
                    company = string.Join(",", t.company),
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    percent = t.percent,
                    ppfTitle = t.ppfTitle,
                    title = t.title
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateCashPayDiscountVM input)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            createValidation(input, siteSettingId);

            var foundItem = db.CashPayDiscounts.Include(t => t.CashPayDiscountCompanies).Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.CashPayDiscountCompanies != null)
                foreach (var item in foundItem.CashPayDiscountCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Percent = input.percent.Value;
            foundItem.ProposalFormId = input.formId.Value;
            foundItem.SiteSettingId = siteSettingId.Value;
            foundItem.Title = input.title;

            foreach (var cid in input.cIds)
                db.Entry(new CashPayDiscountCompany() { CompanyId = cid, CashPayDiscountId = foundItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
