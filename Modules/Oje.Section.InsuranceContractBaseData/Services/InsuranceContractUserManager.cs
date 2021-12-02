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
    public class InsuranceContractUserManager : IInsuranceContractUserManager
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IUserManager UserManager = null;
        readonly ISiteSettingManager SiteSettingManager = null;
        readonly IInsuranceContractManager InsuranceContractManager = null;
        readonly IRoleManager RoleManager = null;
        readonly IUploadedFileManager uploadedFileManager = null;
        public InsuranceContractUserManager
            (
                InsuranceContractBaseDataDBContext db,
                IUserManager UserManager,
                ISiteSettingManager SiteSettingManager,
                IInsuranceContractManager InsuranceContractManager,
                IRoleManager RoleManager,
                IUploadedFileManager uploadedFileManager
            )
        {
            this.db = db;
            this.UserManager = UserManager;
            this.SiteSettingManager = SiteSettingManager;
            this.InsuranceContractManager = InsuranceContractManager;
            this.RoleManager = RoleManager;
            this.uploadedFileManager = uploadedFileManager;
        }

        public ApiResult Create(CreateUpdateInsuranceContractUserVM input, InsuranceContractUserStatus status)
        {
            var loginUserId = UserManager.GetLoginUser();
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUserIds = UserManager.GetChildsUserId(loginUserId.UserId.ToLongReturnZiro());
            int roleId = RoleManager.CreateOrGetRole("بیمه شدگان گروهی", "InsuranceContractUsers", 1);
            long? parentId = null;

            CreateValidation(input, status, siteSettingId, loginUserId?.UserId, childUserIds);

            if (input.familyRelation != InsuranceContractUserFamilyRelation.Self)
            {
                parentId = getParentId(input.mainPersonNationalCode, input.eCode, siteSettingId, childUserIds, input.insuranceContractId);
                if (parentId.ToLongReturnZiro() <= 0)
                    throw BException.GenerateNewException(BMessages.Invalid_NationalCode_Or_InsuranceECode);
            }
            long foundUserId = 0;
            try
            {
                foundUserId = UserManager.CreateForUser(new AccountManager.Models.View.CreateUpdateUserForUserVM()
                {
                    firstname = input.firstName,
                    lastname = input.lastName,
                    nationalCode = input.nationalCode,
                    insuranceECode = input.eCode,
                    birthDate = input.birthDate,
                    email = input.email,
                    mobile = input.mobile,
                    bankAccount = input.bankAcount,
                    bankShaba = input.bankShaba,
                    username = input.mobile,
                    password = input.mobile + "Aa@" + input.nationalCode,
                    confirmPassword = input.mobile + "Aa@" + input.nationalCode,
                    roleIds = new List<int>() { roleId },
                    isActive = input.isActive.ToBooleanReturnFalse()
                }, loginUserId?.UserId, loginUserId, siteSettingId).data.ToLongReturnZiro();
            }
            catch (Exception)
            {
                foundUserId = UserManager.GetUserIdByNationalEmailMobleEcode(input.nationalCode, input.mobile, input.email, input.eCode,
                    childUserIds.Select(t => t as long?).ToList(), siteSettingId);
                if (foundUserId <= 0)
                    throw;
            }

            if (foundUserId <= 0)
                BException.GenerateNewException(BMessages.Can_Not_Create_User);

            var newItem = new InsuranceContractUser()
            {
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId.UserId,
                FamilyRelation = input.familyRelation.Value,
                InsuranceContractId = input.insuranceContractId.Value,
                ParentId = parentId,
                SiteSettingId = siteSettingId.Value,
                Status = status,
                UserId = foundUserId
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.nationalcodeImage != null && input.nationalcodeImage.Length > 0)
            {
                newItem.KartMeliFileUrl = uploadedFileManager.UploadNewFile(FileType.ContractUserDocuemnt, input.nationalcodeImage, loginUserId?.UserId, null, newItem.Id, ".jpg,.png,jpeg", true);
            }
            if (input.shenasnamePage1Image != null && input.shenasnamePage1Image.Length > 0)
            {
                newItem.ShenasnamePage1FileUrl = uploadedFileManager.UploadNewFile(FileType.ContractUserDocuemnt, input.shenasnamePage1Image, loginUserId?.UserId, null, newItem.Id, ".jpg,.png,jpeg", true);
            }
            if (input.shenasnamePage2Image != null && input.shenasnamePage2Image.Length > 0)
            {
                newItem.ShenasnamePage2FileUrl = uploadedFileManager.UploadNewFile(FileType.ContractUserDocuemnt, input.shenasnamePage2Image, loginUserId?.UserId, null, newItem.Id, ".jpg,.png,jpeg", true);
            }
            if (input.bimeImage != null && input.bimeImage.Length > 0)
            {
                newItem.BimeFileUrl = uploadedFileManager.UploadNewFile(FileType.ContractUserDocuemnt, input.bimeImage, loginUserId?.UserId, null, newItem.Id, ".jpg,.png,jpeg", true);
            }

            db.SaveChanges();


            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private long? getParentId(string mainPersonNationalCode, string eCode, int? siteSettingId, List<long> childUserIds, int? insuranceContractId)
        {
            return db.InsuranceContractUsers
                .Where(t => t.InsuranceContractId == insuranceContractId && t.SiteSettingId == siteSettingId &&
                            (childUserIds == null || childUserIds.Contains(t.CreateUserId)) && t.UserId > 0 &&
                            (t.User.Nationalcode == mainPersonNationalCode || t.User.InsuranceECode == eCode))
                .Select(t => t.Id).FirstOrDefault();
        }

        private void CreateValidation(
                CreateUpdateInsuranceContractUserVM input, InsuranceContractUserStatus status,
                int? siteSettingId, long? loginUserId, List<long> childUserIds
            )
        {
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.insuranceContractId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Contract);
            if (string.IsNullOrEmpty(input.mobile))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile);
            if (string.IsNullOrEmpty(input.email))
                throw BException.GenerateNewException(BMessages.Please_Enter_Email);
            if (!input.mobile.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (string.IsNullOrEmpty(input.nationalCode))
                throw BException.GenerateNewException(BMessages.Please_Enter_NationalCode);
            if (!input.nationalCode.IsCodeMeli())
                throw BException.GenerateNewException(BMessages.National_Is_Not_Valid);
            if (!InsuranceContractManager.Exist(input.insuranceContractId.ToIntReturnZiro(), siteSettingId, childUserIds))
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
            if (!string.IsNullOrEmpty(input.mainPersonNationalCode) && !existByNationalCode(input.id, siteSettingId, childUserIds, input.mainPersonNationalCode, input.insuranceContractId))
                throw BException.GenerateNewException(BMessages.National_Is_Not_Valid);
            if (!string.IsNullOrEmpty(input.mainPersonECode) && !existByECode(input.id, siteSettingId, input.mainPersonECode, input.insuranceContractId, childUserIds))
                throw BException.GenerateNewException(BMessages.Main_Person_Code_Is_Not_Valid);
        }

        private bool existByNationalCode(long? id, int? siteSettingId, List<long> childUserIds, string nationalCode, int? insuranceContractId)
        {
            return db.InsuranceContractUsers.Any(t => t.Id != id && t.SiteSettingId == siteSettingId && t.InsuranceContractId == insuranceContractId &&
            t.UserId > 0 && t.User.Nationalcode == nationalCode && (childUserIds == null || childUserIds.Contains(t.CreateUserId)));
        }

        private bool existByECode(long? id, int? siteSettingId, string eCode, int? insuranceContractId, List<long> childUserIds)
        {
            return db.InsuranceContractUsers
                .Any(t => t.Id != id && t.SiteSettingId == siteSettingId && t.UserId > 0 && t.User.InsuranceECode == eCode &&
                          t.InsuranceContractId == insuranceContractId && (childUserIds == null || childUserIds.Contains(t.CreateUserId)));
        }

        public ApiResult Delete(long? id, InsuranceContractUserStatus status)
        {
            var loginUserId = UserManager.GetLoginUser();
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUserIds = UserManager.GetChildsUserId(loginUserId.UserId.ToLongReturnZiro());
            var foundItem = db.InsuranceContractUsers.Include(t => t.Childs).Where(t => t.Id == id && t.SiteSettingId == siteSettingId && t.Status == status && (childUserIds == null || childUserIds.Contains(t.CreateUserId))).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            UserManager.DeleteFlag(foundItem.UserId, siteSettingId, childUserIds);

            if (foundItem.Childs != null)
                foreach (var item in foundItem.Childs)
                    db.Entry(item).State = EntityState.Deleted;
            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateInsuranceContractUserVM GetById(long? id, InsuranceContractUserStatus status)
        {
            var loginUserId = UserManager.GetLoginUser();
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUserIds = UserManager.GetChildsUserId(loginUserId.UserId.ToLongReturnZiro());
            return
                db.InsuranceContractUsers
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId && t.Status == status && (childUserIds == null || childUserIds.Contains(t.CreateUserId)) && t.UserId > 0)
                .Select(t => new
                {
                    id = t.Id,
                    bankAcount = t.User.BankAccount,
                    bankShaba = t.User.BankShaba,
                    eCode = t.User.InsuranceECode,
                    email = t.User.Email,
                    familyRelation = t.FamilyRelation,
                    insuranceContractId = t.InsuranceContractId,
                    mainPersonECode = t.Parent != null ? t.Parent.User.InsuranceECode : "",
                    firstName = t.User.Firstname,
                    lastName = t.User.Lastname,
                    birthDate = t.User.BirthDate,
                    mainPersonNationalCode = t.Parent != null ? t.Parent.User.Nationalcode : "",
                    mobile = t.User.Mobile,
                    nationalCode = t.User.Nationalcode,
                    bimeImage_address = !string.IsNullOrEmpty(t.BimeFileUrl) ? GlobalConfig.FileAccessHandlerUrl + t.BimeFileUrl : "",
                    nationalcodeImage_address = !string.IsNullOrEmpty(t.KartMeliFileUrl) ? GlobalConfig.FileAccessHandlerUrl + t.KartMeliFileUrl : "",
                    shenasnamePage1Image_address = !string.IsNullOrEmpty(t.ShenasnamePage1FileUrl) ? GlobalConfig.FileAccessHandlerUrl + t.ShenasnamePage1FileUrl : "",
                    shenasnamePage2Image_address = !string.IsNullOrEmpty(t.ShenasnamePage2FileUrl) ? GlobalConfig.FileAccessHandlerUrl + t.ShenasnamePage2FileUrl : "",
                    isActive = t.User.IsActive
                })
                .OrderByDescending(t => t.id)
                .Take(1)
                .ToList()
                .Select(t => new CreateUpdateInsuranceContractUserVM
                {
                    id = t.id,
                    bankAcount = t.bankAcount,
                    bankShaba = t.bankShaba,
                    eCode = t.eCode,
                    email = t.email,
                    familyRelation = t.familyRelation,
                    insuranceContractId = t.insuranceContractId,
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
                    isActive = t.isActive
                })
                .FirstOrDefault();

        }

        public GridResultVM<InsuranceContractUserMainGridResultVM> GetList(InsuranceContractUserMainGrid searchInput, InsuranceContractUserStatus status)
        {
            if (searchInput == null)
                searchInput = new InsuranceContractUserMainGrid();

            var loginUserId = UserManager.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUserIds = UserManager.GetChildsUserId(loginUserId.ToLongReturnZiro());

            var qureResult = db.InsuranceContractUsers.Where(t => t.SiteSettingId == siteSettingId && (childUserIds == null || childUserIds.Contains(t.CreateUserId)) && t.Status == status);

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

            int row = searchInput.skip;

            return new GridResultVM<InsuranceContractUserMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    birthDate = t.User.BirthDate,
                    contract = t.InsuranceContract.Title,
                    createByUser = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    eCode = t.User.InsuranceECode,
                    familyRelation = t.FamilyRelation,
                    fistname = t.User.Firstname,
                    id = t.Id,
                    isActive = t.User.IsActive,
                    lastname = t.User.Lastname,
                    mainECode = t.Parent != null ? t.Parent.User.InsuranceECode : "",
                    mainPersonNationalcode = t.Parent != null ? t.Parent.User.Nationalcode : "",
                    nationalcode = t.User.Nationalcode
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
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateInsuranceContractUserVM input, InsuranceContractUserStatus status)
        {
            var loginUserId = UserManager.GetLoginUser();
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUserIds = UserManager.GetChildsUserId(loginUserId.UserId.ToLongReturnZiro());
            long? parentId = null;

            CreateValidation(input, status, siteSettingId, loginUserId?.UserId, childUserIds);

            var foundItem = db.InsuranceContractUsers.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId && t.Status == status && (childUserIds == null || childUserIds.Contains(t.CreateUserId))).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (input.familyRelation != InsuranceContractUserFamilyRelation.Self)
            {
                parentId = getParentId(input.mainPersonNationalCode, input.mainPersonECode, siteSettingId, childUserIds, input.insuranceContractId);
                if (parentId.ToLongReturnZiro() <= 0)
                    throw BException.GenerateNewException(BMessages.Invalid_NationalCode_Or_InsuranceECode);
            }

            var foundUser = UserManager.GetByIdForUser(foundItem.UserId, loginUserId, siteSettingId);
            if (foundUser == null)
                throw BException.GenerateNewException(BMessages.User_Not_Found);

            foundUser.firstname = input.firstName;
            foundUser.lastname = input.lastName;
            foundUser.nationalCode = input.nationalCode;
            foundUser.birthDate = input.birthDate;
            foundUser.bankAccount = input.bankAcount;
            foundUser.bankShaba = input.bankShaba;
            foundUser.insuranceECode = input.eCode;
            foundUser.mobile = input.mobile;
            foundUser.email = input.email;
            foundUser.isActive = input.isActive.ToBooleanReturnFalse();

            if (UserManager.UpdateForUser(foundUser, loginUserId?.UserId, loginUserId, siteSettingId).isSuccess == false)
                throw BException.GenerateNewException(BMessages.UnknownError);


            foundItem.ParentId = parentId;
            foundItem.UpdateDate = DateTime.Now;
            foundItem.UpdateUserId = loginUserId.UserId;
            foundItem.InsuranceContractId = input.insuranceContractId.Value;
            foundItem.FamilyRelation = input.familyRelation.Value;

            if (input.nationalcodeImage != null && input.nationalcodeImage.Length > 0)
            {
                foundItem.KartMeliFileUrl = uploadedFileManager.UploadNewFile(FileType.ContractUserDocuemnt, input.nationalcodeImage, loginUserId?.UserId, null, foundItem.Id, ".jpg,.png,jpeg", true);
            }
            if (input.shenasnamePage1Image != null && input.shenasnamePage1Image.Length > 0)
            {
                foundItem.ShenasnamePage1FileUrl = uploadedFileManager.UploadNewFile(FileType.ContractUserDocuemnt, input.shenasnamePage1Image, loginUserId?.UserId, null, foundItem.Id, ".jpg,.png,jpeg", true);
            }
            if (input.shenasnamePage2Image != null && input.shenasnamePage2Image.Length > 0)
            {
                foundItem.ShenasnamePage2FileUrl = uploadedFileManager.UploadNewFile(FileType.ContractUserDocuemnt, input.shenasnamePage2Image, loginUserId?.UserId, null, foundItem.Id, ".jpg,.png,jpeg", true);
            }
            if (input.bimeImage != null && input.bimeImage.Length > 0)
            {
                foundItem.BimeFileUrl = uploadedFileManager.UploadNewFile(FileType.ContractUserDocuemnt, input.bimeImage, loginUserId?.UserId, null, foundItem.Id, ".jpg,.png,jpeg", true);
            }

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult ChangeStatus(long? id, InsuranceContractUserStatus fromStatus, InsuranceContractUserStatus toStatus)
        {
            var loginUserId = UserManager.GetLoginUser();
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUserIds = UserManager.GetChildsUserId(loginUserId.UserId.ToLongReturnZiro());
            var foundItem = db.InsuranceContractUsers.Include(t => t.Childs).Where(t => t.Id == id && t.SiteSettingId == siteSettingId && t.Status == fromStatus && (childUserIds == null || childUserIds.Contains(t.CreateUserId))).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.Childs != null)
                foreach (var item in foundItem.Childs)
                    item.Status = toStatus;
            foundItem.Status = toStatus;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult CreateFromExcel(GlobalExcelFile input, InsuranceContractUserStatus status)
        {
            string resultText = "";

            var excelFile = input?.excelFile;

            if (excelFile == null || excelFile.Length == 0)
                return ApiResult.GenerateNewResult(false, BMessages.Please_Select_File);

            List<CreateUpdateInsuranceContractUserVM> models = ExportToExcel.ConvertToModel<CreateUpdateInsuranceContractUserVM>(input?.excelFile);
            if (models != null && models.Count > 0)
            {
                for (var i = 0; i < models.Count; i++)
                {
                    var model = models[i];
                    try
                    {
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
