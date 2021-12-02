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
    public class InquiryMaxDiscountManager : IInquiryMaxDiscountManager
    {
        readonly InquiryBaseDataDBContext db = null;
        readonly AccountManager.Interfaces.ISiteSettingManager SiteSettingManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        public InquiryMaxDiscountManager(
                InquiryBaseDataDBContext db,
                AccountManager.Interfaces.ISiteSettingManager SiteSettingManager,
                IProposalFormManager ProposalFormManager
            )
        {
            this.SiteSettingManager = SiteSettingManager;
            this.db = db;
            this.ProposalFormManager = ProposalFormManager;
        }

        public ApiResult Create(CreateUpdateInquiryMaxDiscountVM input)
        {
            var siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            createValidation(input, siteSettingId);

            var newItem = new InquiryMaxDiscount()
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
                db.Entry(new InquiryMaxDiscountCompany() { CompanyId = cid, InquiryMaxDiscountId = newItem.Id }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateInquiryMaxDiscountVM input, int? siteSettingId)
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
            if (!ProposalFormManager.Exist(input.formId.ToIntReturnZiro(), siteSettingId))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (input.cIds == null || input.cIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (db.InquiryMaxDiscounts.Any(t => t.Id != input.id && t.Title == input.title && t.SiteSettingId == siteSettingId && t.ProposalFormId == input.formId))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;

            var foundItem = db.InquiryMaxDiscounts.Include(t => t.InquiryMaxDiscountCompanies).Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.InquiryMaxDiscountCompanies != null)
                foreach (var item in foundItem.InquiryMaxDiscountCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateInquiryMaxDiscountVM GetById(int? id)
        {
            var siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            return db.InquiryMaxDiscounts
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .Select(t => new CreateUpdateInquiryMaxDiscountVM
                {
                    cIds = t.InquiryMaxDiscountCompanies.Select(tt => tt.CompanyId).ToList(),
                    formId = t.ProposalFormId,
                    formId_Title = t.ProposalForm.Title,
                    isActive = t.IsActive,
                    percent = t.Percent,
                    title = t.Title,
                    id = t.Id
                })
                .FirstOrDefault();
        }

        public GridResultVM<InquiryMaxDiscountMainGridResultVM> GetList(InquiryMaxDiscountMainGrid searchInput)
        {
            var siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            if (searchInput == null)
                searchInput = new InquiryMaxDiscountMainGrid();

            var qureResult = db.InquiryMaxDiscounts.Where(t => t.SiteSettingId == siteSettingId);

            if (searchInput.company.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.InquiryMaxDiscountCompanies.Any(tt => tt.CompanyId == searchInput.company));
            if (!string.IsNullOrEmpty(searchInput.ppfTitle))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.ppfTitle));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.percent > 0)
                qureResult = qureResult.Where(t => t.Percent == searchInput.percent);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            var row = searchInput.skip;

            return new GridResultVM<InquiryMaxDiscountMainGridResultVM>
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    company = t.InquiryMaxDiscountCompanies.Select(tt => tt.Company.Title).ToList(),
                    ppfTitle = t.ProposalForm.Title,
                    title = t.Title,
                    percent = t.Percent,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new InquiryMaxDiscountMainGridResultVM
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

        public ApiResult Update(CreateUpdateInquiryMaxDiscountVM input)
        {
            var siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            createValidation(input, siteSettingId);

            var foundItem = db.InquiryMaxDiscounts.Include(t => t.InquiryMaxDiscountCompanies).Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.InquiryMaxDiscountCompanies != null)
                foreach (var item in foundItem.InquiryMaxDiscountCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Percent = input.percent.Value;
            foundItem.ProposalFormId = input.formId.Value;
            foundItem.SiteSettingId = siteSettingId.Value;
            foundItem.Title = input.title;

            foreach (var cid in input.cIds)
                db.Entry(new InquiryMaxDiscountCompany() { CompanyId = cid, InquiryMaxDiscountId = foundItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
