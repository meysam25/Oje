using Oje.AccountManager.Interfaces;
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

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class InsuranceContractManager : IInsuranceContractManager
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IInsuranceContractCompanyManager InsuranceContractCompanyManager = null;
        readonly IInsuranceContractTypeManager InsuranceContractTypeManager = null;
        readonly Interfaces.IProposalFormManager ProposalFormManager = null;
        readonly IUserManager UserManager = null;
        readonly ISiteSettingManager SiteSettingManager = null;
        readonly IUploadedFileManager uploadedFileManager = null;
        public InsuranceContractManager
            (
                InsuranceContractBaseDataDBContext db,
                IUserManager UserManager,
                ISiteSettingManager SiteSettingManager,
                Interfaces.IProposalFormManager ProposalFormManager,
                IInsuranceContractTypeManager InsuranceContractTypeManager,
                IInsuranceContractCompanyManager InsuranceContractCompanyManager,
                IUploadedFileManager uploadedFileManager
            )
        {
            this.db = db;
            this.UserManager = UserManager;
            this.SiteSettingManager = SiteSettingManager;
            this.ProposalFormManager = ProposalFormManager;
            this.InsuranceContractTypeManager = InsuranceContractTypeManager;
            this.InsuranceContractCompanyManager = InsuranceContractCompanyManager;
            this.uploadedFileManager = uploadedFileManager;
        }

        public ApiResult Create(CreateUpdateInsuranceContractVM input)
        {
            long? loginUserId = UserManager.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUserIds = UserManager.GetChildsUserId(loginUserId.ToLongReturnZiro());
            CreateValidation(input, loginUserId, siteSettingId, childUserIds);

            var newItem = new InsuranceContract()
            {
                Code = input.code.Value,
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId.Value,
                Description = input.description,
                FromDate = input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value,
                InsuranceContractCompanyId = input.insuranceContractCompanyId.Value,
                InsuranceContractTypeId = input.insuranceContractTypeId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                MonthlyPrice = input.monthlyPrice.ToLongReturnZiro(),
                ProposalFormId = input.proposalFormId.Value,
                SiteSettingId = siteSettingId.Value,
                Title = input.title,
                ToDate = input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.contractDocument != null && input.contractDocument.Length > 0)
            {
                newItem.ContractDocumentUrl = uploadedFileManager.UploadNewFile(FileType.CompanyLogo, input.contractDocument, loginUserId, null, newItem.Id, ".pdf,.doc,.docx,.xlsx", false);
                db.SaveChanges();
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateInsuranceContractVM input, long? loginUserId, int? siteSettingId, List<long> childUserIds)
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
            if (input.insuranceContractTypeId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Contract_Type);
            if (!InsuranceContractTypeManager.Exist(input.insuranceContractTypeId.ToIntReturnZiro(), siteSettingId, childUserIds))
                throw BException.GenerateNewException(BMessages.Please_Select_Contract_Type);
            if (input.insuranceContractCompanyId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Legal_Company);
            if (!InsuranceContractCompanyManager.Exist(input.insuranceContractCompanyId.ToIntReturnZiro(), siteSettingId, childUserIds))
                throw BException.GenerateNewException(BMessages.Please_Select_Legal_Company);
            if (input.proposalFormId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (!string.IsNullOrEmpty(input.description) && input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (!ProposalFormManager.Exist(input.proposalFormId.ToIntReturnZiro(), siteSettingId))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (input.monthlyPrice != null && input.monthlyPrice < 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            if (string.IsNullOrEmpty(input.fromDate) || input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_FromDate);
            if (string.IsNullOrEmpty(input.toDate) || input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_ToDate);
            if (input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value > input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value)
                throw BException.GenerateNewException(BMessages.ToDate_Should_Be_Greader_Then_FromYear);
            if (db.InsuranceContracts.Any(t => t.Id != input.id && t.SiteSettingId == siteSettingId && t.Code == input.code))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            long? loginUserId = UserManager.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUserIds = UserManager.GetChildsUserId(loginUserId.ToLongReturnZiro());

            var foundItem = db.InsuranceContracts.Where(t => t.SiteSettingId == siteSettingId && (childUserIds == null ||childUserIds.Contains(t.CreateUserId)) && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateInsuranceContractVM GetById(int? id)
        {
            long? loginUserId = UserManager.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUserIds = UserManager.GetChildsUserId(loginUserId.ToLongReturnZiro());

            return db.InsuranceContracts
                .Where(t => t.SiteSettingId == siteSettingId && (childUserIds == null || childUserIds.Contains(t.CreateUserId)) && t.Id == id)
                .OrderByDescending(t => t.Id)
                .Select(t => new
                {
                    code = t.Code,
                    description = t.Description,
                    fromDate = t.FromDate,
                    id = t.Id,
                    insuranceContractCompanyId = t.InsuranceContractCompanyId,
                    insuranceContractTypeId = t.InsuranceContractTypeId,
                    isActive = t.IsActive,
                    monthlyPrice = t.MonthlyPrice,
                    proposalFormId = t.ProposalFormId,
                    proposalFormId_Title = t.ProposalForm.Title,
                    title = t.Title,
                    toDate = t.ToDate,
                    contractDocument_address = GlobalConfig.FileAccessHandlerUrl + t.ContractDocumentUrl
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
                    insuranceContractTypeId = t.insuranceContractTypeId,
                    isActive = t.isActive,
                    monthlyPrice = t.monthlyPrice,
                    proposalFormId = t.proposalFormId,
                    proposalFormId_Title = t.proposalFormId_Title,
                    title = t.title,
                    toDate = t.toDate.ToFaDate(),
                    contractDocument_address = t.contractDocument_address
                })
                .FirstOrDefault();
        }

        public GridResultVM<InsuranceContractMainGridResultVM> GetList(InsuranceContractMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new InsuranceContractMainGrid();
            long? loginUserId = UserManager.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUserIds = UserManager.GetChildsUserId(loginUserId.ToLongReturnZiro());

            var qureResult = db.InsuranceContracts.Where(t => t.SiteSettingId == siteSettingId &&  (childUserIds == null || childUserIds.Contains(t.CreateUserId)));

            if (searchInput.contractCompany.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.InsuranceContractCompanyId == searchInput.contractCompany);
            if (searchInput.contractType.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.InsuranceContractTypeId == searchInput.contractType);
            if (!string.IsNullOrEmpty(searchInput.ppfTitle))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.ppfTitle));
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

            var row = searchInput.skip;

            return new GridResultVM<InsuranceContractMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    code = t.Code,
                    contractCompany = t.InsuranceContractCompany.Title,
                    contractType = t.InsuranceContractCompany.Title,
                    fromDate = t.FromDate,
                    id = t.Id,
                    isActive = t.IsActive,
                    ppfTitle = t.ProposalForm.Title,
                    title = t.Title,
                    toDate = t.ToDate,
                    createDate = t.CreateDate,
                    createUser = t.CreateUser.Firstname + " " + t.CreateUser.Lastname
                })
                .ToList()
                .Select(t => new InsuranceContractMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    contractCompany = t.contractCompany,
                    contractType = t.contractType,
                    ppfTitle = t.ppfTitle,
                    title = t.title,
                    createUser = t.createUser,
                    createDate = t.createDate.ToFaDate(),
                    code = t.code + "",
                    fromDate = t.fromDate.ToFaDate(),
                    toDate = t.toDate.ToFaDate(),
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateInsuranceContractVM input)
        {
            long? loginUserId = UserManager.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUserIds = UserManager.GetChildsUserId(loginUserId.ToLongReturnZiro());

            CreateValidation(input, loginUserId, siteSettingId, childUserIds);

            var foundItem = db.InsuranceContracts.Where(t => t.SiteSettingId == siteSettingId && (childUserIds == null || childUserIds.Contains(t.CreateUserId)) && t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (input.contractDocument != null && input.contractDocument.Length > 0)
                foundItem.ContractDocumentUrl = uploadedFileManager.UploadNewFile(FileType.CompanyLogo, input.contractDocument, loginUserId, null, foundItem.Id, ".pdf,.doc,.docx", false);

            foundItem.Code = input.code.Value;
            foundItem.CreateDate = DateTime.Now;
            foundItem.CreateUserId = loginUserId.Value;
            foundItem.Description = input.description;
            foundItem.FromDate = input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
            foundItem.InsuranceContractCompanyId = input.insuranceContractCompanyId.Value;
            foundItem.InsuranceContractTypeId = input.insuranceContractTypeId.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.MonthlyPrice = input.monthlyPrice.ToLongReturnZiro();
            foundItem.ProposalFormId = input.proposalFormId.Value;
            foundItem.SiteSettingId = siteSettingId.Value;
            foundItem.Title = input.title;
            foundItem.ToDate = input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public bool Exist(int id, int? siteSettingId, List<long> userChilds)
        {
            return db.InsuranceContracts.Any(t => t.Id == id && t.SiteSettingId == siteSettingId && (userChilds == null || userChilds.Contains(t.CreateUserId)));
        }

        public object GetLightList()
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };
            long? loginUserId = UserManager.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUserIds = UserManager.GetChildsUserId(loginUserId.ToLongReturnZiro());

            result.AddRange(db.InsuranceContracts
                .Where(t => t.SiteSettingId == siteSettingId && (childUserIds == null || childUserIds.Contains(t.CreateUserId)))
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
