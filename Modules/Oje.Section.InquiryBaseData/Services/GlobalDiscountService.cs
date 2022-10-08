using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Models.View;
using Oje.Section.InquiryBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.InquiryBaseData.Services.EContext;
using Microsoft.AspNetCore.Http;

namespace Oje.Section.InquiryBaseData.Services
{
    public class GlobalDiscountService : IGlobalDiscountService
    {
        readonly InquiryBaseDataDBContext db = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly Interfaces.IProposalFormService ProposalFormService = null;
        readonly IUserService UserService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public GlobalDiscountService(
                InquiryBaseDataDBContext db,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                Interfaces.IProposalFormService ProposalFormService,
                IUserService UserService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.SiteSettingService = SiteSettingService;
            this.ProposalFormService = ProposalFormService;
            this.UserService = UserService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(CreateUpdateGlobalDiscountVM input)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            CreateValidation(input, siteSettingId, loginUserId, canSetSiteSetting);

            GlobalDiscount newItem = new GlobalDiscount()
            {
                Code = input.discountCode,
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId.Value,
                FromDate = input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value,
                InqueryCode = input.inqueryCode,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                MaxPrice = input.maxPrice.ToLongReturnZiro(),
                MaxUse = input.countUse.ToIntReturnZiro(),
                Percent = input.percent,
                Price = input.price,
                ProposalFormId = input.formId.Value,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Title = input.title,
                ToDate = input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value
            };
            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            foreach (int cId in input.cIds)
                db.Entry(new GlobalDiscountCompany() { CompanyId = cId, GlobalDiscountId = newItem.Id }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateGlobalDiscountVM input, int? siteSettingId, long? loginUserId, bool? canSetSiteSetting)
        {
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.cIds == null || input.cIds.Count <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (input.formId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (!ProposalFormService.Exist(input.formId.ToIntReturnZiro(), canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (string.IsNullOrEmpty(input.fromDate))
                throw BException.GenerateNewException(BMessages.Please_Enter_FromDate);
            if (input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (string.IsNullOrEmpty(input.toDate))
                throw BException.GenerateNewException(BMessages.Please_Enter_ToDate);
            if (input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (input.percent.ToIntReturnZiro() <= 0 && input.price.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Percent_Or_Price);
            if (input.percent.ToIntReturnZiro() > 0 && input.price.ToLongReturnZiro() > 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Percent_Or_Price);
            if (input.percent.ToIntReturnZiro() >= 100)
                throw BException.GenerateNewException(BMessages.Invalid_Percent);
            if (input.maxPrice.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_MaxPrice);
            if (input.countUse.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_CountUse);
            if (input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value > input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value)
                throw BException.GenerateNewException(BMessages.ToDate_Should_Be_Greader_Then_FromYear);
        }

        public ApiResult Delete(int? id)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.Value);

            GlobalDiscount foundItem = db.GlobalDiscounts
                .Include(t => t.GlobalDiscountCompanies)
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<GlobalDiscount, User>(loginUserId, canSeeAllItems)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.GlobalDiscountCompanies != null)
                foreach (var item in foundItem.GlobalDiscountCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateGlobalDiscountVM GetById(int? id)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.Value);

            return db.GlobalDiscounts
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<GlobalDiscount, User>(loginUserId, canSeeAllItems)
                .Select(t => new
                {
                    cIds = t.GlobalDiscountCompanies.Select(tt => tt.CompanyId).ToList(),
                    countUse = t.MaxUse,
                    discountCode = t.Code,
                    formId = t.ProposalFormId,
                    formId_Title = t.ProposalForm.Title,
                    fromDate = t.FromDate,
                    id = t.Id,
                    inqueryCode = t.InqueryCode,
                    isActive = t.IsActive,
                    maxPrice = t.MaxPrice,
                    percent = t.Percent,
                    price = t.Price,
                    title = t.Title,
                    toDate = t.ToDate,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                })
                .OrderByDescending(t => t.id)
                .Take(1)
                .ToList()
                .Select(t => new CreateUpdateGlobalDiscountVM
                {
                    cIds = t.cIds,
                    toDate = t.toDate.ToFaDate(),
                    title = t.title,
                    price = t.price,
                    countUse = t.countUse,
                    discountCode = t.discountCode,
                    formId = t.formId,
                    fromDate = t.fromDate.ToFaDate(),
                    inqueryCode = t.inqueryCode,
                    isActive = t.isActive,
                    maxPrice = t.maxPrice,
                    id = t.id,
                    percent = t.percent,
                    formId_Title = t.formId_Title,
                    cSOWSiteSettingId = t.cSOWSiteSettingId,
                    cSOWSiteSettingId_Title = t.cSOWSiteSettingId_Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<GlobalDiscountMainGridResult> GetList(GlobalDiscountMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new GlobalDiscountMainGrid();

            long? loginUserId = UserService.GetLoginUser()?.UserId;
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.Value);

            var qureResult = db.GlobalDiscounts
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<GlobalDiscount, User>(loginUserId, canSeeAllItems);

            if (searchInput.company.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.GlobalDiscountCompanies.Any(tt => tt.CompanyId == searchInput.company));
            if (!string.IsNullOrEmpty(searchInput.ppfTitle))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.ppfTitle));
            if (!string.IsNullOrEmpty(searchInput.createUser))
                qureResult = qureResult.Where(t => (t.CreateUser.Firstname + " " + t.CreateUser.Lastname).Contains(searchInput.createUser));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.fromDate) && searchInput.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.FromDate.Year == targetDate.Year && t.FromDate.Month == targetDate.Month && t.FromDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.toDate) && searchInput.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.ToDate.Year == targetDate.Year && t.ToDate.Month == targetDate.Month && t.ToDate.Day == targetDate.Day);
            }
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<GlobalDiscountMainGridResult>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    company = t.GlobalDiscountCompanies.Select(tt => tt.Company.Title).ToList(),
                    createDate = t.CreateDate,
                    createUser = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    fromDate = t.FromDate,
                    id = t.Id,
                    isActive = t.IsActive,
                    ppfTitle = t.ProposalForm.Title,
                    title = t.Title,
                    toDate = t.ToDate,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new GlobalDiscountMainGridResult
                {
                    row = ++row,
                    id = t.id,
                    company = string.Join(",", t.company),
                    ppfTitle = t.ppfTitle,
                    createUser = t.createUser,
                    createDate = t.createDate.ToFaDate(),
                    title = t.title,
                    fromDate = t.fromDate.ToFaDate(),
                    toDate = t.toDate.ToFaDate(),
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateGlobalDiscountVM input)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.Value);
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            CreateValidation(input, siteSettingId, loginUserId, canSetSiteSetting);

            GlobalDiscount foundItem = db.GlobalDiscounts
                .Include(t => t.GlobalDiscountCompanies)
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<GlobalDiscount, User>(loginUserId, canSeeAllItems)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);


            if (foundItem.GlobalDiscountCompanies != null)
                foreach (var item in foundItem.GlobalDiscountCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            foundItem.Code = input.discountCode;
            foundItem.UpdateDate = DateTime.Now;
            foundItem.UpdateUserId = loginUserId.Value;
            foundItem.FromDate = input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
            foundItem.InqueryCode = input.inqueryCode;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.MaxPrice = input.maxPrice.ToLongReturnZiro();
            foundItem.MaxUse = input.countUse.ToIntReturnZiro();
            foundItem.Percent = input.percent;
            foundItem.Price = input.price;
            foundItem.ProposalFormId = input.formId.Value;
            foundItem.Title = input.title;
            foundItem.ToDate = input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            foreach (int cId in input.cIds)
                db.Entry(new GlobalDiscountCompany() { CompanyId = cId, GlobalDiscountId = foundItem.Id }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
