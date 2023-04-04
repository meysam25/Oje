using Oje.AccountService.Interfaces;
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
using Microsoft.AspNetCore.Http;
using ISmsSendingQueueService = Oje.Sms.Interfaces.ISmsSendingQueueService;
using ISmsValidationHistoryService = Oje.Sms.Interfaces.ISmsValidationHistoryService;
using Oje.Security.Interfaces;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class InsuranceContractService : IInsuranceContractService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IInsuranceContractCompanyService InsuranceContractCompanyService = null;
        readonly IInsuranceContractTypeService InsuranceContractTypeService = null;
        readonly IUserService UserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IUploadedFileService uploadedFileService = null;
        readonly IInsuranceContractProposalFormService InsuranceContractProposalFormService = null;
        readonly Interfaces.IProposalFormService ProposalFormService = null;
        readonly IInsuranceContractTypeRequiredDocumentService InsuranceContractTypeRequiredDocumentService = null;
        readonly ISmsSendingQueueService SmsSendingQueueService = null;
        readonly ISmsValidationHistoryService SmsValidationHistoryService = null;
        readonly IUserLoginLogoutLogService UserLoginLogoutLogService = null;
        readonly AccountService.Interfaces.IRoleService RoleService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        static string accpetFileExtension = ".pdf,.doc,.docx,.xlsx";

        public InsuranceContractService
            (
                InsuranceContractBaseDataDBContext db,
                IUserService UserService,
                ISiteSettingService SiteSettingService,
                IInsuranceContractTypeService InsuranceContractTypeService,
                IInsuranceContractCompanyService InsuranceContractCompanyService,
                IUploadedFileService uploadedFileService,
                IInsuranceContractProposalFormService InsuranceContractProposalFormService,
                Interfaces.IProposalFormService ProposalFormService,
                IInsuranceContractTypeRequiredDocumentService InsuranceContractTypeRequiredDocumentService,
                ISmsValidationHistoryService SmsValidationHistoryService,
                IHttpContextAccessor HttpContextAccessor,
                ISmsSendingQueueService SmsSendingQueueService,
                IUserLoginLogoutLogService UserLoginLogoutLogService,
                AccountService.Interfaces.IRoleService RoleService
            )
        {
            this.db = db;
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
            this.InsuranceContractTypeService = InsuranceContractTypeService;
            this.InsuranceContractCompanyService = InsuranceContractCompanyService;
            this.uploadedFileService = uploadedFileService;
            this.InsuranceContractProposalFormService = InsuranceContractProposalFormService;
            this.ProposalFormService = ProposalFormService;
            this.InsuranceContractTypeRequiredDocumentService = InsuranceContractTypeRequiredDocumentService;
            this.HttpContextAccessor = HttpContextAccessor;
            this.SmsSendingQueueService = SmsSendingQueueService;
            this.SmsValidationHistoryService = SmsValidationHistoryService;
            this.UserLoginLogoutLogService = UserLoginLogoutLogService;
            this.RoleService = RoleService;
        }

        public ApiResult Create(CreateUpdateInsuranceContractVM input)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;

            CreateValidation(input, loginUserId, siteSettingId, canSetSiteSetting);

            var newItem = new InsuranceContract()
            {
                Code = input.code.Value,
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId.Value,
                Description = input.description,
                FromDate = input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value,
                InsuranceContractCompanyId = input.insuranceContractCompanyId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                MonthlyPrice = input.monthlyPrice.ToLongReturnZiro(),
                InsuranceContractProposalFormId = input.proposalFormId.Value,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Title = input.title,
                ToDate = input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value,
                ProposalFormId = input.rPFId
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.insuranceContractTypeIds != null && input.insuranceContractTypeIds.Any())
                foreach (var id in input.insuranceContractTypeIds)
                    db.Entry(new InsuranceContractInsuranceContractType() { InsuranceContractId = newItem.Id, InsuranceContractTypeId = id }).State = EntityState.Added;

            if (input.contractDocument != null && input.contractDocument.Length > 0)
                newItem.ContractDocumentUrl = uploadedFileService.UploadNewFile(FileType.CompanyLogo, input.contractDocument, loginUserId, null, newItem.Id, accpetFileExtension, false);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateInsuranceContractVM input, long? loginUserId, int? siteSettingId, bool? canSetSiteSetting)
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
            if (input.code.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Code);
            if (input.insuranceContractTypeIds == null || input.insuranceContractTypeIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Contract_Type);
            if (!InsuranceContractTypeService.Exist(input.insuranceContractTypeIds, (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value), loginUserId))
                throw BException.GenerateNewException(BMessages.Please_Select_Contract_Type);
            if (input.insuranceContractCompanyId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Legal_Company);
            if (!InsuranceContractCompanyService.Exist(input.insuranceContractCompanyId.ToIntReturnZiro(), (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value), loginUserId))
                throw BException.GenerateNewException(BMessages.Please_Select_Legal_Company);
            if (input.proposalFormId.ToIntReturnZiro() <= 0 && input.rPFId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (!string.IsNullOrEmpty(input.description) && input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if ((input.proposalFormId.ToIntReturnZiro() > 0 && !InsuranceContractProposalFormService.Exist(input.proposalFormId.ToIntReturnZiro(), canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value)))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (input.rPFId.ToIntReturnZiro() > 0 && !ProposalFormService.Exist(input.rPFId.ToIntReturnZiro(), canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (input.monthlyPrice != null && input.monthlyPrice < 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            if (string.IsNullOrEmpty(input.fromDate) || input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_FromDate);
            if (string.IsNullOrEmpty(input.toDate) || input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_ToDate);
            if (input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value > input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value)
                throw BException.GenerateNewException(BMessages.ToDate_Should_Be_Greader_Then_FromYear);
            if (db.InsuranceContracts.Any(t => t.Id != input.id && t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value) && t.Code == input.code))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
            if (input.contractDocument != null && input.contractDocument.Length > 0 && !input.contractDocument.IsValidExtension(accpetFileExtension))
                throw BException.GenerateNewException(BMessages.Invalid_File);
        }

        public ApiResult Delete(int? id)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());

            var foundItem = db.InsuranceContracts
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContract, User>(loginUserId, canSeeAllItems)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateInsuranceContractVM GetById(int? id)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());

            return db.InsuranceContracts
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContract, User>(loginUserId, canSeeAllItems)
                .OrderByDescending(t => t.Id)
                .Select(t => new
                {
                    code = t.Code,
                    description = t.Description,
                    fromDate = t.FromDate,
                    id = t.Id,
                    insuranceContractCompanyId = t.InsuranceContractCompanyId,
                    insuranceContractTypeIds = t.InsuranceContractInsuranceContractTypes.Select(tt => tt.InsuranceContractTypeId).ToList(),
                    isActive = t.IsActive,
                    monthlyPrice = t.MonthlyPrice,
                    proposalFormId = t.InsuranceContractProposalFormId,
                    proposalFormId_Title = t.InsuranceContractProposalForm.Title,
                    rPFId = t.ProposalFormId,
                    rPFId_Title = t.ProposalForm.Title,
                    title = t.Title,
                    toDate = t.ToDate,
                    contractDocument_address = GlobalConfig.FileAccessHandlerUrl + t.ContractDocumentUrl,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                })
                .Take(1)
                .ToList()
                .Select(t => new CreateUpdateInsuranceContractVM
                {
                    code = t.code,
                    description = t.description,
                    fromDate = t.fromDate.ToFaDate(),
                    id = t.id,
                    insuranceContractCompanyId = t.insuranceContractCompanyId,
                    insuranceContractTypeIds = t.insuranceContractTypeIds,
                    isActive = t.isActive,
                    monthlyPrice = t.monthlyPrice,
                    proposalFormId = t.proposalFormId,
                    proposalFormId_Title = !string.IsNullOrEmpty(t.proposalFormId_Title) ? t.proposalFormId_Title : "",
                    rPFId = t.rPFId,
                    rPFId_Title = string.IsNullOrEmpty(t.rPFId_Title) ? "" : t.rPFId_Title,
                    title = t.title,
                    toDate = t.toDate.ToFaDate(),
                    contractDocument_address = t.contractDocument_address,
                    cSOWSiteSettingId = t.cSOWSiteSettingId,
                    cSOWSiteSettingId_Title = t.cSOWSiteSettingId_Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<InsuranceContractMainGridResultVM> GetList(InsuranceContractMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new InsuranceContractMainGrid();
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());

            var qureResult = db.InsuranceContracts
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContract, User>(loginUserId, canSeeAllItems)
                ;

            if (searchInput.contractCompany.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.InsuranceContractCompanyId == searchInput.contractCompany);
            if (searchInput.contractType.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.InsuranceContractInsuranceContractTypes.Any(tt => tt.InsuranceContractTypeId == searchInput.contractType));
            if (!string.IsNullOrEmpty(searchInput.ppfTitle))
                qureResult = qureResult.Where(t => t.InsuranceContractProposalForm.Title.Contains(searchInput.ppfTitle));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.code.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.Code == searchInput.code);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
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
            if (!string.IsNullOrEmpty(searchInput.createUser))
                qureResult = qureResult.Where(t => (t.CreateUser.Firstname + " " + t.CreateUser.Lastname).Contains(searchInput.createUser));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            var row = searchInput.skip;

            return new GridResultVM<InsuranceContractMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    code = t.Code,
                    contractCompany = t.InsuranceContractCompany.Title,
                    contractType = t.InsuranceContractInsuranceContractTypes.Select(tt => tt.InsuranceContractType.Title),
                    fromDate = t.FromDate,
                    id = t.Id,
                    isActive = t.IsActive,
                    ppfTitle = t.InsuranceContractProposalForm.Title,
                    title = t.Title,
                    toDate = t.ToDate,
                    createDate = t.CreateDate,
                    createUser = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new InsuranceContractMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    contractCompany = t.contractCompany,
                    contractType = String.Join(',', t.contractType),
                    ppfTitle = t.ppfTitle,
                    title = t.title,
                    createUser = t.createUser,
                    createDate = t.createDate.ToFaDate(),
                    code = t.code + "",
                    fromDate = t.fromDate.ToFaDate(),
                    toDate = t.toDate.ToFaDate(),
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateInsuranceContractVM input)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;

            CreateValidation(input, loginUserId, siteSettingId, canSetSiteSetting);

            var foundItem =
                db.InsuranceContracts
                .Include(t => t.InsuranceContractInsuranceContractTypes)
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContract, User>(loginUserId, canSeeAllItems)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (input.contractDocument != null && input.contractDocument.Length > 0)
                foundItem.ContractDocumentUrl = uploadedFileService.UploadNewFile(FileType.CompanyLogo, input.contractDocument, loginUserId, null, foundItem.Id, accpetFileExtension, false);

            foundItem.Code = input.code.Value;
            foundItem.Description = input.description;
            foundItem.FromDate = input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
            foundItem.InsuranceContractCompanyId = input.insuranceContractCompanyId.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.MonthlyPrice = input.monthlyPrice.ToLongReturnZiro();
            foundItem.InsuranceContractProposalFormId = input.proposalFormId.Value;
            foundItem.Title = input.title;
            foundItem.ToDate = input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
            foundItem.ProposalFormId = input.rPFId;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            if (foundItem.InsuranceContractInsuranceContractTypes != null)
                foreach (var item in foundItem.InsuranceContractInsuranceContractTypes)
                    db.Entry(item).State = EntityState.Deleted;


            if (input.insuranceContractTypeIds != null && input.insuranceContractTypeIds.Any())
                foreach (var id in input.insuranceContractTypeIds)
                    db.Entry(new InsuranceContractInsuranceContractType() { InsuranceContractId = foundItem.Id, InsuranceContractTypeId = id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public bool Exist(int id, int? siteSettingId, long? loginUserId)
        {
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());
            return db.InsuranceContracts.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContract, User>(loginUserId, canSeeAllItems).Any();
        }

        public object GetLightList()
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var canSeeAllItems = UserService.CanSeeAllItems(loginUserId.ToLongReturnZiro());

            result.AddRange(db.InsuranceContracts
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .getWhereCreateUserMultiLevelForUserOwnerShip<InsuranceContract, User>(loginUserId, canSeeAllItems)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title
                })
                .ToList());

            return result;
        }

        public ApiResult IsValid(contractUserInput input, int? siteSettingId, IpSections curIp)
        {
            isValidValidation(input, siteSettingId);

            if (curIp != null)
                SmsSendingQueueService.LoginWithSMS(new Sms.Models.View.RegLogSMSVM() { username = input.username }, curIp, siteSettingId, SmsValidationHistoryType.LoginWithSmsForContract);

            return ApiResult.GenerateNewResult(true, BMessages.Please_Enter_SMSCode, new
            {
                data = new { input.username, input.nationalCode },
                labels = new List<object>() { new { inputName = "code", labelText = "کد  به شماره  " + input.username + " ارسال گردید" } },
                stepId = "contractConfirmSMS",
                countDownId = "tryAginButtonCDForContract"
            });
        }

        private void isValidValidation(contractUserInput input, int? siteSettingId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.username))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile);
            if (!input.username.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (string.IsNullOrEmpty(input.nationalCode))
                throw BException.GenerateNewException(BMessages.Please_Enter_NationalCode);
            if (!input.nationalCode.IsCodeMeli())
                throw BException.GenerateNewException(BMessages.Invalid_NationaCode);
            if (!db.InsuranceContracts.Any(t => t.FromDate <= DateTime.Now && t.ToDate >= DateTime.Now && t.IsActive == true && t.InsuranceContractProposalFormId > 0 && t.SiteSettingId == siteSettingId && t.InsuranceContractUsers
                    .Any(tt => tt.Status == InsuranceContractUserStatus.Premanent && tt.User.IsActive == true && tt.User.Username == input.username && tt.User.Nationalcode == input.nationalCode)))
                throw BException.GenerateNewException(BMessages.Validation_Error);

        }

        public string GetFormJsonConfig(contractUserInput input, int? siteSettingId)
        {
            isValidValidation(input, siteSettingId);

            string isValid = SmsValidationHistoryService.ValidatePreUsedBy(input.username.ToLongReturnZiro(), input.code.ToIntReturnZiro(), HttpContextAccessor.GetIpAddress(), -3600, SmsValidationHistoryType.LoginWithSmsForContract);
            if (string.IsNullOrEmpty(isValid))
                throw BException.GenerateNewException(BMessages.Invalid_Code, ApiResultErrorCode.ValidationError,  0);

            return db.InsuranceContracts
                .Where(t => t.FromDate <= DateTime.Now && t.ToDate >= DateTime.Now && t.IsActive == true && t.InsuranceContractProposalFormId > 0 && t.SiteSettingId == siteSettingId
                    && t.InsuranceContractUsers
                            .Any(tt => tt.Status == InsuranceContractUserStatus.Premanent && tt.User.IsActive == true && tt.User.Username == input.username && tt.User.Nationalcode == input.nationalCode))
                .Select(t => t.InsuranceContractProposalForm.JsonConfig)
                .FirstOrDefault();
        }

        public InsuranceContract GetByCode(contractUserInput input, int? siteSettingId)
        {
            string isValid = SmsValidationHistoryService.ValidatePreUsedBy(input.username.ToLongReturnZiro(), input.code.ToIntReturnZiro(), HttpContextAccessor.GetIpAddress(), -3600, SmsValidationHistoryType.LoginWithSmsForContract);
            if (string.IsNullOrEmpty(isValid))
                throw BException.GenerateNewException(BMessages.Invalid_Code, ApiResultErrorCode.ValidationError, 0);

            return db.InsuranceContracts
                .Where(t =>
                            t.FromDate <= DateTime.Now && t.ToDate >= DateTime.Now && t.IsActive == true && t.SiteSettingId == siteSettingId &&
                            t.InsuranceContractUsers.Any(tt => tt.Status == InsuranceContractUserStatus.Premanent && tt.User.IsActive == true && tt.User.Username == input.username && tt.User.Nationalcode == input.nationalCode)
                        )
                .FirstOrDefault();
        }

        public ContractTermsInfo GetTermsInfo(contractUserInput input, int? siteSettingId)
        {
            isValidValidation(input, siteSettingId);

            string isValid = SmsValidationHistoryService.ValidatePreUsedBy(input.username.ToLongReturnZiro(), input.code.ToIntReturnZiro(), HttpContextAccessor.GetIpAddress(), -3600, SmsValidationHistoryType.LoginWithSmsForContract);
            if (string.IsNullOrEmpty(isValid))
                throw BException.GenerateNewException(BMessages.Invalid_Code, ApiResultErrorCode.ValidationError, 0);

            return db.InsuranceContracts
                .Where(t => t.FromDate <= DateTime.Now && t.ToDate >= DateTime.Now && t.IsActive == true && t.InsuranceContractProposalFormId > 0 && t.SiteSettingId == siteSettingId && t.InsuranceContractUsers
                    .Any(tt => tt.Status == InsuranceContractUserStatus.Premanent &&
                        tt.User.Username == input.username && tt.User.Nationalcode == input.nationalCode))
                        .Select(t => new ContractTermsInfo()
                        {
                            termsDescription = t.InsuranceContractProposalForm.TermTemplate,
                            firstName = t.InsuranceContractUsers.Where(tt => tt.Status == InsuranceContractUserStatus.Premanent &&
                                                                        tt.User.Username == input.username && tt.User.Nationalcode == input.nationalCode).Select(tt => tt.User.Firstname).FirstOrDefault(),
                            lastName = t.InsuranceContractUsers.Where(tt => tt.Status == InsuranceContractUserStatus.Premanent &&
                                                                        tt.User.Username == input.username && tt.User.Nationalcode == input.nationalCode).Select(tt => tt.User.Lastname).FirstOrDefault(),
                            nationalCode = input.nationalCode,
                            mobile = input.username,
                            contractDocumentUrl = !string.IsNullOrEmpty(t.ContractDocumentUrl) ? (GlobalConfig.FileAccessHandlerUrl + t.ContractDocumentUrl) : ""
                        })
                        .FirstOrDefault();
        }

        public List<IdTitle> GetFamilyMemberList(contractUserInput input, int? siteSettingId)
        {
            isValidValidation(input, siteSettingId);

            string isValid = SmsValidationHistoryService.ValidatePreUsedBy(input.username.ToLongReturnZiro(), input.code.ToIntReturnZiro(), HttpContextAccessor.GetIpAddress(), -3600, SmsValidationHistoryType.LoginWithSmsForContract);
            if (string.IsNullOrEmpty(isValid))
                throw BException.GenerateNewException(BMessages.Invalid_Code, ApiResultErrorCode.ValidationError, 0);

            var contractUserId = db.InsuranceContracts
                .Where(t => t.FromDate <= DateTime.Now && t.ToDate >= DateTime.Now && t.IsActive == true && t.InsuranceContractProposalFormId > 0 && t.SiteSettingId == siteSettingId)
                .SelectMany(t => t.InsuranceContractUsers)
                .Where(tt => tt.Status == InsuranceContractUserStatus.Premanent && tt.User.IsActive == true && tt.User.Username == input.username && tt.User.Nationalcode == input.nationalCode)
                .Select(t => t.Id)
                .FirstOrDefault();

            return
                new List<IdTitle>() { new IdTitle() { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } }
                .Union(
                    db.InsuranceContractUsers
                    .Where(t => t.Id == contractUserId).Select(t => new { id = t.Id, title = t.FirstName + " " + t.LastName }).Select(t => new IdTitle() { id = t.id + "", title = t.title })
                    .ToList()
                        .Union(
                              db.InsuranceContractUsers
                            .Where(t => t.ParentId == contractUserId).Select(t => new { id = t.Id, title = t.FirstName + " " + t.LastName }).Select(t => new IdTitle() { id = t.id + "", title = t.title })
                            .ToList()
                        )
                )
                .ToList()
                ;
        }

        public List<IdTitle> GetContractTypeList(contractUserInput input, int? siteSettingId)
        {
            isValidValidation(input, siteSettingId);

            string isValid = SmsValidationHistoryService.ValidatePreUsedBy(input.username.ToLongReturnZiro(), input.code.ToIntReturnZiro(), HttpContextAccessor.GetIpAddress(), -3600, SmsValidationHistoryType.LoginWithSmsForContract);
            if (string.IsNullOrEmpty(isValid))
                throw BException.GenerateNewException(BMessages.Invalid_Code, ApiResultErrorCode.ValidationError, 0);

            return
                new List<IdTitle>() { new IdTitle() { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } }
                .Union(
                    db.InsuranceContracts
                    .Where(t =>
                            t.FromDate <= DateTime.Now && t.ToDate >= DateTime.Now && t.IsActive == true && t.InsuranceContractProposalFormId > 0 && t.SiteSettingId == siteSettingId &&
                            t.InsuranceContractUsers.Any(tt => tt.Status == InsuranceContractUserStatus.Premanent && tt.User.IsActive == true && tt.User.Username == input.username && tt.User.Nationalcode == input.nationalCode)
                           )
                    .SelectMany(t => t.InsuranceContractInsuranceContractTypes)
                    .Select(t => t.InsuranceContractType)
                    .Select(t => new { t.Id, t.Title })
                    .Select(t => new IdTitle { id = t.Id + "", title = t.Title })
                    .ToList()
                )
                .ToList()
                ;
        }

        public RequiredDocumentVM GetRequiredDocuments(contractUserInput input, int? insuranceContractTypeId, int? siteSettingId)
        {
            string isValid = SmsValidationHistoryService.ValidatePreUsedBy(input.username.ToLongReturnZiro(), input.code.ToIntReturnZiro(), HttpContextAccessor.GetIpAddress(), -3600, SmsValidationHistoryType.LoginWithSmsForContract);
            if (string.IsNullOrEmpty(isValid))
                throw BException.GenerateNewException(BMessages.Invalid_Code, ApiResultErrorCode.ValidationError, 0);

            var foundType = db.InsuranceContracts
                .Where
                (
                    t => t.FromDate <= DateTime.Now && t.ToDate >= DateTime.Now && t.IsActive == true && t.SiteSettingId == siteSettingId &&
                         t.InsuranceContractUsers.Any(tt => tt.Status == InsuranceContractUserStatus.Premanent && tt.User.IsActive == true && tt.User.Username == input.username && tt.User.Nationalcode == input.nationalCode)
                )
                .SelectMany(t => t.InsuranceContractInsuranceContractTypes)
                .Where(t => t.InsuranceContractTypeId == insuranceContractTypeId)
                .Select(t => new { id = t.InsuranceContractId, tId = t.InsuranceContractTypeId, desc = t.InsuranceContractType.Description })
                .FirstOrDefault();


            return InsuranceContractTypeRequiredDocumentService.GetRequiredDocuments(foundType?.id, foundType?.tId, siteSettingId, foundType?.desc);
        }

        public IdTitle GetIdTitleBy(contractUserInput contractInfo, int? siteSettingId)
        {
            return db.InsuranceContracts
                .Where
                (t =>
                    t.FromDate <= DateTime.Now && t.ToDate >= DateTime.Now && t.IsActive == true && t.SiteSettingId == siteSettingId &&
                    t.InsuranceContractUsers.Any(tt => tt.Status == InsuranceContractUserStatus.Premanent && tt.User.IsActive == true && tt.User.Username == contractInfo.username && tt.User.Nationalcode == contractInfo.nationalCode)
                )
                .Select(t => new IdTitle { id = t.Id.ToString(), title = t.Title })
                .FirstOrDefault();
        }

        public string GetIdByCode(int? id, int? siteSettingId)
        {
            return db.InsuranceContracts.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).Select(t => t.Code).FirstOrDefault() + "";
        }

        public bool Exist(int? id, int? siteSettingId)
        {
            return db.InsuranceContracts.Any(t => t.Id == id && t.SiteSettingId == siteSettingId);
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.InsuranceContracts.Where(t => t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title
                }).ToList());

            return result;
        }

        public object IsValidSMS(contractUserInput input, int? curSiteSettingId, IpSections curIp)
        {
            isValidValidation(input, curSiteSettingId);

            var foundUser = UserService.GetBy(input.username, curSiteSettingId);

            if (foundUser != null && (foundUser.IsDelete == true || foundUser.IsActive == false))
                throw BException.GenerateNewException(BMessages.Validation_Error);

            if (string.IsNullOrEmpty(input.code))
                throw BException.GenerateNewException(BMessages.Please_Enter_Code, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
            if (input.code.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Just_Use_Number_For_Code, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
            if (input.code.Length > 20)
                throw BException.GenerateNewException(BMessages.Validation_Error, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
            string isValid = SmsValidationHistoryService.ValidatePreUsedBy(input.username.ToLongReturnZiro(), input.code.ToIntReturnZiro(), curIp, -3600, SmsValidationHistoryType.LoginWithSmsForContract);
            if (string.IsNullOrEmpty(isValid))
                throw BException.GenerateNewException(BMessages.Invalid_Code, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);

            UserService.setCookieForThisUser(foundUser, new AccountService.Models.View.LoginVM() { rememberMe = true }, RoleService.HasAnyAutoRefreshRole(foundUser.Id), RoleService.HasAnySeeOtherSiteRoleConfig(foundUser.Id));
            UserService.UpdateUserSessionFileName(foundUser?.Id, foundUser.tempLastSession);
            UserLoginLogoutLogService.Create(foundUser.Id, UserLoginLogoutLogType.LoginWithPhoneNumber, SiteSettingService.GetSiteSetting()?.Id, true, BMessages.Operation_Was_Successfull.GetEnumDisplayName());


            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull, new
            {
                stepId = "ContractSearchPanel",
                postUrl = "/Contract/Create",
                //userfullname = (!string.IsNullOrEmpty(foundUser.Firstname) ? (foundUser.Firstname + " " + foundUser.Lastname) : foundUser.Username),
                //isUser = UserService.isWebsiteUser(foundUser.Id)
            });
        }

        public object ConfirmSMSForCreate(contractUserInput input, int? curSiteSettingId, IpSections curIp)
        {
            isValidValidation(input, curSiteSettingId);

            SmsSendingQueueService.LoginWithSMS(new Sms.Models.View.RegLogSMSVM() { username = input.username }, curIp, curSiteSettingId, SmsValidationHistoryType.SMSForCreateContract);


            return ApiResult.GenerateNewResult(true, BMessages.Please_Enter_SMSCode);
        }
    }
}
