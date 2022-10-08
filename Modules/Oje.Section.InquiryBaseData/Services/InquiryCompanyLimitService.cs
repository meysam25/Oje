using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Models.View;
using Oje.Section.InquiryBaseData.Models.DB;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.InquiryBaseData.Services.EContext;
using Microsoft.AspNetCore.Http;

namespace Oje.Section.InquiryBaseData.Services
{
    public class InquiryCompanyLimitService : IInquiryCompanyLimitService
    {
        readonly InquiryBaseDataDBContext db = null;
        readonly IUserService UserService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public InquiryCompanyLimitService
            (
                InquiryBaseDataDBContext db, 
                AccountService.Interfaces.ISiteSettingService SiteSettingService, 
                IUserService UserService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.SiteSettingService = SiteSettingService;
            this.UserService = UserService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(CreateUpdateInquiryCompanyLimitVM input)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var loginUserId = UserService.GetLoginUser()?.UserId;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createValidation(input, siteSettingId, loginUserId);

            InquiryCompanyLimit addItem = new InquiryCompanyLimit()
            {
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Type = input.type.Value,
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId.Value
            };

            db.Entry(addItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.comIds != null && input.comIds.Count > 0)
                foreach (var comId in input.comIds)
                    db.Entry(new InquiryCompanyLimitCompany()
                    {
                        CompanyId = comId,
                        InquiryCompanyLimitId = addItem.Id
                    }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        void createValidation(CreateUpdateInquiryCompanyLimitVM input, int? siteSettingId, long? loginUserId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (input.comIds == null || input.comIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (db.InquiryCompanyLimits.Any(t => t.Type == input.type && t.SiteSettingId == siteSettingId && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            var foundItem = db.InquiryCompanyLimits
                .Include(t => t.InquiryCompanyLimitCompanies)
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.InquiryCompanyLimitCompanies != null)
                foreach (var item in foundItem.InquiryCompanyLimitCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            return db.InquiryCompanyLimits
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    type = t.Type,
                    comIds = t.InquiryCompanyLimitCompanies.Select(tt => tt.CompanyId).ToList(),
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                })
                .Take(1)
                .ToList()
                .Select(t => new
                {
                    t.id,
                    type = (int)t.type,
                    t.comIds,
                    t.cSOWSiteSettingId,
                    t.cSOWSiteSettingId_Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<InquiryCompanyLimitMainGridResult> GetList(InquiryCompanyLimitMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new InquiryCompanyLimitMainGrid();

            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            var quaryResult = db.InquiryCompanyLimits
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (searchInput.type != null)
                quaryResult = quaryResult.Where(t => t.Type == searchInput.type);
            if (searchInput.comId.ToIntReturnZiro() > 0)
                quaryResult = quaryResult.Where(t => t.InquiryCompanyLimitCompanies.Any(tt => tt.CompanyId == searchInput.comId));
            if (!string.IsNullOrEmpty(searchInput.createUser))
                quaryResult = quaryResult.Where(t => (t.CreateUser.Firstname + " " + t.CreateUser.Lastname).Contains(searchInput.createUser));
            if(!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var createDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                quaryResult = quaryResult.Where(t => t.CreateDate.Year == createDate.Year && t.CreateDate.Month == createDate.Month && t.CreateDate.Day == createDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                quaryResult = quaryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));
            int row = searchInput.skip;

            return new GridResultVM<InquiryCompanyLimitMainGridResult>
            {
                total = quaryResult.Count(),
                data = quaryResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    comId = t.InquiryCompanyLimitCompanies.Select(tt => tt.Company.Title).ToList(),
                    createDate = t.CreateDate,
                    createUser = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    type = t.Type,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new InquiryCompanyLimitMainGridResult
                {
                    row = ++row,
                    id = t.id,
                    comId = string.Join(",", t.comId),
                    createDate = t.createDate.ToFaDate(),
                    createUser = t.createUser,
                    type = t.type.GetAttribute<DisplayAttribute>()?.Name,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateInquiryCompanyLimitVM input)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var loginUserId = UserService.GetLoginUser()?.UserId;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createValidation(input, siteSettingId, loginUserId);

            var foundItem = db.InquiryCompanyLimits
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Include(t => t.InquiryCompanyLimitCompanies)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.InquiryCompanyLimitCompanies != null)
                foreach (var item in foundItem.InquiryCompanyLimitCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            foundItem.Type = input.type.Value;
            foundItem.UpdateDate = DateTime.Now;
            foundItem.UpdateUserId = loginUserId.Value;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            if (input.comIds != null && input.comIds.Count > 0)
                foreach (var comId in input.comIds)
                    db.Entry(new InquiryCompanyLimitCompany()
                    {
                        CompanyId = comId,
                        InquiryCompanyLimitId = foundItem.Id
                    }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
