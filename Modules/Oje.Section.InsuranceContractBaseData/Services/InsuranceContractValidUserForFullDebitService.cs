using Oje.AccountService.Interfaces;
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

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class InsuranceContractValidUserForFullDebitService : IInsuranceContractValidUserForFullDebitService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IInsuranceContractService InsuranceContractService = null;
        readonly IUserService UserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public InsuranceContractValidUserForFullDebitService
            (
                InsuranceContractBaseDataDBContext db,
                IInsuranceContractService InsuranceContractService,
                IUserService UserService,
                ISiteSettingService SiteSettingService
            )
        {
            this.db = db;
            this.UserService = UserService;
            this.InsuranceContractService = InsuranceContractService;
            this.SiteSettingService = SiteSettingService;
        }

        public ApiResult Create(CreateUpdateInsuranceContractValidUserForFullDebitVM input)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            long? loginUserId = UserService.GetLoginUser()?.UserId;

            CreateValidation(input, siteSettingId, loginUserId);

            db.Entry(new InsuranceContractValidUserForFullDebit()
            {
                CountUse = input.countUse.ToIntReturnZiro(),
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId.Value,
                InsuranceContractId = input.insuranceContractId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Mobile = input.mobile,
                NationalCode = input.nationalCode,
                SiteSettingId = siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateInsuranceContractValidUserForFullDebitVM input, int? siteSettingId, long? loginUserId)
        {
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.insuranceContractId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Contract);
            if (!InsuranceContractService.Exist(input.insuranceContractId.ToIntReturnZiro(), siteSettingId, loginUserId))
                throw BException.GenerateNewException(BMessages.Please_Select_Contract);
            if (string.IsNullOrEmpty(input.mobile))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile);
            if (!input.mobile.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (ExistByMobile(input.mobile, siteSettingId, input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Mobile);
            if (string.IsNullOrEmpty(input.nationalCode))
                throw BException.GenerateNewException(BMessages.Please_Enter_NationalCode);
            if (!input.nationalCode.IsCodeMeli())
                throw BException.GenerateNewException(BMessages.National_Is_Not_Valid);
            if (ExistByNationalCode(input.nationalCode, siteSettingId, input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_NationalCode);
            if (input.countUse.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_CountUsed);
        }

        private bool ExistByMobile(string mobile, int? siteSettingId, long? id)
        {
            return db.InsuranceContractValidUserForFullDebits.Any(t => t.Mobile == mobile && t.SiteSettingId == siteSettingId && t.Id != id);
        }

        private bool ExistByNationalCode(string nationalCode, int? siteSettingId, long? id)
        {
            return db.InsuranceContractValidUserForFullDebits.Any(t => t.NationalCode == nationalCode && t.SiteSettingId == siteSettingId && t.Id != id);
        }

        public ApiResult Delete(long? id)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToIntReturnZiro());

            var foundItem = db.InsuranceContractValidUserForFullDebits.Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractValidUserForFullDebit, User>(loginUserId, canSeeAllItems)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateInsuranceContractValidUserForFullDebitVM GetById(long? id)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToIntReturnZiro());

            return db.InsuranceContractValidUserForFullDebits
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractValidUserForFullDebit, User>(loginUserId, canSeeAllItems)
                .Select(t => new CreateUpdateInsuranceContractValidUserForFullDebitVM
                {
                    countUse = t.CountUse,
                    id = t.Id,
                    insuranceContractId = t.InsuranceContractId,
                    isActive = t.IsActive,
                    mobile = t.Mobile,
                    nationalCode = t.NationalCode
                })
                .FirstOrDefault();
        }

        public GridResultVM<InsuranceContractValidUserForFullDebitMainGridResultVM> GetList(InsuranceContractValidUserForFullDebitMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new InsuranceContractValidUserForFullDebitMainGrid();
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToIntReturnZiro());

            var qureResult = db.InsuranceContractValidUserForFullDebits.Where(t => t.SiteSettingId == siteSettingId).getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractValidUserForFullDebit, User>(loginUserId, canSeeAllItems);

            if (searchInput.contract.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.InsuranceContractId == searchInput.contract);
            if (!string.IsNullOrEmpty(searchInput.mobile))
                qureResult = qureResult.Where(t => t.Mobile == searchInput.mobile);
            if (!string.IsNullOrEmpty(searchInput.nationalCode))
                qureResult = qureResult.Where(t => t.NationalCode == searchInput.nationalCode);
            if (searchInput.countUse.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.CountUse == searchInput.countUse);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.createUser))
                qureResult = qureResult.Where(t => (t.CreateUser.Firstname + " " + t.CreateUser.Lastname).Contains(searchInput.createUser));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }

            int row = searchInput.skip;

            return new GridResultVM<InsuranceContractValidUserForFullDebitMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    contract = t.InsuranceContract.Title,
                    countUse = t.CountUse,
                    isActive = t.IsActive,
                    mobile = t.Mobile,
                    nationalCode = t.NationalCode,
                    createUser = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    createDate = t.CreateDate
                })
                .ToList()
                .Select(t => new InsuranceContractValidUserForFullDebitMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    contract = t.contract,
                    createUser = t.createUser,
                    createDate = t.createDate.ToFaDate(),
                    nationalCode = t.nationalCode,
                    mobile = t.mobile,
                    countUse = t.countUse,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateInsuranceContractValidUserForFullDebitVM input)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToIntReturnZiro());

            CreateValidation(input, siteSettingId, loginUserId);

            var foundItem = db.InsuranceContractValidUserForFullDebits.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContractValidUserForFullDebit, User>(loginUserId, canSeeAllItems)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.CountUse = input.countUse.ToIntReturnZiro();
            foundItem.UpdateDate = DateTime.Now;
            foundItem.UpdateUserId = loginUserId.Value;
            foundItem.InsuranceContractId = input.insuranceContractId.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Mobile = input.mobile;
            foundItem.NationalCode = input.nationalCode;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult CreateFromExcel(GlobalExcelFile input)
        {
            string resultText = "";

            var excelFile = input?.excelFile;

            if (excelFile == null || excelFile.Length == 0)
                return ApiResult.GenerateNewResult(false, BMessages.Please_Select_File);

            List<CreateUpdateInsuranceContractValidUserForFullDebitVM> models = ExportToExcel.ConvertToModel<CreateUpdateInsuranceContractValidUserForFullDebitVM>(input?.excelFile);
            if (models != null && models.Count > 0)
            {
                for (var i = 0; i < models.Count; i++)
                {
                    var model = models[i];
                    try
                    {
                        Create(model);

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
