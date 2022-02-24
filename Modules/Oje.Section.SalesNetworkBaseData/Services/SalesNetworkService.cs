using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.SalesNetworkBaseData.Interfaces;
using Oje.Section.SalesNetworkBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.SalesNetworkBaseData.Services.EContext;
using Oje.Section.SalesNetworkBaseData.Models.View;

namespace Oje.Section.SalesNetworkBaseData.Services
{
    public class SalesNetworkService : ISalesNetworkService
    {
        readonly SalesNetworkBaseDataDBContext db = null;
        readonly Interfaces.IProposalFormService ProposalFormService = null;
        readonly IUserService UserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public SalesNetworkService(
                SalesNetworkBaseDataDBContext db,
                Interfaces.IProposalFormService ProposalFormService,
                IUserService UserService,
                ISiteSettingService SiteSettingService
            )
        {
            this.db = db;
            this.ProposalFormService = ProposalFormService;
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
        }

        public ApiResult Create(CreateUpdateSalesNetworkVM input)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var loginUserId = UserService.GetLoginUser()?.UserId;
            createValidation(input, siteSettingId, loginUserId);

            SalesNetwork newItem = new SalesNetwork()
            {
                CalceType = input.calceType.Value,
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId.Value,
                Description = input.description,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                SiteSettingId = siteSettingId.Value,
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

        private void createValidation(CreateUpdateSalesNetworkVM input, int? siteSettingId, long? loginUserId)
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
                if (!ProposalFormService.Exist(ppfid, siteSettingId))
                    throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (input.cIds == null || input.cIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (input.calceType == null || string.IsNullOrEmpty(input.calceType.GetAttribute<DisplayAttribute>()?.Name))
                throw BException.GenerateNewException(BMessages.Please_Select_Calculate_Type);
            if (input.type == null || string.IsNullOrEmpty(input.type.GetAttribute<DisplayAttribute>()?.Name))
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (input.userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_One_User);
            if (db.SalesNetworkMarketers.Any(t => t.SalesNetworkId == input.id && t.UserId == input.userId && t.ParentId != null))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var loginUserId = UserService.GetLoginUser()?.UserId;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());
            var foundItem = db.SalesNetworks
                .Include(t => t.SalesNetworkCompanies)
                .Include(t => t.SalesNetworkProposalForms)
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<SalesNetwork, User>(loginUserId, canSeeAllItems)
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
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<SalesNetwork, User>(loginUserId, canSeeAllItems)
                .Select(t => new
                {
                    userId = t.SalesNetworkMarketers.Where(tt => tt.ParentId == null).OrderBy(tt => t.Id).Select(t => t.UserId).FirstOrDefault(),
                    userId_Title = t.SalesNetworkMarketers.Where(tt => tt.ParentId == null).OrderBy(tt => t.Id).Select(t => t.User.Firstname + " " + t.User.Lastname).FirstOrDefault(),
                    calceType = t.CalceType,
                    cIds = t.SalesNetworkCompanies.Select(tt => tt.CompanyId).ToList(),
                    description = t.Description,
                    id = t.Id,
                    isActive = t.IsActive,
                    ppfIds = t.SalesNetworkProposalForms.Select(tt => new { id = tt.ProposalFormId, title = tt.ProposalForm.Title }).ToList(),
                    title = t.Title,
                    type = t.Type
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

            var qureResullt = db.SalesNetworks.Where(t => t.SiteSettingId == siteSettingId).getWhereCreateUserMultiLevelForUserOwnerShip<SalesNetwork, User>(loginUserId, canSeeAllItems);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResullt = qureResullt.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.ppfIds))
                qureResullt = qureResullt.Where(t => t.SalesNetworkProposalForms.Any(tt => tt.ProposalForm.Title.Contains(searchInput.ppfIds)));
            if (searchInput.cIds.ToIntReturnZiro() > 0)
                qureResullt = qureResullt.Where(t => t.SalesNetworkCompanies.Any(tt => tt.CompanyId == searchInput.cIds));
            if (searchInput.type != null)
                qureResullt = qureResullt.Where(t => t.Type == searchInput.type);
            if (searchInput.calceType != null)
                qureResullt = qureResullt.Where(t => t.CalceType == searchInput.calceType);
            if (searchInput.isActive != null)
                qureResullt = qureResullt.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.createUser))
                qureResullt = qureResullt.Where(t => (t.CreateUser.Firstname + " " + t.CreateUser.Lastname).Contains(searchInput.createUser));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResullt = qureResullt.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }

            int row = searchInput.skip;

            return new GridResultVM<SalesNetworkMainGridResulgVM>()
            {
                total = qureResullt.Count(),
                data = qureResullt.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    calceType = t.CalceType,
                    cIds = t.SalesNetworkCompanies.Select(tt => tt.Company.Title).ToList(),
                    id = t.Id,
                    isActive = t.IsActive,
                    ppfIds = t.SalesNetworkProposalForms.Select(tt => tt.ProposalForm.Title).ToList(),
                    title = t.Title,
                    type = t.Type,
                    createUser = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    createDate = t.CreateDate
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
                    calceType = t.calceType.GetAttribute<DisplayAttribute>()?.Name,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateSalesNetworkVM input)
        {
            var siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var loginUserId = UserService.GetLoginUser()?.UserId;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());

            createValidation(input, siteSettingId, loginUserId);

            var foundItem = db.SalesNetworks
                .Include(t => t.SalesNetworkCompanies)
                .Include(t => t.SalesNetworkProposalForms)
                .Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<SalesNetwork, User>(loginUserId, canSeeAllItems)
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


            foundItem.CalceType = input.calceType.Value;
            foundItem.UpdateDate = DateTime.Now;
            foundItem.UpdateUserId = loginUserId.Value;
            foundItem.Description = input.description;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Title = input.title;
            foundItem.Type = input.type.Value;

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
    }
}
