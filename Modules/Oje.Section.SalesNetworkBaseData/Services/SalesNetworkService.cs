using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.SalesNetworkBaseData.Interfaces;
using Oje.Section.SalesNetworkBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.SalesNetworkBaseData.Services.EContext;
using Oje.Section.SalesNetworkBaseData.Models.View;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;

namespace Oje.Section.SalesNetworkBaseData.Services
{
    public class SalesNetworkService : ISalesNetworkService
    {
        readonly SalesNetworkBaseDataDBContext db = null;
        readonly Interfaces.IProposalFormService ProposalFormService = null;
        readonly IUserService UserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public SalesNetworkService(
                SalesNetworkBaseDataDBContext db,
                Interfaces.IProposalFormService ProposalFormService,
                IUserService UserService,
                ISiteSettingService SiteSettingService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.ProposalFormService = ProposalFormService;
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(CreateUpdateSalesNetworkVM input)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var loginUserId = UserService.GetLoginUser()?.UserId;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createValidation(input, siteSettingId, loginUserId, canSetSiteSetting);

            SalesNetwork newItem = new SalesNetwork()
            {
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId.Value,
                Description = input.description,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Title = input.title,
                Type = input.type.Value
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.cIds != null)
                foreach (var cid in input.cIds)
                    db.Entry(new SalesNetworkCompany() { CompanyId = cid, SalesNetworkId = newItem.Id }).State = EntityState.Added;
            if (input.ppfIds != null)
                foreach (var ppfid in input.ppfIds)
                    db.Entry(new SalesNetworkProposalForm() { ProposalFormId = ppfid, SalesNetworkId = newItem.Id }).State = EntityState.Added;
            if (input.userId.ToLongReturnZiro() > 0)
                db.Entry(new SalesNetworkMarketer() { CreateDate = DateTime.Now, SalesNetworkId = newItem.Id, UserId = input.userId.ToLongReturnZiro() }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateSalesNetworkVM input, int? siteSettingId, long? loginUserId, bool? canSetSiteSetting)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);

            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.ppfIds == null || input.ppfIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            foreach (var ppfid in input.ppfIds)
                if (!ProposalFormService.Exist(ppfid, (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId)))
                    throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (input.cIds == null || input.cIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (input.type == null || string.IsNullOrEmpty(input.type.GetAttribute<DisplayAttribute>()?.Name))
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (input.userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_One_User);
            if (db.SalesNetworkMarketers.Any(t => t.SalesNetworkId == input.id && t.UserId == input.userId && t.ParentId != null))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
            if (!db.Users.Any(t => t.Id == input.userId && t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId)))
                throw BException.GenerateNewException(BMessages.User_Not_Found);
        }

        public ApiResult Delete(int? id)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var loginUserId = UserService.GetLoginUser()?.UserId;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());
            var foundItem = db.SalesNetworks
                .Include(t => t.SalesNetworkCompanies)
                .Include(t => t.SalesNetworkProposalForms)
                .Where(t => t.Id == id)
                .getWhereCreateUserMultiLevelForUserOwnerShip<SalesNetwork, User>(loginUserId, canSeeAllItems)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var loginUserId = UserService.GetLoginUser()?.UserId;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());
            return db.SalesNetworks
                .Include(t => t.SalesNetworkCompanies)
                .Include(t => t.SalesNetworkProposalForms)
                .Where(t => t.Id == id)
                .getWhereCreateUserMultiLevelForUserOwnerShip<SalesNetwork, User>(loginUserId, canSeeAllItems)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new
                {
                    userId = t.SalesNetworkMarketers.Where(tt => tt.ParentId == null).OrderBy(tt => t.Id).Select(t => t.UserId).FirstOrDefault(),
                    userId_Title = t.SalesNetworkMarketers.Where(tt => tt.ParentId == null).OrderBy(tt => t.Id).Select(t => t.User.Firstname + " " + t.User.Lastname).FirstOrDefault(),
                    cIds = t.SalesNetworkCompanies.Select(tt => tt.CompanyId).ToList(),
                    description = t.Description,
                    id = t.Id,
                    isActive = t.IsActive,
                    ppfIds = t.SalesNetworkProposalForms.Select(tt => new { id = tt.ProposalFormId, title = tt.ProposalForm.Title }).ToList(),
                    title = t.Title,
                    type = t.Type,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<SalesNetworkMainGridResulgVM> GetList(SalesNetworkMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new SalesNetworkMainGrid();
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var loginUserId = UserService.GetLoginUser()?.UserId;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());

            var qureResullt = db.SalesNetworks
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<SalesNetwork, User>(loginUserId, canSeeAllItems);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResullt = qureResullt.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.ppfIds))
                qureResullt = qureResullt.Where(t => t.SalesNetworkProposalForms.Any(tt => tt.ProposalForm.Title.Contains(searchInput.ppfIds)));
            if (searchInput.cIds.ToIntReturnZiro() > 0)
                qureResullt = qureResullt.Where(t => t.SalesNetworkCompanies.Any(tt => tt.CompanyId == searchInput.cIds));
            if (searchInput.type != null)
                qureResullt = qureResullt.Where(t => t.Type == searchInput.type);
            if (searchInput.isActive != null)
                qureResullt = qureResullt.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.createUser))
                qureResullt = qureResullt.Where(t => (t.CreateUser.Firstname + " " + t.CreateUser.Lastname).Contains(searchInput.createUser));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResullt = qureResullt.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResullt = qureResullt.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<SalesNetworkMainGridResulgVM>()
            {
                total = qureResullt.Count(),
                data = qureResullt.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    cIds = t.SalesNetworkCompanies.Select(tt => tt.Company.Title).ToList(),
                    id = t.Id,
                    isActive = t.IsActive,
                    ppfIds = t.SalesNetworkProposalForms.Select(tt => tt.ProposalForm.Title).ToList(),
                    title = t.Title,
                    type = t.Type,
                    createUser = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    createDate = t.CreateDate,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new SalesNetworkMainGridResulgVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    ppfIds = string.Join(",", t.ppfIds),
                    cIds = string.Join(",", t.cIds),
                    createUser = t.createUser,
                    createDate = t.createDate.ToFaDate(),
                    type = t.type.GetAttribute<DisplayAttribute>()?.Name,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateSalesNetworkVM input)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var loginUserId = UserService.GetLoginUser()?.UserId;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createValidation(input, siteSettingId, loginUserId, canSetSiteSetting);

            var foundItem = db.SalesNetworks
                .Include(t => t.SalesNetworkCompanies)
                .Include(t => t.SalesNetworkProposalForms)
                .Where(t => t.Id == input.id)
                .getWhereCreateUserMultiLevelForUserOwnerShip<SalesNetwork, User>(loginUserId, canSeeAllItems)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.SalesNetworkCompanies != null)
                foreach (var item in foundItem.SalesNetworkCompanies)
                    db.Entry(item).State = EntityState.Deleted;
            if (foundItem.SalesNetworkProposalForms != null)
                foreach (var item in foundItem.SalesNetworkProposalForms)
                    db.Entry(item).State = EntityState.Deleted;

            var foundParentUser = db.SalesNetworkMarketers.Where(t => t.SalesNetworkId == foundItem.Id && t.ParentId == null).OrderBy(t => t.Id).FirstOrDefault();
            if (foundParentUser != null)
                foundParentUser.UserId = input.userId.ToLongReturnZiro();
            else
            {
                if (!db.SalesNetworkMarketers.Any(t => t.SalesNetworkId == foundItem.Id))
                {
                    foundParentUser = new SalesNetworkMarketer() { CreateDate = DateTime.Now, SalesNetworkId = foundItem.Id, UserId = input.userId.ToLongReturnZiro() };
                    db.Entry(foundParentUser).State = EntityState.Added;
                }
            }


            foundItem.UpdateDate = DateTime.Now;
            foundItem.UpdateUserId = loginUserId.Value;
            foundItem.Description = input.description;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Title = input.title;
            foundItem.Type = input.type.Value;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            db.SaveChanges();

            if (input.cIds != null)
                foreach (var cid in input.cIds)
                    db.Entry(new SalesNetworkCompany() { CompanyId = cid, SalesNetworkId = foundItem.Id }).State = EntityState.Added;
            if (input.ppfIds != null)
                foreach (var ppfid in input.ppfIds)
                    db.Entry(new SalesNetworkProposalForm() { ProposalFormId = ppfid, SalesNetworkId = foundItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightListMultiLevel(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.SalesNetworks.Where(t => t.SiteSettingId == siteSettingId && t.Type == SalesNetworkType.Multilevel).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }

        public GridResultVM<SalesNetworkReportMainGridResultVM> GetReportList(SalesNetworkReportMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new();

            if (searchInput.snId.ToIntReturnZiro() <= 0 || string.IsNullOrEmpty(searchInput.fromDate) || searchInput.fromDate.ToEnDate() == null || string.IsNullOrEmpty(searchInput.toDate) || searchInput.toDate.ToEnDate() == null || searchInput.userId.ToLongReturnZiro() <= 0)
                return new GridResultVM<SalesNetworkReportMainGridResultVM>() { total = 0, data = new List<SalesNetworkReportMainGridResultVM>() { } };

            var result = db.UserCommissions
                .FromSqlRaw("[dbo].[getMarketerComissions] @fromDate, @toDate, @userId, @networksaleId, @siteSetting",
                    new SqlParameter("@fromDate", searchInput.fromDate.ToEnDate().Value), new SqlParameter("@toDate", searchInput.toDate.ToEnDate().Value), new SqlParameter("@userId", searchInput.userId),
                    new SqlParameter("@networksaleId", searchInput.snId), new SqlParameter("@siteSetting", siteSettingId))
                .ToList();

            int row = searchInput.skip;

            return new GridResultVM<SalesNetworkReportMainGridResultVM>()
            {
                total = result.Count,
                data = result
                .OrderByDescending(t => t.UserId)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new SalesNetworkReportMainGridResultVM
                {
                    row = ++row,
                    id = t.UserId.ToString(),
                    firstName = t.fistname,
                    lastName = t.lastname,
                    level = t.lv.ToString(),
                    commission = t.commission != null ? t.commission.Value.ToString("###,###") : "0",
                    commissionNumber = t.commission != null ? t.commission.Value : 0,
                    typeOfCalc = t.realOrLegal.GetEnumDisplayName(),
                    saleSum = t.saleSum != null ? t.saleSum.Value.ToString("###,###") : "0",
                    saleSumNumber = t.saleSum != null ? t.saleSum.Value : 0,
                    role = t.role
                })
                .ToList()
            };
        }

        public object GetUserListBySaleNetworkId(int? siteSettingId, int? id, Select2SearchVM searchInput)
        {
            var foundUserId = db.SalesNetworks.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).SelectMany(t => t.SalesNetworkMarketers).Select(t => t.UserId).FirstOrDefault();

            if (foundUserId > 0)
            {
                var hasPagination = false;
                int take = 50;
                searchInput = searchInput ?? new();
                if (searchInput.page == null || searchInput.page <= 0)
                    searchInput.page = 1;
                var result = db.UserLevels.FromSqlRaw("[dbo].[getUserLevelList] @userId, @skip, @take, @search",
                new SqlParameter("@userId", foundUserId), new SqlParameter("@skip", (searchInput.page.Value - 1) * take), new SqlParameter("@take", take), new SqlParameter("@search", (string.IsNullOrEmpty(searchInput.search) ? "" : ("%" + searchInput.search + "%"))))
                .ToList();

                if (result.Count >= take)
                    hasPagination = true;

                return new { results = result, pagination = new { more = hasPagination } };
            }

            return new { results = new List<object>(), pagination = new { more = false } };
        }

        public object GetReportChart(int? siteSettingId, SalesNetworkReportMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            if (searchInput.snId.ToIntReturnZiro() <= 0 || string.IsNullOrEmpty(searchInput.fromDate) || searchInput.fromDate.ToEnDate() == null || string.IsNullOrEmpty(searchInput.toDate) || searchInput.toDate.ToEnDate() == null || searchInput.userId.ToLongReturnZiro() <= 0)
                return new { };

            var result = db.UserCommissions
                .FromSqlRaw("[dbo].[getMarketerComissions] @fromDate, @toDate, @userId, @networksaleId, @siteSetting",
                    new SqlParameter("@fromDate", searchInput.fromDate.ToEnDate().Value), new SqlParameter("@toDate", searchInput.toDate.ToEnDate().Value), new SqlParameter("@userId", searchInput.userId),
                    new SqlParameter("@networksaleId", searchInput.snId), new SqlParameter("@siteSetting", siteSettingId))
                .ToList();


            while (result.Any(t => t.parentid != null && !result.Any(tt => tt.UserId == t.parentid)))
            {
                var foundFirstUser = result.Where(t => t.parentid != null && !result.Any(tt => tt.UserId == t.parentid)).FirstOrDefault();
                var foundUser = db.Users.Where(t => t.Id == foundFirstUser.parentid).Select(t => new { t.Firstname, t.Lastname, t.ParentId, role = t.UserRoles.Select(tt => tt.Role.Title).FirstOrDefault(), id = t.Id, t.Username }).FirstOrDefault();
                result.Add(new Models.SP.UserCommission()
                {
                    commission = 0,
                    fistname = (foundUser?.Firstname + foundUser?.Lastname + "").Trim() == "" ? foundUser?.Username :  foundUser?.Firstname,
                    lastname = foundUser?.Lastname,
                    lv = foundFirstUser.lv - 1,
                    parentid = foundUser?.ParentId,
                    role = foundUser?.role,
                    saleSum = 0,
                    UserId = foundUser != null ? foundUser.id : 0
                });
                var foundParentUser = result.Where(t => t.UserId == searchInput.userId).FirstOrDefault();
                if (foundParentUser != null)
                    foundParentUser.parentid = null;
            }

            var data = new List<object>();
            var nodes = new List<object>();

            foreach (var user in result)
                if (user.parentid != null)
                    data.Add(new List<string>() { user.parentid.Value.ToString(), user.UserId.ToString() });
            nodes.AddRange(
                result.Select(t => new { id = t.UserId.ToString(), title = t.role, name = t.fistname + " " + t.lastname + (t.commission != null && t.commission != 0 ? ("(" + t.commission.Value.ToString("###,###") + ")") : "") }).ToList()
                );


            return new List<object>()
            {
                new
                {
                    data = data,
                    nodes = nodes,
                    type = "organization",
                    keys = new List<string>() { "from", "to" },
                    levels = new List<object>()
                    {
                        new { level= 0, color = "silver", dataLabels = new { color = "black" } },
                        new { level= 1, color = "#6610f2" },
                        new { level= 2, color = "#980104" },
                        new { level= 3, color = "#359154" },
                        new { level= 4, color = "#ffa500" },
                        new { level= 5, color = "#ffc107" }
                    },
                    colorByPoint = false,
                    color = "#007ad0",
                    dataLabels =  new { color = "white" },
                    borderColor = "silver",
                    nodeWidth = 65
                }
            };
        }
    }
}
