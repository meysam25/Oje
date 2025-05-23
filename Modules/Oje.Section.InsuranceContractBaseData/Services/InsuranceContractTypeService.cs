﻿using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Oje.Section.InsuranceContractBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.InsuranceContractBaseData.Services.EContext;
using Microsoft.AspNetCore.Http;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class InsuranceContractTypeService : IInsuranceContractTypeService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IUserService UserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public InsuranceContractTypeService(
                InsuranceContractBaseDataDBContext db,
                IUserService UserService,
                ISiteSettingService SiteSettingService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(CreateUpdateInsuranceContractTypeVM input)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            CreateValidation(input, loginUserId, siteSettingId, canSetSiteSetting);

            db.Entry(new InsuranceContractType()
            {
                Title = input.title,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId.Value,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Description = input.desc
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateInsuranceContractTypeVM input, long? loginUserId, int? siteSettingId, bool? canSetSiteSetting)
        {
            var sId = (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (db.InsuranceContractTypes.Any(t => t.Title == input.title && t.Id != input.id && t.SiteSettingId == sId))
                throw BException.GenerateNewException(BMessages.Dublicate_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (db.InsuranceContractTypes.Any(t => t.Id != input.id && t.SiteSettingId == sId && t.Title == input.title))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
            if (!string.IsNullOrEmpty(input.desc) && input.desc.Length > 4000)
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        public ApiResult Delete(int? id)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToIntReturnZiro());

            var foundItem = db.InsuranceContractTypes
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractType, User>(loginUserId, canSeeAllItems)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateInsuranceContractTypeVM GetById(int? id)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToIntReturnZiro());

            return db.InsuranceContractTypes
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractType, User>(loginUserId, canSeeAllItems)
                .Select(t => new CreateUpdateInsuranceContractTypeVM
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                    desc = t.Description,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                }).FirstOrDefault();
        }

        public GridResultVM<InsuranceContractTypeMainGridResultVM> GetList(InsuranceContractTypeMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new InsuranceContractTypeMainGrid();
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.Value);

            var qureResult = db.InsuranceContractTypes
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractType, User>(loginUserId, canSeeAllItems);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.createUser))
                qureResult = qureResult.Where(t => (t.CreateUser.Firstname + " " + t.CreateUser.Lastname).Contains(searchInput.createUser));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                DateTime targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<InsuranceContractTypeMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    createUser = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    createDate = t.CreateDate,
                    isActive = t.IsActive,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new InsuranceContractTypeMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    createUser = t.createUser,
                    createDate = t.createDate.ToFaDate(),
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateInsuranceContractTypeVM input)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.Value);

            CreateValidation(input, loginUserId, siteSettingId, canSetSiteSetting);

            var foundItem = db.InsuranceContractTypes
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractType, User>(loginUserId, canSeeAllItems)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Title = input.title;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.UpdateDate = DateTime.Now;
            foundItem.UpdateUserId = loginUserId.Value;
            foundItem.Description = input.desc;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            long? loginUserId = UserService.GetLoginUser()?.UserId;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.Value);

            result.AddRange(db.InsuranceContractTypes
                .Where(t => t.SiteSettingId == siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractType, User>(loginUserId, canSeeAllItems)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title
                }).ToList());

            return result;
        }

        public bool Exist(List<int> ids, int? siteSettingId, long? loginUserId)
        {
            if (ids == null)
                ids = new();
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());
            return db.InsuranceContractTypes.Where(t => ids.Contains(t.Id) && t.SiteSettingId == siteSettingId).getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractType, User>(loginUserId, canSeeAllItems).Count() == ids.Count;
        }

        public object GetLightList(int? contractId, int? siteSettingId)
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            long? loginUserId = UserService.GetLoginUser()?.UserId;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.Value);

            result.AddRange(db.InsuranceContractTypes
                .Where(t => t.SiteSettingId == siteSettingId && t.InsuranceContractInsuranceContractTypes.Any(tt => tt.InsuranceContractId == contractId))
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractType, User>(loginUserId, canSeeAllItems)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title
                }).ToList());

            return result;
        }

        public bool Exist(int? id, int? siteSettingId)
        {
            return db.InsuranceContractTypes.Any(t => t.Id == id && t.SiteSettingId == siteSettingId);
        }

        public object GetLightListBySiteSettingId(int? siteSettingId)
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.InsuranceContractTypes.Where(t => t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title
                }).ToList());

            return result;
        }
    }
}
