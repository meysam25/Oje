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
    public class InsuranceContractCompanyService : IInsuranceContractCompanyService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IUserService UserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public InsuranceContractCompanyService
            (
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

        public ApiResult Create(CreateUpdateInsuranceContractCompanyVM input)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;

            CreateValidation(input, loginUserId, siteSettingId);

            db.Entry(new InsuranceContractCompany()
            {
                Address = input.address,
                CodeEghtesadi = input.codeEghtesadi,
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                ModirAmelName = input.modirAmelName,
                Phone = input.phone,
                RabeteSazmaniName = input.rabeteSazmaniName,
                ShenaseMeli = input.shenaseMeli,
                ShomareSabt = input.shomareSabt,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Title = input.title
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateInsuranceContractCompanyVM input, long? loginUserId, int? siteSettingId)
        {
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (!string.IsNullOrEmpty(input.shomareSabt) && input.shomareSabt.Length > 50)
                throw BException.GenerateNewException(BMessages.RegisterNumber_Can_Not_Be_More_Then_50_chars);
            if (!string.IsNullOrEmpty(input.codeEghtesadi) && input.codeEghtesadi.Length > 50)
                throw BException.GenerateNewException(BMessages.EconomicCode_Can_Not_Be_More_Then_50_chars);
            if (!string.IsNullOrEmpty(input.shenaseMeli) && input.shenaseMeli.Length > 50)
                throw BException.GenerateNewException(BMessages.GlobalNumber_Can_Not_Be_More_Then_50_chars);
            if (!string.IsNullOrEmpty(input.address) && input.address.Length > 4000)
                throw BException.GenerateNewException(BMessages.Address_Length_Can_Not_Be_More_Then_4000);
            if (!string.IsNullOrEmpty(input.phone) && input.phone.Length > 50)
                throw BException.GenerateNewException(BMessages.PhoneNumber_Can_Not_Be_More_Then_50_chars);
            if (!string.IsNullOrEmpty(input.rabeteSazmaniName) && input.rabeteSazmaniName.Length > 100)
                throw BException.GenerateNewException(BMessages.Company_Connector_Person_Name_Can_Not_Be_More_Then_100_chars);
            if (!string.IsNullOrEmpty(input.modirAmelName) && input.modirAmelName.Length > 100)
                throw BException.GenerateNewException(BMessages.Service_Name_Can_Not_Be_More_Then_100_chars);
        }

        public ApiResult Delete(int? id)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToIntReturnZiro());

            var foundItem = db.InsuranceContractCompanies.Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractCompany, User>(loginUserId, canSeeAllItems)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateInsuranceContractCompanyVM GetById(int? id)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToIntReturnZiro());


            return db.InsuranceContractCompanies
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractCompany, User>(loginUserId, canSeeAllItems)
                .Select(t => new CreateUpdateInsuranceContractCompanyVM
                {
                    address = t.Address,
                    codeEghtesadi = t.CodeEghtesadi,
                    id = t.Id,
                    isActive = t.IsActive,
                    modirAmelName = t.ModirAmelName,
                    phone = t.Phone,
                    rabeteSazmaniName = t.RabeteSazmaniName,
                    shenaseMeli = t.ShenaseMeli,
                    shomareSabt = t.ShomareSabt,
                    title = t.Title,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<InsuranceContractCompanyMainGridResultVM> GetList(InsuranceContractCompanyMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new InsuranceContractCompanyMainGrid();
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToIntReturnZiro());

            var qureResult = db.InsuranceContractCompanies
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractCompany, User>(loginUserId, canSeeAllItems);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.phone))
                qureResult = qureResult.Where(t => t.Phone.Contains(searchInput.phone));
            if (!string.IsNullOrEmpty(searchInput.rabeteSazmaniName))
                qureResult = qureResult.Where(t => t.RabeteSazmaniName.Contains(searchInput.rabeteSazmaniName));
            if (!string.IsNullOrEmpty(searchInput.modirAmelName))
                qureResult = qureResult.Where(t => t.ModirAmelName.Contains(searchInput.modirAmelName));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.createUser))
                qureResult = qureResult.Where(t => (t.CreateUser.Firstname + " " + t.CreateUser.Lastname).Contains(searchInput.createUser));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<InsuranceContractCompanyMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    isActive = t.IsActive,
                    createUser = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    createDate = t.CreateDate,
                    modirAmelName = t.ModirAmelName,
                    phone = t.Phone,
                    rabeteSazmaniName = t.RabeteSazmaniName,
                    title = t.Title,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new InsuranceContractCompanyMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    createUser = t.createUser,
                    createDate = t.createDate.ToFaDate(),
                    title = t.title,
                    phone = t.phone,
                    rabeteSazmaniName = t.rabeteSazmaniName,
                    modirAmelName = t.modirAmelName,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateInsuranceContractCompanyVM input)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;

            CreateValidation(input, loginUserId, siteSettingId);

            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.Value);

            var foundItem = db.InsuranceContractCompanies
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractCompany, User>(loginUserId, canSeeAllItems)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Address = input.address;
            foundItem.CodeEghtesadi = input.codeEghtesadi;
            foundItem.UpdateDate = DateTime.Now;
            foundItem.UpdateUserId = loginUserId.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.ModirAmelName = input.modirAmelName;
            foundItem.Phone = input.phone;
            foundItem.RabeteSazmaniName = input.rabeteSazmaniName;
            foundItem.ShenaseMeli = input.shenaseMeli;
            foundItem.ShomareSabt = input.shomareSabt;
            foundItem.Title = input.title;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public bool Exist(int id, int? siteSettingId, long? loginUserId)
        {
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());
            return db.InsuranceContractCompanies.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractCompany, User>(loginUserId, canSeeAllItems).Any();
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            long? loginUserId = UserService.GetLoginUser()?.UserId;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.Value);

            result.AddRange(db.InsuranceContractCompanies
                .Where(t => t.SiteSettingId == siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractCompany, User>(loginUserId, canSeeAllItems)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title
                })
                .ToList());

            return result;
        }
    }
}
