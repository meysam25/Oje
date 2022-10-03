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
    public class InsuranceContractDiscountService : IInsuranceContractDiscountService
    {
        readonly InquiryBaseDataDBContext db = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IInsuranceContractService InsuranceContractService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public InsuranceContractDiscountService(
                InquiryBaseDataDBContext db,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IInsuranceContractService InsuranceContractService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.SiteSettingService = SiteSettingService;
            this.InsuranceContractService = InsuranceContractService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(CreateUpdateInsuranceContractDiscountVM input)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            CreateValidation(input, siteSettingId);

            InsuranceContractDiscount newItem = new InsuranceContractDiscount() 
            {
                InsuranceContractId = input.contractId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Percent = input.percent.Value,
                SiteSettingId = siteSettingId.Value,
                Title = input.title,
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.cIds != null)
                foreach (var cid in input.cIds)
                    db.Entry(new InsuranceContractDiscountCompany() { CompanyId = cid, InsuranceContractDiscountId = newItem.Id }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateInsuranceContractDiscountVM input, int? siteSettingId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.cIds == null || input.cIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.contractId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Contract);
            if (!InsuranceContractService.Exist(input.contractId.ToIntReturnZiro()))
                throw BException.GenerateNewException(BMessages.Please_Select_Contract);
            if (input.percent.ToIntReturnZiro() <= 0 || input.percent >= 100)
                throw BException.GenerateNewException(BMessages.Invalid_Percent);
        }

        public ApiResult Delete(int? id)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            InsuranceContractDiscount foundItem = db.InsuranceContractDiscounts
                .Include(t => t.InsuranceContractDiscountCompanies)
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.InsuranceContractDiscountCompanies != null)
                foreach (var item in foundItem.InsuranceContractDiscountCompanies)
                    db.Entry(item).State = EntityState.Deleted;
            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateInsuranceContractDiscountVM GetById(int? id)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            return db.InsuranceContractDiscounts
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new CreateUpdateInsuranceContractDiscountVM
                {
                    cIds = t.InsuranceContractDiscountCompanies.Select(tt => tt.CompanyId).ToList(),
                    contractId = t.InsuranceContractId,
                    isActive = t.IsActive,
                    id = t.Id,
                    title = t.Title,
                    percent = t.Percent
                })
                .FirstOrDefault();
        }

        public GridResultVM<InsuranceContractDiscountMainGridResultVM> GetList(InsuranceContractDiscountMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new InsuranceContractDiscountMainGrid();
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            var qureResult = db.InsuranceContractDiscounts
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);
            if (searchInput.company.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.InsuranceContractDiscountCompanies.Any(tt => tt.CompanyId == searchInput.company));
            if (!string.IsNullOrEmpty(searchInput.contract))
                qureResult = qureResult.Where(t => t.InsuranceContract.Title.Contains(searchInput.contract) || t.InsuranceContract.ProposalForm.Title.Contains(searchInput.contract));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.percent.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.Percent == searchInput.percent);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<InsuranceContractDiscountMainGridResultVM>() 
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new  
                {
                    company = t.InsuranceContractDiscountCompanies.Select(tt => tt.Company.Title).ToList(),
                    contract = t.InsuranceContract.Title + "(" + t.InsuranceContract.ProposalForm.Title + ")",
                    id = t.Id,
                    isActive = t.IsActive,
                    percent = t.Percent,
                    title = t.Title,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new InsuranceContractDiscountMainGridResultVM 
                { 
                    row = ++row,
                    id = t.id,
                    company = string.Join(",", t.company),
                    contract = t.contract,
                    title = t.title,
                    percent = t.percent,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateInsuranceContractDiscountVM input)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            CreateValidation(input, siteSettingId);

            InsuranceContractDiscount foundItem = db.InsuranceContractDiscounts
                .Include(t => t.InsuranceContractDiscountCompanies)
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.InsuranceContractDiscountCompanies != null)
                foreach (var item in foundItem.InsuranceContractDiscountCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            foundItem.InsuranceContractId = input.contractId.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Percent = input.percent.Value;
            foundItem.SiteSettingId = siteSettingId.Value;
            foundItem.Title = input.title;

            if (input.cIds != null)
                foreach (var cid in input.cIds)
                    db.Entry(new InsuranceContractDiscountCompany() { CompanyId = cid, InsuranceContractDiscountId = foundItem.Id }).State = EntityState.Added;
            db.SaveChanges();
           
            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
