﻿using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
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
using Oje.FileService.Interfaces;
using Oje.PaymentService.Interfaces;
using Oje.Infrastructure.Interfac;
using Microsoft.AspNetCore.Http;
using IProvinceService = Oje.Section.InsuranceContractBaseData.Interfaces.IProvinceService;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class InsuranceContractUserService : IInsuranceContractUserService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly AccountService.Interfaces.IUserService UserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IInsuranceContractService InsuranceContractService = null;
        readonly IRoleService RoleService = null;
        readonly IUploadedFileService uploadedFileService = null;
        readonly IInsuranceContractUserSubCategoryService InsuranceContractUserSubCategoryService = null;
        readonly IInsuranceContractUserBaseInsuranceService InsuranceContractUserBaseInsuranceService = null;
        readonly IBankService BankService = null;
        readonly IProvinceService ProvinceService = null;
        readonly IUserNotifierService UserNotifierService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public InsuranceContractUserService
            (
                InsuranceContractBaseDataDBContext db,
                AccountService.Interfaces.IUserService UserService,
                ISiteSettingService SiteSettingService,
                IInsuranceContractService InsuranceContractService,
                IRoleService RoleService,
                IUploadedFileService uploadedFileService,
                IInsuranceContractUserSubCategoryService InsuranceContractUserSubCategoryService,
                IInsuranceContractUserBaseInsuranceService InsuranceContractUserBaseInsuranceService,
                IBankService BankService,
                IUserNotifierService UserNotifierService,
                IHttpContextAccessor HttpContextAccessor,
                IProvinceService ProvinceService
            )
        {
            this.db = db;
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
            this.InsuranceContractService = InsuranceContractService;
            this.RoleService = RoleService;
            this.uploadedFileService = uploadedFileService;
            this.InsuranceContractUserSubCategoryService = InsuranceContractUserSubCategoryService;
            this.InsuranceContractUserBaseInsuranceService = InsuranceContractUserBaseInsuranceService;
            this.BankService = BankService;
            this.UserNotifierService = UserNotifierService;
            this.HttpContextAccessor = HttpContextAccessor;
            this.ProvinceService = ProvinceService;
        }

        public ApiResult Create(CreateUpdateInsuranceContractUserVM input, InsuranceContractUserStatus status)
        {
            var loginUserId = UserService.GetLoginUser();
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            int roleId = RoleService.CreateOrGetRole("بیمه شدگان گروهی", "InsuranceContractUsers", 1);
            long? parentId = null;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;

            CreateValidation(input, siteSettingId, loginUserId?.UserId, canSetSiteSetting);

            if (input.familyRelation != InsuranceContractUserFamilyRelation.Self)
            {
                parentId = getParentId(input.mainPersonNationalCode, input.mainPersonECode, siteSettingId, loginUserId?.UserId, input.insuranceContractId);
                if (parentId.ToLongReturnZiro() <= 0)
                    throw BException.GenerateNewException(BMessages.Invalid_NationalCode_Or_InsuranceECode);
            }
            long foundUserId = 0;
            var newPassword = RandomService.GeneratePassword(10);
            if (!string.IsNullOrEmpty(input.password))
                newPassword = input.password;

            try
            {
                if (input.familyRelation == InsuranceContractUserFamilyRelation.Self)
                    foundUserId = UserService.CreateForUser(new AccountService.Models.View.CreateUpdateUserForUserVM()
                    {
                        firstname = input.firstName,
                        lastname = input.lastName,
                        nationalCode = input.nationalCode,
                        insuranceECode = input.eCode,
                        birthDate = input.birthDate,
                        email = input.email,
                        mobile = input.mobile,
                        accountCardNo = input.accountCardNo,
                        bankShaba = input.bankShaba,
                        username = input.mobile,
                        password = newPassword,
                        confirmPassword = newPassword,
                        roleIds = new List<int>() { roleId },
                        isActive = input.isActive.ToBooleanReturnFalse(),
                        fatherName = input.fatherName,
                        gender = input.gender,
                        marrageStatus = input.marrageStatus,
                        tell = input.tell,
                        shenasnameNo = input.shenasnameNo,
                        hireDate = input.hireDate,
                        bankId = (input.bankId.ToIntReturnZiro() == 0 ? null : input.bankId.ToIntReturnZiro()),
                        bProvinceId = (input.bProvinceId.ToIntReturnZiro() == 0 ? null : input.bProvinceId.ToIntReturnZiro()),
                        provinceId = (input.provinceId.ToIntReturnZiro() > 0 ? input.provinceId.ToIntReturnZiro() : null),
                        cityId = input.cityId,
                    }, loginUserId?.UserId, loginUserId, canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value).data.ToLongReturnZiro();
            }
            catch (Exception)
            {
                foundUserId = UserService.GetUserIdByNationalEmailMobleEcode(input.nationalCode, input.mobile, input.eCode, loginUserId?.UserId, siteSettingId);
                if (foundUserId <= 0)
                    throw;
            }

            if (input.familyRelation == InsuranceContractUserFamilyRelation.Self && foundUserId <= 0)
                BException.GenerateNewException(BMessages.Can_Not_Create_User);

            var newItem = new InsuranceContractUser()
            {
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId.UserId,
                FamilyRelation = input.familyRelation.Value,
                InsuranceContractId = input.insuranceContractId.Value,
                ParentId = parentId,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Status = status,
                UserId = foundUserId.ToLongReturnZiro() > 0 ? foundUserId : null,
                FirstName = input.firstName,
                LastName = input.lastName,
                BirthDate = input.birthDate.ToEnDate(),
                FatherName = input.fatherName,
                MarrageStatus = input.marrageStatus,
                InsuranceContractUserBaseInsuranceId = (input.baseInsuranceId.ToIntReturnZiro() == 0 ? null : input.baseInsuranceId.ToIntReturnZiro()),
                InsuranceContractUserSubCategoryId = input.subCatId == 0 ? null : input.subCatId,
                NationalCode = input.nationalCode,
                ShenasnameNo = input.shenasnameNo,
                InsuranceECode = input.eCode,
                Gender = input.gender,
                Mobile = input.mobile,
                Custody = input.custody,
                BirthCertificateIssuingPlaceProvinceId = (input.bProvinceId.ToIntReturnZiro() == 0 ? null : input.bProvinceId.ToIntReturnZiro()),
                ProvinceId = (input.provinceId.ToIntReturnZiro() > 0 ? input.provinceId.ToIntReturnZiro() : null),
                CityId = input.cityId,
                BaseInsuranceCode = input.baseInsuranceCode,
                HireExpiredDate = input.hireExpiredDate.ToEnDate()
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.nationalcodeImage != null && input.nationalcodeImage.Length > 0)
                newItem.KartMeliFileUrl = uploadedFileService.UploadNewFile(FileType.ContractUserDocuemnt, input.nationalcodeImage, loginUserId?.UserId, null, newItem.Id, ".jpg,.png,jpeg", true);
            if (input.shenasnamePage1Image != null && input.shenasnamePage1Image.Length > 0)
                newItem.ShenasnamePage1FileUrl = uploadedFileService.UploadNewFile(FileType.ContractUserDocuemnt, input.shenasnamePage1Image, loginUserId?.UserId, null, newItem.Id, ".jpg,.png,jpeg", true);
            if (input.shenasnamePage2Image != null && input.shenasnamePage2Image.Length > 0)
                newItem.ShenasnamePage2FileUrl = uploadedFileService.UploadNewFile(FileType.ContractUserDocuemnt, input.shenasnamePage2Image, loginUserId?.UserId, null, newItem.Id, ".jpg,.png,jpeg", true);
            if (input.bimeImage != null && input.bimeImage.Length > 0)
                newItem.BimeFileUrl = uploadedFileService.UploadNewFile(FileType.ContractUserDocuemnt, input.bimeImage, loginUserId?.UserId, null, newItem.Id, ".jpg,.png,jpeg", true);

            db.SaveChanges();

            if (input.familyRelation == InsuranceContractUserFamilyRelation.Self)
                UserNotifierService.Notify(loginUserId?.UserId, UserNotificationType.NewInsuranceContractUser, new List<PPFUserTypes>() { UserService.GetUserTypePPFInfo(foundUserId, ProposalFilledFormUserType.OwnerUser) }, newItem.Id,
                    "\n" + "کلمه عبور شما : " + newPassword + "\n" + "کد قرارداد شما : " + InsuranceContractService.GetIdByCode(input.insuranceContractId, canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value),
                    canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value, "/InsuranceContractUserPremanent/Index");

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private long? getParentId(string mainPersonNationalCode, string eCode, int? siteSettingId, long? loginUserId, int? insuranceContractId)
        {
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());
            return db.InsuranceContractUsers
                .Where(t => t.InsuranceContractId == insuranceContractId
                            && t.UserId > 0 &&
                            (t.User.Nationalcode == mainPersonNationalCode || t.User.InsuranceECode == eCode))
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractUser, User>(loginUserId, canSeeAllItems)
                .Select(t => t.Id)
                .FirstOrDefault();
        }

        private void CreateValidation(CreateUpdateInsuranceContractUserVM input, int? siteSettingId, long? loginUserId, bool? canSetSiteSetting)
        {
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.insuranceContractId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Contract);
            if (input.familyRelation == InsuranceContractUserFamilyRelation.Self && string.IsNullOrEmpty(input.mobile))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile);
            if (!string.IsNullOrEmpty(input.mobile) && !input.mobile.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (string.IsNullOrEmpty(input.nationalCode))
                throw BException.GenerateNewException(BMessages.Please_Enter_NationalCode);
            if (!input.nationalCode.IsCodeMeli())
                throw BException.GenerateNewException(BMessages.National_Is_Not_Valid);
            if (!InsuranceContractService.Exist(input.insuranceContractId.ToIntReturnZiro(), canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value, loginUserId))
                throw BException.GenerateNewException(BMessages.Please_Select_Contract);
            if (input.familyRelation == null)
                throw BException.GenerateNewException(BMessages.Please_Select_FamilyRelation);
            if (string.IsNullOrEmpty(input.familyRelation.GetAttribute<DisplayAttribute>()?.Name))
                throw BException.GenerateNewException(BMessages.Please_Select_FamilyRelation);
            if (input.familyRelation != InsuranceContractUserFamilyRelation.Self && string.IsNullOrEmpty(input.mainPersonECode) && string.IsNullOrEmpty(input.mainPersonNationalCode))
                throw BException.GenerateNewException(BMessages.Please_Enter_NationalCode_Or_InsuranceECode);
            if (input.familyRelation == InsuranceContractUserFamilyRelation.Self && !string.IsNullOrEmpty(input.mainPersonNationalCode))
                throw BException.GenerateNewException(BMessages.Please_Clear_Main_Person_NationalCode);
            if (input.familyRelation == InsuranceContractUserFamilyRelation.Self && !string.IsNullOrEmpty(input.mainPersonECode))
                throw BException.GenerateNewException(BMessages.Please_Clear_Main_Person_ECode);
            if (!string.IsNullOrEmpty(input.mainPersonNationalCode) && !input.mainPersonNationalCode.IsCodeMeli())
                throw BException.GenerateNewException(BMessages.National_Is_Not_Valid);
            if (!string.IsNullOrEmpty(input.mainPersonNationalCode) && !existByNationalCode(input.id, canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value, loginUserId, input.mainPersonNationalCode, input.insuranceContractId))
                throw BException.GenerateNewException(BMessages.National_Is_Not_Valid);
            if (!string.IsNullOrEmpty(input.mainPersonECode) && !existByECode(input.id, canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value, input.mainPersonECode, input.insuranceContractId, loginUserId))
                throw BException.GenerateNewException(BMessages.Main_Person_Code_Is_Not_Valid);
            var tempBaseId = input.baseInsuranceId.ToIntReturnZiro();
            if (tempBaseId > 0 && !InsuranceContractUserBaseInsuranceService.Exist(canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value, tempBaseId))
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input.subCatId.ToIntReturnZiro() > 0 && !InsuranceContractUserSubCategoryService.Exist(canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value, input.subCatId))
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (!string.IsNullOrEmpty(input.password) && input.password.IsWeekPassword())
                throw BException.GenerateNewException(BMessages.The_Password_Is_Week);
            int tempProvinceId = input.provinceId.ToIntReturnZiro();
            if (tempProvinceId > 0 && input.cityId.ToIntReturnZiro() > 0 && !db.Cities.Any(t => t.Id == input.cityId && t.ProvinceId == tempProvinceId))
                throw BException.GenerateNewException(BMessages.Invalid_City);
            if (tempProvinceId > 0 && !db.Provinces.Any(t => t.Id == tempProvinceId))
                throw BException.GenerateNewException(BMessages.Province_Not_Found);
            if (!string.IsNullOrEmpty(input.baseInsuranceCode) && input.baseInsuranceCode.Length > 20)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input.gender == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Gender);
            if (string.IsNullOrEmpty(input.birthDate))
                throw BException.GenerateNewException(BMessages.Please_Enter_BirthDate);
            if (input.birthDate.ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (!string.IsNullOrEmpty(input.bProvinceId) && input.bProvinceId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Province_Not_Found);
            if (!string.IsNullOrEmpty(input.bankShaba) && input.bankShaba.Length != 24)
                throw BException.GenerateNewException(BMessages.Invalid_ShabaNo);
            if (!string.IsNullOrEmpty(input.nationalCode) && db.InsuranceContractUsers.Any(t => t.NationalCode == input.nationalCode && t.Id != input.id && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Dublicate_NationalCode);
            if (!string.IsNullOrEmpty(input.eCode) && db.InsuranceContractUsers.Any(t => t.InsuranceECode == input.eCode && t.Id != input.id && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Dublicate_ECode);
            if (!string.IsNullOrEmpty(input.hireExpiredDate) && input.hireExpiredDate.ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
        }

        private bool existByNationalCode(long? id, int? siteSettingId, long? loginUserId, string nationalCode, int? insuranceContractId)
        {
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());
            return db.InsuranceContractUsers
                .Where(t => t.Id != id && t.InsuranceContractId == insuranceContractId &&
                            t.UserId > 0 && t.User.Nationalcode == nationalCode)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractUser, User>(loginUserId, canSeeAllItems).Any()
                ;
        }

        private bool existByECode(long? id, int? siteSettingId, string eCode, int? insuranceContractId, long? loginUserId)
        {
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());
            return db.InsuranceContractUsers
                .Where(t => t.Id != id && t.UserId > 0 && t.User.InsuranceECode == eCode &&
                          t.InsuranceContractId == insuranceContractId)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractUser, User>(loginUserId, canSeeAllItems).Any();
        }

        public ApiResult Delete(long? id, InsuranceContractUserStatus status)
        {
            var loginUserId = UserService.GetLoginUser();
            var tempLong = loginUserId?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.UserId.ToLongReturnZiro());
            var foundItem = db.InsuranceContractUsers.Include(t => t.Childs).Where(t => t.Id == id && t.Status == status)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractUser, User>(tempLong, canSeeAllItems)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            UserService.DeleteFlag(foundItem.UserId, siteSettingId, tempLong);

            if (foundItem.Childs != null)
                foreach (var item in foundItem.Childs)
                    db.Entry(item).State = EntityState.Deleted;
            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateInsuranceContractUserVM GetById(long? id, InsuranceContractUserStatus status)
        {
            var loginUserId = UserService.GetLoginUser();
            var tempId = loginUserId?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.UserId.ToLongReturnZiro());
            return
                db.InsuranceContractUsers
                .Where(t => t.Id == id && t.Status == status)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractUser, User>(tempId, canSeeAllItems)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    bankShaba = t.User != null ? t.User.BankShaba : "",
                    eCode = t.InsuranceECode,
                    email = t.User != null ? t.User.Email : "",
                    familyRelation = t.FamilyRelation,
                    insuranceContractId = t.InsuranceContractId,
                    mainPersonECode = t.Parent != null ? t.Parent.User.InsuranceECode : "",
                    firstName = t.FirstName,
                    lastName = t.LastName,
                    birthDate = t.BirthDate,
                    fatherName = t.FatherName,
                    mainPersonNationalCode = t.Parent != null ? t.Parent.User.Nationalcode : "",
                    mobile = t.Mobile,
                    nationalCode = t.NationalCode,
                    bimeImage_address = !string.IsNullOrEmpty(t.BimeFileUrl) ? GlobalConfig.FileAccessHandlerUrl + t.BimeFileUrl : "",
                    nationalcodeImage_address = !string.IsNullOrEmpty(t.KartMeliFileUrl) ? GlobalConfig.FileAccessHandlerUrl + t.KartMeliFileUrl : "",
                    shenasnamePage1Image_address = !string.IsNullOrEmpty(t.ShenasnamePage1FileUrl) ? GlobalConfig.FileAccessHandlerUrl + t.ShenasnamePage1FileUrl : "",
                    shenasnamePage2Image_address = !string.IsNullOrEmpty(t.ShenasnamePage2FileUrl) ? GlobalConfig.FileAccessHandlerUrl + t.ShenasnamePage2FileUrl : "",
                    isActive = t.User != null ? t.User.IsActive : true,
                    baseInsuranceId = t.InsuranceContractUserBaseInsuranceId,
                    subCatId = t.InsuranceContractUserSubCategoryId,
                    gender = t.Gender,
                    marrageStatus = t.MarrageStatus,
                    shenasnameNo = t.ShenasnameNo,
                    bankid = t.User != null ? t.User.BankId : null,
                    hireDate = t.User != null ? t.User.HireDate : null,
                    tell = t.User != null ? t.User.Tell : "",
                    custody = t.Custody,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title,
                    bProvinceId = t.BirthCertificateIssuingPlaceProvinceId,
                    t.ProvinceId,
                    t.CityId,
                    t.BaseInsuranceCode,
                    accountCardNo = t.User != null ? t.User.AccountCardNo : ""
                })
                .OrderByDescending(t => t.id)
                .Take(1)
                .ToList()
                .Select(t => new CreateUpdateInsuranceContractUserVM
                {
                    id = t.id,
                    bankShaba = t.bankShaba,
                    baseInsuranceId = t.baseInsuranceId ,
                    subCatId = t.subCatId,
                    eCode = t.eCode,
                    email = t.email,
                    familyRelation = t.familyRelation,
                    insuranceContractId = t.insuranceContractId,
                    fatherName = t.fatherName,
                    mainPersonECode = t.mainPersonECode,
                    firstName = t.firstName,
                    lastName = t.lastName,
                    birthDate = t.birthDate.ToFaDate(),
                    mainPersonNationalCode = t.mainPersonNationalCode,
                    mobile = t.mobile,
                    nationalCode = t.nationalCode,
                    bimeImage_address = t.bimeImage_address,
                    nationalcodeImage_address = t.nationalcodeImage_address,
                    shenasnamePage1Image_address = t.shenasnamePage1Image_address,
                    shenasnamePage2Image_address = t.shenasnamePage2Image_address,
                    isActive = t.isActive,
                    gender = t.gender,
                    marrageStatus = t.marrageStatus,
                    shenasnameNo = t.shenasnameNo,
                    bankId = t.bankid,
                    hireDate = t.hireDate.ToFaDate(),
                    tell = t.tell,
                    custody = t.custody,
                    cSOWSiteSettingId = t.cSOWSiteSettingId,
                    cSOWSiteSettingId_Title = t.cSOWSiteSettingId_Title,
                    bProvinceId = t.bProvinceId + "",
                    provinceId = t.ProvinceId + "",
                    cityId = t.CityId,
                    baseInsuranceCode = t.BaseInsuranceCode,
                    accountCardNo = t.accountCardNo
                })
                .FirstOrDefault();
        }

        public GridResultVM<InsuranceContractUserMainGridResultVM> GetList(InsuranceContractUserMainGrid searchInput, InsuranceContractUserStatus status)
        {
            if (searchInput == null)
                searchInput = new InsuranceContractUserMainGrid();

            var loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());

            var qureResult = db.InsuranceContractUsers.Where(t => t.Status == status)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractUser, User>(loginUserId, canSeeAllItems)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (searchInput.contract.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.InsuranceContractId == searchInput.contract);
            if (!string.IsNullOrEmpty(searchInput.fistname))
                qureResult = qureResult.Where(t => t.User.Firstname.Contains(searchInput.fistname));
            if (!string.IsNullOrEmpty(searchInput.lastName))
                qureResult = qureResult.Where(t => t.User.Lastname.Contains(searchInput.lastName));
            if (!string.IsNullOrEmpty(searchInput.nationalcode))
                qureResult = qureResult.Where(t => t.User.Nationalcode == searchInput.nationalcode);
            if (!string.IsNullOrEmpty(searchInput.mainPersonNationalcode))
                qureResult = qureResult.Where(t => t.Parent != null && t.Parent.User.Nationalcode == searchInput.mainPersonNationalcode);
            if (!string.IsNullOrEmpty(searchInput.birthDate) && searchInput.birthDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.birthDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.UserId > 0 && t.User.BirthDate != null && t.User.BirthDate.Value.Year == targetDate.Year && t.User.BirthDate.Value.Month == targetDate.Month && t.User.BirthDate.Value.Day == targetDate.Day);
            }
            if (searchInput.familyRelation != null)
                qureResult = qureResult.Where(t => t.FamilyRelation == searchInput.familyRelation);
            if (!string.IsNullOrEmpty(searchInput.createUser))
                qureResult = qureResult.Where(t => (t.CreateUser.Firstname + " " + t.CreateUser.Lastname).Contains(searchInput.createUser));
            if (!string.IsNullOrEmpty(searchInput.eCode))
                qureResult = qureResult.Where(t => t.User.InsuranceECode == searchInput.eCode);
            if (!string.IsNullOrEmpty(searchInput.mainECode))
                qureResult = qureResult.Where(t => t.Parent != null && t.Parent.User.InsuranceECode == searchInput.mainECode);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<InsuranceContractUserMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    birthDate = t.BirthDate,
                    contract = t.InsuranceContract.Title,
                    createByUser = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    eCode = t.InsuranceECode,
                    familyRelation = t.FamilyRelation,
                    fistname = t.FirstName,
                    id = t.Id,
                    isActive = t.User != null ? t.User.IsActive : true,
                    lastname = t.LastName,
                    mainECode = t.Parent != null ? t.Parent.User.InsuranceECode : "",
                    mainPersonNationalcode = t.Parent != null ? t.Parent.User.Nationalcode : "",
                    nationalcode = t.NationalCode,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new InsuranceContractUserMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    contract = t.contract,
                    fistname = t.fistname,
                    lastname = t.lastname,
                    nationalcode = t.nationalcode,
                    mainPersonNationalcode = t.mainPersonNationalcode,
                    birthDate = t.birthDate.ToFaDate(),
                    familyRelation = t.familyRelation.GetAttribute<DisplayAttribute>()?.Name,
                    createUser = t.createByUser,
                    eCode = t.eCode,
                    mainECode = t.mainECode,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateInsuranceContractUserVM input, InsuranceContractUserStatus status)
        {
            var loginUserId = UserService.GetLoginUser();
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.UserId.ToLongReturnZiro());
            var tempId = loginUserId?.UserId;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;

            CreateValidation(input, siteSettingId, loginUserId?.UserId, canSetSiteSetting);

            var foundItem = db.InsuranceContractUsers
                .Where(t => t.Id == input.id && t.Status == status)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractUser, User>(tempId, canSeeAllItems)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (input.familyRelation != foundItem.FamilyRelation)
                throw BException.GenerateNewException(BMessages.FamilyRelation_Can_Not_Be_Edited);

            if (input.familyRelation == InsuranceContractUserFamilyRelation.Self)
            {
                var foundUser = UserService.GetByIdForUser(foundItem.UserId, loginUserId, canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value);
                if (foundUser == null)
                    throw BException.GenerateNewException(BMessages.User_Not_Found);

                foundUser.firstname = input.firstName;
                foundUser.lastname = input.lastName;
                foundUser.nationalCode = input.nationalCode;
                foundUser.birthDate = input.birthDate;
                foundUser.bankShaba = input.bankShaba;
                foundUser.insuranceECode = input.eCode;
                foundUser.mobile = input.mobile;
                foundUser.email = input.email;
                foundUser.gender = input.gender;
                foundUser.tell = input.tell;
                foundUser.hireDate = input.hireDate;
                foundUser.isActive = input.isActive.ToBooleanReturnFalse();
                foundUser.bankId = (input.bankId.ToIntReturnZiro() > 0 ? input.bankId.ToIntReturnZiro() : null);
                foundUser.shenasnameNo = input.shenasnameNo;
                foundUser.bProvinceId = (input.bProvinceId.ToIntReturnZiro() == 0 ? null : input.bProvinceId.ToIntReturnZiro());
                foundUser.provinceId = (input.provinceId.ToIntReturnZiro() > 0 ? input.provinceId.ToIntReturnZiro() : null);
                foundUser.cityId = input.cityId;
                foundUser.accountCardNo = input.accountCardNo;

                if (UserService.UpdateForUser(foundUser, loginUserId?.UserId, loginUserId, canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value).isSuccess == false)
                    throw BException.GenerateNewException(BMessages.UnknownError);
            }

            foundItem.UpdateDate = DateTime.Now;
            foundItem.UpdateUserId = loginUserId.UserId;
            foundItem.InsuranceContractId = input.insuranceContractId.Value;
            foundItem.MarrageStatus = input.marrageStatus;
            foundItem.ShenasnameNo = input.shenasnameNo;
            foundItem.InsuranceContractUserBaseInsuranceId = (input.baseInsuranceId.ToIntReturnZiro() > 0 ? input.baseInsuranceId.ToIntReturnZiro() : null);
            foundItem.InsuranceContractUserSubCategoryId = input.subCatId;
            foundItem.FirstName = input.firstName;
            foundItem.LastName = input.lastName;
            foundItem.FatherName = input.fatherName;
            foundItem.NationalCode = input.nationalCode;
            foundItem.BirthDate = input.birthDate.ToEnDate();
            foundItem.InsuranceECode = input.eCode;
            foundItem.Gender = input.gender;
            foundItem.Mobile = input.mobile;
            foundItem.Custody = input.custody;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;
            foundItem.BirthCertificateIssuingPlaceProvinceId = (input.bProvinceId.ToIntReturnZiro() == 0 ? null : input.bProvinceId.ToIntReturnZiro());
            foundItem.ProvinceId = (input.provinceId.ToIntReturnZiro() > 0 ? input.provinceId.ToIntReturnZiro() : null);
            foundItem.CityId = input.cityId;
            foundItem.BaseInsuranceCode = input.baseInsuranceCode;

            if (input.nationalcodeImage != null && input.nationalcodeImage.Length > 0)
                foundItem.KartMeliFileUrl = uploadedFileService.UploadNewFile(FileType.ContractUserDocuemnt, input.nationalcodeImage, loginUserId?.UserId, null, foundItem.Id, ".jpg,.png,jpeg", true);
            if (input.shenasnamePage1Image != null && input.shenasnamePage1Image.Length > 0)
                foundItem.ShenasnamePage1FileUrl = uploadedFileService.UploadNewFile(FileType.ContractUserDocuemnt, input.shenasnamePage1Image, loginUserId?.UserId, null, foundItem.Id, ".jpg,.png,jpeg", true);
            if (input.shenasnamePage2Image != null && input.shenasnamePage2Image.Length > 0)
                foundItem.ShenasnamePage2FileUrl = uploadedFileService.UploadNewFile(FileType.ContractUserDocuemnt, input.shenasnamePage2Image, loginUserId?.UserId, null, foundItem.Id, ".jpg,.png,jpeg", true);
            if (input.bimeImage != null && input.bimeImage.Length > 0)
                foundItem.BimeFileUrl = uploadedFileService.UploadNewFile(FileType.ContractUserDocuemnt, input.bimeImage, loginUserId?.UserId, null, foundItem.Id, ".jpg,.png,jpeg", true);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult ChangeStatus(long? id, InsuranceContractUserStatus fromStatus, InsuranceContractUserStatus toStatus)
        {
            var loginUserId = UserService.GetLoginUser();
            var tempId = loginUserId?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.UserId.ToLongReturnZiro());
            var foundItem = db.InsuranceContractUsers
                .Include(t => t.Childs)
                .Where(t => t.Id == id && t.Status == fromStatus)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractUser, User>(tempId, canSeeAllItems)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.Childs != null)
                foreach (var item in foundItem.Childs)
                    item.Status = toStatus;
            foundItem.Status = toStatus;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult CreateFromExcel(GlobalExcelFile input, InsuranceContractUserStatus status, int? siteSettingId)
        {
            string resultText = "";

            var excelFile = input?.excelFile;

            if (excelFile == null || excelFile.Length == 0)
                return ApiResult.GenerateNewResult(false, BMessages.Please_Select_File);

            List<CreateUpdateInsuranceContractUserVM> models = ExportToExcel.ConvertToModel<CreateUpdateInsuranceContractUserVM>(input?.excelFile);
            models = models.Where(t => !string.IsNullOrEmpty(t.firstName) && !string.IsNullOrEmpty(t.lastName)).ToList();

            if (models != null && models.Count > 0)
            {
                for (var i = 0; i < models.Count; i++)
                {
                    var model = models[i];
                    try
                    {
                        model.familyRelation = InsuranceContractUserFamilyRelation.Self;
                        if (model.bankId.ToIntReturnZiro() > 0)
                        {
                            model.bankId = BankService.GetByCode(model.bankId);
                            if (model.bankId.ToIntReturnZiro() <= 0)
                                throw BException.GenerateNewException(BMessages.Invalid_Bank);
                        }
                        if (model.baseInsuranceId.ToIntReturnZiro() > 0)
                        {
                            model.baseInsuranceId = InsuranceContractUserBaseInsuranceService.GetByCode(siteSettingId, model.baseInsuranceId + "");
                            if (model.baseInsuranceId.ToIntReturnZiro() <= 0)
                                throw BException.GenerateNewException(BMessages.Invalid_BaseInsurance);
                        }
                        if (model.subCatId.ToIntReturnZiro() > 0)
                        {
                            model.subCatId = InsuranceContractUserSubCategoryService.GetByCode(siteSettingId, model.subCatId.Value.ToString());
                            if (model.subCatId <= 0)
                                throw BException.GenerateNewException(BMessages.Invalid_SubCategory);
                        }
                        if (!string.IsNullOrEmpty(model.bProvinceId))
                        {
                            model.bProvinceId = ProvinceService.GetIdByTitle(model.bProvinceId);
                            if (model.bProvinceId.ToIntReturnZiro() <= 0)
                                throw BException.GenerateNewException(BMessages.Province_Not_Found);
                        }
                        if (!string.IsNullOrEmpty(model.provinceId))
                        {
                            model.provinceId = ProvinceService.GetIdByTitle(model.provinceId);
                            if (model.provinceId.ToIntReturnZiro() <= 0)
                                throw BException.GenerateNewException(BMessages.Province_Not_Found);
                        }
                        if (model.insuranceContractId.ToIntReturnZiro() > 0)
                        {
                            model.insuranceContractId = InsuranceContractService.GetIdByCode2(model.insuranceContractId, siteSettingId);
                            if (model.insuranceContractId.ToIntReturnZiro() <= 0)
                                throw BException.GenerateNewException(BMessages.Invalid_Code);
                        }
                        Create(model, status);

                    }
                    catch (BException be)
                    {
                        resultText += "ردیف " + (i + 1) + " " + be.Message + Environment.NewLine;
                    }
                    catch (Exception)
                    {
                        resultText += "ردیف " + (i + 1) + " " + "خطای نامشخص " + Environment.NewLine;
                    }
                }
            }
            else
                return ApiResult.GenerateNewResult(false, BMessages.No_Row_Detected);

            return ApiResult.GenerateNewResult(
                    true,
                    (string.IsNullOrEmpty(resultText) ? BMessages.Operation_Was_Successfull : BMessages.Some_Operation_Was_Successfull),
                    resultText,
                    string.IsNullOrEmpty(resultText) ? null : "reportResult.txt"
                );
        }

        public ApiResult CreateFromExcelChild(GlobalExcelFile input, InsuranceContractUserStatus status, int? siteSettingId)
        {
            string resultText = "";

            var excelFile = input?.excelFile;

            if (excelFile == null || excelFile.Length == 0)
                return ApiResult.GenerateNewResult(false, BMessages.Please_Select_File);

            List<CreateUpdateInsuranceContractUserChildVM> models = ExportToExcel.ConvertToModel<CreateUpdateInsuranceContractUserChildVM>(input?.excelFile);
            models = models.Where(t => !string.IsNullOrEmpty(t.firstName) && !string.IsNullOrEmpty(t.lastName)).ToList();
            if (models != null && models.Count > 0)
            {
                var tempModels = models.Select(t => new CreateUpdateInsuranceContractUserVM
                {
                    mainPersonECode = t.mainPersonECode,
                    familyRelation = t.familyRelation,
                    firstName = t.firstName,
                    lastName = t.lastName,
                    fatherName = t.fatherName,
                    birthDate = t.birthDate,
                    shenasnameNo = t.shenasnameNo,
                    nationalCode = t.nationalCode,
                    custody = t.custody,
                    marrageStatus = t.marrageStatus,
                    baseInsuranceId = t.baseInsuranceId,
                    baseInsuranceCode = t.baseInsuranceCode,
                    hireExpiredDate = t.hireExpiredDate,
                    gender = t.gender,
                    bProvinceId = t.bProvinceId,
                    mainPersonNationalCode = t.mainPersonNationalCode,
                }).ToList();
                for (var i = 0; i < tempModels.Count; i++)
                {
                    var model = tempModels[i];

                    model.insuranceContractId = db.InsuranceContractUsers
                        .Where(t => t.FamilyRelation == InsuranceContractUserFamilyRelation.Self && (t.NationalCode == model.mainPersonNationalCode || t.InsuranceECode == model.mainPersonECode))
                        .Select(t => t.InsuranceContractId)
                        .FirstOrDefault();

                    if (model.baseInsuranceId.ToIntReturnZiro() > 0)
                    {
                        model.baseInsuranceId = InsuranceContractUserBaseInsuranceService.GetByCode(siteSettingId, model.baseInsuranceId + "");
                        if (model.baseInsuranceId.ToIntReturnZiro() <= 0)
                            throw BException.GenerateNewException(BMessages.Invalid_BaseInsurance);
                    }
                    if (model.subCatId.ToIntReturnZiro() > 0)
                    {
                        model.subCatId = InsuranceContractUserSubCategoryService.GetByCode(siteSettingId, model.subCatId.Value.ToString());
                        if (model.subCatId <= 0)
                            throw BException.GenerateNewException(BMessages.Invalid_SubCategory);
                    }

                    try
                    {

                        if (model.familyRelation == InsuranceContractUserFamilyRelation.Self)
                            throw BException.GenerateNewException(BMessages.Please_Select_FamilyRelation);
                        if (!string.IsNullOrEmpty(model.bProvinceId))
                        {
                            model.bProvinceId = ProvinceService.GetIdByTitle(model.bProvinceId);
                            if (model.bProvinceId.ToIntReturnZiro() <= 0)
                                throw BException.GenerateNewException(BMessages.Province_Not_Found);
                        }

                        Create(model, status);

                    }
                    catch (BException be)
                    {
                        resultText += "ردیف " + (i + 1) + " " + be.Message + Environment.NewLine;
                    }
                    catch (Exception )
                    {
                        resultText += "ردیف " + (i + 1) + " " + "خطای نامشخص " + Environment.NewLine;
                    }
                }
            }
            else
            {
                return ApiResult.GenerateNewResult(false, BMessages.No_Row_Detected);
            }

            return ApiResult.GenerateNewResult(
                    true,
                    (string.IsNullOrEmpty(resultText) ? BMessages.Operation_Was_Successfull : BMessages.Some_Operation_Was_Successfull),
                    resultText,
                    string.IsNullOrEmpty(resultText) ? null : "reportResult.txt"
                );
        }
    }
}
