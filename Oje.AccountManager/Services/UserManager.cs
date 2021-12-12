using Oje.AccountManager.Interfaces;
using Oje.AccountManager.Models.DB;
using Oje.AccountManager.Models.SP;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.AccountManager.Services.EContext;
using Oje.AccountManager.Models.View;
using Oje.Infrastructure.Models.PageForms;

namespace Oje.AccountManager.Services
{
    public class UserManager : IUserManager
    {
        readonly AccountDBContext db = null;
        readonly IHttpContextAccessor httpContextAccessor = null;
        readonly IUploadedFileManager uploadedFileManager = null;
        readonly IRoleManager RoleManager = null;
        readonly ISiteSettingManager SiteSettingManager = null;
        public UserManager(
                AccountDBContext db,
                IHttpContextAccessor httpContextAccessor,
                IUploadedFileManager uploadedFileManager,
                IRoleManager RoleManager,
                ISiteSettingManager SiteSettingManager
            )
        {
            this.db = db;
            this.httpContextAccessor = httpContextAccessor;
            this.uploadedFileManager = uploadedFileManager;
            this.RoleManager = RoleManager;
            this.SiteSettingManager = SiteSettingManager;
        }

        //private void LoginValidation(LoginVM input)
        //{
        //    if (input == null)
        //        throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
        //    if (string.IsNullOrEmpty(input.username))
        //        throw BException.GenerateNewException(BMessages.Please_Enter_Username, ApiResultErrorCode.ValidationError);
        //    if (string.IsNullOrEmpty(input.password))
        //        throw BException.GenerateNewException(BMessages.Please_Enter_Password, ApiResultErrorCode.ValidationError);
        //    if (Captcha.ValidateCaptchaCode(input.sCode, input.sCodeGuid) == false)
        //        throw BException.GenerateNewException(BMessages.Invalid_Captcha, ApiResultErrorCode.InvalidCaptcha);
        //}

        public ApiResult Login(LoginVM input, int? siteSettingId)
        {
            //LoginValidation(input);


            var foundUser = db.Users.Where(t => t.Username.ToLower() == input.username.ToLower()).FirstOrDefault();

            if (foundUser != null && foundUser.SiteSettingId != null)
                MyValidations.SiteSettingValidation(foundUser.SiteSettingId, siteSettingId);

            if (foundUser == null && !db.Users.Any())
                return CreateAdminUser(input);
            else if (foundUser != null && (foundUser.IsActive == false || foundUser.IsDelete == true))
                throw BException.GenerateNewException(BMessages.Inactive_User, ApiResultErrorCode.InActiveUser);
            else if (foundUser != null && foundUser.Password == input.password.Encrypt())
            {
                setCookieForThisUser(foundUser, input);
                return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
            }

            throw BException.GenerateNewException(BMessages.Invalid_User_Or_Password, ApiResultErrorCode.InvalidUserOrPassword);
        }

        private ApiResult CreateAdminUser(LoginVM input)
        {
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    User newUser = new User();
                    newUser.Firstname = input.username;
                    newUser.Lastname = input.username;
                    newUser.Username = input.username;
                    newUser.Password = input.password.Encrypt();
                    newUser.IsActive = true;
                    newUser.CreateDate = DateTime.Now;

                    db.Entry(newUser).State = EntityState.Added;
                    db.SaveChanges();

                    Role foundRole = db.Roles.Where(t => t.Name == "SysAdmin").FirstOrDefault();
                    if (foundRole == null)
                    {
                        foundRole = new Role();
                        foundRole.Name = "SysAdmin";
                        foundRole.Title = "کل سایت";
                        foundRole.Value = long.MaxValue;
                        db.Entry(foundRole).State = EntityState.Added;
                        db.SaveChanges();
                    }

                    UserRole newUserRole = new UserRole();
                    newUserRole.RoleId = foundRole.Id;
                    newUserRole.UserId = newUser.Id;

                    db.Entry(newUserRole).State = EntityState.Added;
                    db.SaveChanges();

                    var allMSectionIds = db.Actions.Select(t => t.Id).ToList();
                    foreach (var sId in allMSectionIds)
                    {
                        db.Entry(new RoleAction() { RoleId = foundRole.Id, ActionId = sId }).State = EntityState.Added;
                    }
                    db.SaveChanges();

                    tr.Commit();

                    setCookieForThisUser(newUser, input);
                    return new ApiResult() { isSuccess = true };
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        private void setCookieForThisUser(User newUser, LoginVM input)
        {
            string cookiValue = newUser.Id + "," + newUser.Username + "," + newUser.Firstname + " " + newUser.Lastname + "," + httpContextAccessor.HttpContext.Connection.RemoteIpAddress + "," + newUser.SiteSettingId;
            var cOption = new CookieOptions() { HttpOnly = true };
            if (input.rememberMe == true)
                cOption.Expires = DateTime.Now.AddDays(1);
            httpContextAccessor.HttpContext.Response.Cookies.Append("login", cookiValue.Encrypt2(), cOption);
        }

        private void CreateValidation(CreateUpdateUserVM input, long? loginUserId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.email))
                throw BException.GenerateNewException(BMessages.Please_Enter_Email, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.firstname))
                throw BException.GenerateNewException(BMessages.Please_Enter_Firstname, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.lastname))
                throw BException.GenerateNewException(BMessages.Please_Enter_Lastname, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.mobile))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.username))
                throw BException.GenerateNewException(BMessages.Please_Enter_Username, ApiResultErrorCode.ValidationError);
            if (input.username.Length > 50)
                throw BException.GenerateNewException(BMessages.Username_Can_Not_Be_More_Then_50_chars, ApiResultErrorCode.ValidationError);
            if (input.roleIds == null || input.roleIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_One_Or_More_Role, ApiResultErrorCode.ValidationError);
            if (loginUserId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First, ApiResultErrorCode.NeedLoginFist);
            if (!string.IsNullOrEmpty(input.mobile) && input.mobile.IsMobile() == false)
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.nationalCode) && input.nationalCode.IsCodeMeli() == false)
                throw BException.GenerateNewException(BMessages.National_Is_Not_Valid, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.postalCode) && input.postalCode.Length != 10)
                throw BException.GenerateNewException(BMessages.PostalCode_Is_Not_Valid, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.address) && input.address.Length > 1000)
                throw BException.GenerateNewException(BMessages.Address_Length_Is_Not_Valid, ApiResultErrorCode.ValidationError);

            if (db.Users.Any(t => t.Username == input.username && t.Id != input.id && t.SiteSettingId == input.sitesettingId))
                throw BException.GenerateNewException(BMessages.Dublicate_Username, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.mobile) && db.Users.Any(t => t.Id != input.id && t.Mobile == input.mobile && t.SiteSettingId == input.sitesettingId))
                throw BException.GenerateNewException(BMessages.Dublicate_Mobile, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.email) && db.Users.Any(t => t.Id != input.id && t.Email == input.email && t.SiteSettingId == input.sitesettingId))
                throw BException.GenerateNewException(BMessages.Dublicate_Email, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.insuranceECode) && db.Users.Any(t => t.Id != input.id && t.InsuranceECode == input.insuranceECode && t.SiteSettingId == input.sitesettingId))
                throw BException.GenerateNewException(BMessages.Dublicate_Electronic_Code, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.bankAccount) && input.bankAccount.Length > 20)
                throw BException.GenerateNewException(BMessages.BankAcount_Can_Not_Be_More_Then_20);
            if (!string.IsNullOrEmpty(input.bankShaba) && input.bankShaba.Length > 40)
                throw BException.GenerateNewException(BMessages.BankShaba_Can_Not_Be_More_Then_40);
        }

        private void passwordValidation2(CreateUpdateUserVM input)
        {
            if (!string.IsNullOrEmpty(input.password) && input.password != input.confirmPassword)
                throw BException.GenerateNewException(BMessages.The_Password_Is_Not_Look_Like_Confirm_Password, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.password) && input.password.Length > 50)
                throw BException.GenerateNewException(BMessages.Password_Can_Not_Be_More_Then_50_Chars, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.password) && input.password.Length < 6)
                throw BException.GenerateNewException(BMessages.Password_Can_Not_Be_Less_Then_6_Chars, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.password) && input.password.IsWeekPassword() == true)
                throw BException.GenerateNewException(BMessages.The_Password_Is_Week, ApiResultErrorCode.ValidationError);
        }

        private void PasswordValidation(CreateUpdateUserVM input)
        {
            if (string.IsNullOrEmpty(input.password))
                throw BException.GenerateNewException(BMessages.Please_Enter_Password, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.confirmPassword))
                throw BException.GenerateNewException(BMessages.Please_Enter_Confirm_Password, ApiResultErrorCode.ValidationError);
            passwordValidation2(input);
        }

        public ApiResult Create(CreateUpdateUserVM input, long? loginUserId)
        {
            CreateValidation(input, loginUserId);
            PasswordValidation(input);

            User newUser = new User();
            newUser.Email = input.email;
            newUser.Firstname = input.firstname;
            newUser.Lastname = input.lastname;
            newUser.Username = input.username;
            newUser.IsActive = input.isActive.ToBooleanReturnFalse();
            newUser.Mobile = input.mobile;
            newUser.Tell = input.tell;
            newUser.ParentId = loginUserId.Value;
            newUser.Password = input.password.Encrypt();
            newUser.CreateDate = DateTime.Now;
            newUser.CreateByUserId = loginUserId.Value;
            newUser.Nationalcode = input.nationalCode;
            newUser.IsDelete = input.isDelete.ToBooleanReturnFalse();
            newUser.IsMobileConfirm = input.isMobileConfirm.ToBooleanReturnFalse();
            newUser.IsEmailConfirm = input.isEmailConfirm.ToBooleanReturnFalse();
            newUser.PostalCode = input.postalCode;
            newUser.Address = input.address;
            newUser.AgentCode = input.agentCode;
            newUser.CompanyTitle = input.companyTitle;
            newUser.BankAccount = input.bankAccount;
            newUser.BankShaba = input.bankShaba;
            newUser.BirthDate = input.birthDate.ConvertPersianNumberToEnglishNumber().ToEnDate();
            newUser.InsuranceECode = input.insuranceECode;
            newUser.SiteSettingId = input.sitesettingId;
            newUser.ProvinceId = input.provinceId;
            newUser.CityId = input.cityId;

            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    db.Entry(newUser).State = EntityState.Added;
                    db.SaveChanges();

                    if (input.userPic != null && input.userPic.Length > 0)
                        newUser.UserPic = uploadedFileManager.UploadNewFile(FileType.UserProfilePic, input.userPic, loginUserId, null, newUser.Id, ".png,.jpg,.jpeg", true);

                    foreach (var roleId in input.roleIds)
                        db.Entry(new UserRole() { RoleId = roleId, UserId = newUser.Id }).State = EntityState.Added;
                    foreach (var cid in input.cIds)
                        db.Entry(new UserCompany() { CompanyId = cid, UserId = newUser.Id }).State = EntityState.Added;

                    db.SaveChanges();
                    tr.Commit();

                    return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        public ApiResult Delete(long? id)
        {
            var foundItem = db.Users.Where(t => t.Id == id).Include(t => t.UserRoles).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var ur in foundItem.UserRoles)
                        db.Entry(ur).State = EntityState.Deleted;

                    db.Entry(foundItem).State = EntityState.Deleted;
                    db.SaveChanges();
                    tr.Commit();

                    return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        public CreateUpdateUserVM GetById(long? id)
        {
            return db.Users
                .Where(t => t.Id == id)
                .OrderByDescending(t => t.Id)

                .Select(t => new
                {
                    id = t.Id,
                    firstname = t.Firstname,
                    lastname = t.Lastname,
                    username = t.Username,
                    email = t.Email,
                    mobile = t.Mobile,
                    tell = t.Tell,
                    isActive = t.IsActive,
                    roleIds = t.UserRoles.Select(tt => tt.RoleId).ToList(),
                    isDelete = t.IsDelete,
                    nationalCode = t.Nationalcode,
                    isMobileConfirm = t.IsMobileConfirm,
                    isEmailConfirm = t.IsEmailConfirm,
                    postalCode = t.PostalCode,
                    address = t.Address,
                    sitesettingId = t.SiteSettingId,
                    userPic_address = GlobalConfig.FileAccessHandlerUrl + t.UserPic,
                    companyTitle = t.CompanyTitle,
                    agentCode = t.AgentCode,
                    cIds = t.UserCompanies.Select(tt => tt.CompanyId).ToList(),
                    bankAccount = t.BankAccount,
                    bankShaba = t.BankShaba,
                    birthDate = t.BirthDate,
                    insuranceECode = t.InsuranceECode,
                    provinceId = t.ProvinceId,
                    cityId = t.CityId
                })
                .Take(1)
                .Select(t => new CreateUpdateUserVM
                {
                    id = t.id,
                    firstname = t.firstname,
                    lastname = t.lastname,
                    username = t.username,
                    email = t.email,
                    mobile = t.mobile,
                    tell = t.tell,
                    isActive = t.isActive,
                    roleIds = t.roleIds,
                    isDelete = t.isDelete,
                    nationalCode = t.nationalCode,
                    isMobileConfirm = t.isMobileConfirm,
                    isEmailConfirm = t.isEmailConfirm,
                    postalCode = t.postalCode,
                    address = t.address,
                    sitesettingId = t.sitesettingId,
                    userPic_address = t.userPic_address,
                    companyTitle = t.companyTitle,
                    agentCode = t.agentCode,
                    cIds = t.cIds,
                    bankAccount = t.bankAccount,
                    bankShaba = t.bankShaba,
                    birthDate = t.birthDate.ToFaDate(),
                    insuranceECode = t.insuranceECode,
                    provinceId = t.provinceId,
                    cityId = t.cityId
                })
                .FirstOrDefault();
        }

        public ApiResult Update(CreateUpdateUserVM input, long? loginUserId)
        {
            CreateValidation(input, loginUserId);
            passwordValidation2(input);

            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    var foundItem = db.Users.Where(t => t.Id == input.id).Include(t => t.UserCompanies).Include(t => t.UserRoles).FirstOrDefault();
                    if (foundItem == null)
                        throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

                    foundItem.Username = input.username;
                    foundItem.Firstname = input.firstname;
                    foundItem.Lastname = input.lastname;
                    foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
                    foundItem.Tell = input.tell;
                    foundItem.Mobile = input.mobile;
                    foundItem.Email = input.email;
                    foundItem.UpdateDate = DateTime.Now;
                    foundItem.UpdateByUserId = loginUserId.Value;
                    foundItem.Nationalcode = input.nationalCode;
                    foundItem.IsDelete = input.isDelete.ToBooleanReturnFalse();
                    foundItem.IsMobileConfirm = input.isMobileConfirm.ToBooleanReturnFalse();
                    foundItem.IsEmailConfirm = input.isEmailConfirm.ToBooleanReturnFalse();
                    foundItem.PostalCode = input.postalCode;
                    foundItem.Address = input.address;
                    foundItem.AgentCode = input.agentCode;
                    foundItem.CompanyTitle = input.companyTitle;
                    foundItem.SiteSettingId = input.sitesettingId;
                    foundItem.BankAccount = input.bankAccount;
                    foundItem.BankShaba = input.bankShaba;
                    foundItem.BirthDate = input.birthDate.ConvertPersianNumberToEnglishNumber().ToEnDate();
                    foundItem.InsuranceECode = input.insuranceECode;
                    foundItem.ProvinceId = input.provinceId;
                    foundItem.CityId = input.cityId;

                    if (!string.IsNullOrEmpty(input.password))
                        foundItem.Password = input.password.Encrypt();
                    if (input.userPic != null && input.userPic.Length > 0)
                        foundItem.UserPic = uploadedFileManager.UploadNewFile(FileType.UserProfilePic, input.userPic, loginUserId, null, foundItem.Id, ".png,.jpg,.jpeg", true);

                    foreach (var ur in foundItem.UserRoles)
                        db.Entry(ur).State = EntityState.Deleted;
                    foreach (var uc in foundItem.UserCompanies)
                        db.Entry(uc).State = EntityState.Deleted;

                    foreach (var roleId in input.roleIds)
                        db.Entry(new UserRole() { RoleId = roleId, UserId = foundItem.Id }).State = EntityState.Added;
                    foreach (var cid in input.cIds)
                        db.Entry(new UserCompany() { CompanyId = cid, UserId = foundItem.Id }).State = EntityState.Added;

                    db.SaveChanges();
                    tr.Commit();

                    return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        public GridResultVM<AdminUserGridResult> GetList(UserManagerMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new UserManagerMainGrid();

            var qureResult = db.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.fistname))
                qureResult = qureResult.Where(t => t.Firstname.Contains(searchInput.fistname));
            if (!string.IsNullOrEmpty(searchInput.lastname))
                qureResult = qureResult.Where(t => t.Lastname.Contains(searchInput.lastname));
            if (!string.IsNullOrEmpty(searchInput.username))
                qureResult = qureResult.Where(t => t.Username.Contains(searchInput.username));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive.Value);
            if (!string.IsNullOrEmpty(searchInput.mobile))
                qureResult = qureResult.Where(t => t.Mobile.Contains(searchInput.mobile));
            if (searchInput.roleIds.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.UserRoles.Any(tt => tt.RoleId == searchInput.roleIds));

            int row = searchInput.skip;

            return new GridResultVM<AdminUserGridResult>
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take).Select(t => new
                {
                    id = t.Id,
                    username = t.Username,
                    fistname = t.Firstname,
                    lastname = t.Lastname,
                    mobile = t.Mobile,
                    isActive = t.IsActive,
                    roleIds = t.UserRoles.Select(tt => tt.Role.Title).ToList()
                }).ToList()
                .Select(t => new AdminUserGridResult
                {
                    row = ++row,
                    id = t.id,
                    username = t.username,
                    fistname = t.fistname,
                    lastname = t.lastname,
                    mobile = t.mobile,
                    isActive = t.isActive == true ? IsActive.Active.GetAttribute<DisplayAttribute>()?.Name : IsActive.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    roleIds = string.Join(',', t.roleIds)
                })
                .ToList()
            };
        }

        public List<Models.DB.Action> GetUserSections(long userId)
        {
            return db.Users.Where(t => t.Id == userId)
                .SelectMany(t => t.UserRoles)
                .Select(t => t.Role)
                .SelectMany(t => t.RoleActions)
                .Select(t => t.Action)
                .ToList();
        }

        public LoginUserVM GetLoginUser()
        {
            try
            {
                return httpContextAccessor.HttpContext.GetLoginUserId();
            }
            catch
            {
                return null;
            }
        }

        public void Logout()
        {
            string cookiValue = "";
            var cOption = new CookieOptions() { HttpOnly = true };
            cOption.Expires = DateTime.Now.AddDays(-1);
            httpContextAccessor.HttpContext.Response.Cookies.Append("login", cookiValue, cOption);
        }

        public List<long> GetUserIdByRoleIds(List<int> roleIds)
        {
            return db.Users.Where(t => t.UserRoles.Any(tt => roleIds.Contains(tt.RoleId))).Select(t => t.Id).ToList();
        }

        public List<long> GetChildsUserId(long userId)
        {
            if (db.Users.Where(t => t.Id == userId).SelectMany(t => t.UserRoles).Any(t => t.Role.DisabledOnlyMyStuff == true))
                return null;
            var childUserIds = db.ChildUserIds.FromSqlRaw<ChildUserId>("dbo.GetChildUserIds @userId = {0}", userId).ToList();

            if (childUserIds != null && childUserIds.Count > 0)
                return childUserIds.Select(t => t.Id).ToList();

            return new List<long>() { userId };
        }

        public ApiResult CreateForUser(CreateUpdateUserForUserVM input, long? loginUserId, LoginUserVM loginUserVM, int? siteSettingId)
        {
            creatCreateForUserValidation(input, loginUserId, loginUserVM, siteSettingId);
            createForUserPasswordValidation(input);

            User newUser = new User();
            newUser.Email = input.email;
            newUser.Firstname = input.firstname;
            newUser.Lastname = input.lastname;
            newUser.Username = input.username;
            newUser.IsActive = input.isActive.ToBooleanReturnFalse();
            newUser.Mobile = input.mobile;
            newUser.Tell = input.tell;
            newUser.ParentId = loginUserId.Value;
            newUser.Password = input.password.Encrypt();
            newUser.CreateDate = DateTime.Now;
            newUser.CreateByUserId = loginUserId.Value;
            newUser.Nationalcode = input.nationalCode;
            newUser.IsDelete = input.isDelete.ToBooleanReturnFalse();
            newUser.IsMobileConfirm = input.isMobileConfirm.ToBooleanReturnFalse();
            newUser.IsEmailConfirm = input.isEmailConfirm.ToBooleanReturnFalse();
            newUser.PostalCode = input.postalCode;
            newUser.Address = input.address;
            newUser.AgentCode = input.agentCode;
            newUser.CompanyTitle = input.companyTitle;
            newUser.SiteSettingId = siteSettingId.Value;
            newUser.BankAccount = input.bankAccount;
            newUser.BankShaba = input.bankShaba;
            newUser.BirthDate = input.birthDate.ConvertPersianNumberToEnglishNumber().ToEnDate();
            newUser.InsuranceECode = input.insuranceECode;
            newUser.ProvinceId = input.provinceId;
            newUser.CityId = input.cityId;

            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    db.Entry(newUser).State = EntityState.Added;
                    db.SaveChanges();

                    if (input.userPic != null && input.userPic.Length > 0)
                        newUser.UserPic = uploadedFileManager.UploadNewFile(FileType.UserProfilePic, input.userPic, loginUserId, null, newUser.Id, ".png,.jpg,.jpeg", true);

                    foreach (var roleId in input.roleIds)
                        db.Entry(new UserRole() { RoleId = roleId, UserId = newUser.Id }).State = EntityState.Added;
                    foreach (var cid in input.cIds)
                        db.Entry(new UserCompany() { CompanyId = cid, UserId = newUser.Id }).State = EntityState.Added;

                    db.SaveChanges();
                    tr.Commit();

                    return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name, data = newUser.Id };
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

        }

        private void createForUserPasswordValidation(CreateUpdateUserForUserVM input)
        {
            if (string.IsNullOrEmpty(input.password))
                throw BException.GenerateNewException(BMessages.Please_Enter_Password, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.confirmPassword))
                throw BException.GenerateNewException(BMessages.Please_Enter_Confirm_Password, ApiResultErrorCode.ValidationError);
            createForUserPasswordValidation2(input);
        }

        private void createForUserPasswordValidation2(CreateUpdateUserForUserVM input)
        {
            if (!string.IsNullOrEmpty(input.password) && input.password != input.confirmPassword)
                throw BException.GenerateNewException(BMessages.The_Password_Is_Not_Look_Like_Confirm_Password, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.password) && input.password.Length > 50)
                throw BException.GenerateNewException(BMessages.Password_Can_Not_Be_More_Then_50_Chars, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.password) && input.password.Length < 6)
                throw BException.GenerateNewException(BMessages.Password_Can_Not_Be_Less_Then_6_Chars, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.password) && input.password.IsWeekPassword() == true)
                throw BException.GenerateNewException(BMessages.The_Password_Is_Week, ApiResultErrorCode.ValidationError);
        }

        private void creatCreateForUserValidation(CreateUpdateUserForUserVM input, long? loginUserId, LoginUserVM loginUserVM, int? siteSettingId)
        {
            MyValidations.SiteSettingValidation(loginUserVM?.siteSettingId, siteSettingId);

            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.email))
                throw BException.GenerateNewException(BMessages.Please_Enter_Email, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.firstname))
                throw BException.GenerateNewException(BMessages.Please_Enter_Firstname, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.lastname))
                throw BException.GenerateNewException(BMessages.Please_Enter_Lastname, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.mobile))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.username))
                throw BException.GenerateNewException(BMessages.Please_Enter_Username, ApiResultErrorCode.ValidationError);
            if (input.username.Length > 50)
                throw BException.GenerateNewException(BMessages.Username_Can_Not_Be_More_Then_50_chars, ApiResultErrorCode.ValidationError);
            if (input.roleIds == null || input.roleIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_One_Or_More_Role, ApiResultErrorCode.ValidationError);
            if (db.Users.Any(t => t.Username == input.username && t.Id != input.id && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Dublicate_Username, ApiResultErrorCode.ValidationError);
            if (loginUserId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First, ApiResultErrorCode.NeedLoginFist);
            if (!string.IsNullOrEmpty(input.mobile) && input.mobile.IsMobile() == false)
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.nationalCode) && input.nationalCode.IsCodeMeli() == false)
                throw BException.GenerateNewException(BMessages.National_Is_Not_Valid, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.postalCode) && input.postalCode.Length != 10)
                throw BException.GenerateNewException(BMessages.PostalCode_Is_Not_Valid, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.address) && input.address.Length > 1000)
                throw BException.GenerateNewException(BMessages.Address_Length_Is_Not_Valid, ApiResultErrorCode.ValidationError);

            if (!string.IsNullOrEmpty(input.mobile) && db.Users.Any(t => t.Id != input.id && t.Mobile == input.mobile && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Dublicate_Mobile, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.email) && db.Users.Any(t => t.Id != input.id && t.Email == input.email && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Dublicate_Email, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.insuranceECode) && db.Users.Any(t => t.Id != input.id && t.InsuranceECode == input.insuranceECode && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Dublicate_Electronic_Code, ApiResultErrorCode.ValidationError);
            if (!string.IsNullOrEmpty(input.nationalCode) && db.Users.Any(t => t.Nationalcode == input.nationalCode && t.Id != input.id && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Dublicate_NationalCode);

            var loginUserMaxRoleValue = RoleManager.GetRoleValueByUserId(loginUserVM.UserId, siteSettingId);
            foreach (var rid in input.roleIds)
            {
                var roleValue = RoleManager.GetRoleValueByRoleId(rid, siteSettingId);
                if (roleValue >= loginUserMaxRoleValue)
                    throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);
            }
        }

        public ApiResult DeleteForUser(long? id, LoginUserVM loginUserVM, int? siteSettingId)
        {
            MyValidations.SiteSettingValidation(loginUserVM?.siteSettingId, siteSettingId);
            var childIds = GetChildsUserId(loginUserVM.UserId);
            var foundItem = db.Users.Where(t => t.Id == id && (childIds == null || childIds.Contains(t.Id)) && t.SiteSettingId == siteSettingId).Include(t => t.UserRoles).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var ur in foundItem.UserRoles)
                        db.Entry(ur).State = EntityState.Deleted;

                    db.Entry(foundItem).State = EntityState.Deleted;
                    db.SaveChanges();
                    tr.Commit();

                    return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        public CreateUpdateUserForUserVM GetByIdForUser(long? id, LoginUserVM loginUserVM, int? siteSettingId)
        {
            MyValidations.SiteSettingValidation(loginUserVM?.siteSettingId, siteSettingId);
            var childIds = GetChildsUserId(loginUserVM.UserId);
            return db.Users.Where(t => t.Id == id && t.SiteSettingId == siteSettingId && (childIds == null || childIds.Contains(t.Id)))
                .OrderByDescending(t => t.Id)
                .Select(t => new
                {
                    id = t.Id,
                    firstname = t.Firstname,
                    lastname = t.Lastname,
                    username = t.Username,
                    email = t.Email,
                    mobile = t.Mobile,
                    tell = t.Tell,
                    isActive = t.IsActive,
                    roleIds = t.UserRoles.Select(tt => tt.RoleId).ToList(),
                    isDelete = t.IsDelete,
                    nationalCode = t.Nationalcode,
                    isMobileConfirm = t.IsMobileConfirm,
                    isEmailConfirm = t.IsEmailConfirm,
                    postalCode = t.PostalCode,
                    address = t.Address,
                    userPic_address = !string.IsNullOrEmpty(t.UserPic) ? GlobalConfig.FileAccessHandlerUrl + t.UserPic : "",
                    companyTitle = t.CompanyTitle,
                    agentCode = t.AgentCode,
                    cIds = t.UserCompanies.Select(tt => tt.CompanyId).ToList(),
                    bankAccount = t.BankAccount,
                    bankShaba = t.BankShaba,
                    birthDate = t.BirthDate,
                    insuranceECode = t.InsuranceECode,
                    provinceId = t.ProvinceId,
                    cityId = t.CityId

                })
                .Take(1)
                .ToList()
                .Select(t => new CreateUpdateUserForUserVM
                {
                    id = t.id,
                    firstname = t.firstname,
                    lastname = t.lastname,
                    username = t.username,
                    email = t.email,
                    mobile = t.mobile,
                    tell = t.tell,
                    isActive = t.isActive,
                    roleIds = t.roleIds,
                    isDelete = t.isDelete,
                    nationalCode = t.nationalCode,
                    isMobileConfirm = t.isMobileConfirm,
                    isEmailConfirm = t.isEmailConfirm,
                    postalCode = t.postalCode,
                    address = t.address,
                    userPic_address = t.userPic_address,
                    companyTitle = t.companyTitle,
                    agentCode = t.agentCode,
                    cIds = t.cIds,
                    bankAccount = t.bankAccount,
                    bankShaba = t.bankShaba,
                    birthDate = t.birthDate.ToFaDate(),
                    insuranceECode = t.insuranceECode,
                    provinceId = t.provinceId,
                    cityId = t.cityId
                })
                .FirstOrDefault();
        }

        public ApiResult UpdateForUser(CreateUpdateUserForUserVM input, long? loginUserId, LoginUserVM loginUserVM, int? siteSettingId)
        {
            creatCreateForUserValidation(input, loginUserId, loginUserVM, siteSettingId);
            createForUserPasswordValidation2(input);
            var childIds = GetChildsUserId(loginUserVM.UserId);

            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    var foundItem = db.Users
                        .Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId && (childIds == null || childIds.Contains(t.Id)))
                        .Include(t => t.UserRoles)
                        .Include(t => t.UserCompanies)
                        .FirstOrDefault();
                    if (foundItem == null)
                        throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

                    foundItem.Username = input.username;
                    foundItem.Firstname = input.firstname;
                    foundItem.Lastname = input.lastname;
                    foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
                    foundItem.Tell = input.tell;
                    foundItem.Mobile = input.mobile;
                    foundItem.Email = input.email;
                    foundItem.UpdateDate = DateTime.Now;
                    foundItem.UpdateByUserId = loginUserId.Value;
                    foundItem.Nationalcode = input.nationalCode;
                    foundItem.IsDelete = input.isDelete.ToBooleanReturnFalse();
                    foundItem.IsMobileConfirm = input.isMobileConfirm.ToBooleanReturnFalse();
                    foundItem.IsEmailConfirm = input.isEmailConfirm.ToBooleanReturnFalse();
                    foundItem.PostalCode = input.postalCode;
                    foundItem.Address = input.address;
                    foundItem.AgentCode = input.agentCode;
                    foundItem.CompanyTitle = input.companyTitle;
                    foundItem.BankAccount = input.bankAccount;
                    foundItem.BankShaba = input.bankShaba;
                    foundItem.BirthDate = input.birthDate.ConvertPersianNumberToEnglishNumber().ToEnDate();
                    foundItem.InsuranceECode = input.insuranceECode;
                    foundItem.ProvinceId = input.provinceId;
                    foundItem.CityId = input.cityId;

                    if (!string.IsNullOrEmpty(input.password))
                        foundItem.Password = input.password.Encrypt();
                    if (input.userPic != null && input.userPic.Length > 0)
                        foundItem.UserPic = uploadedFileManager.UploadNewFile(FileType.UserProfilePic, input.userPic, loginUserId, null, foundItem.Id, ".png,.jpg,.jpeg", true);

                    foreach (var ur in foundItem.UserRoles)
                        db.Entry(ur).State = EntityState.Deleted;
                    foreach (var uc in foundItem.UserCompanies)
                        db.Entry(uc).State = EntityState.Deleted;

                    foreach (var roleId in input.roleIds)
                        db.Entry(new UserRole() { RoleId = roleId, UserId = foundItem.Id }).State = EntityState.Added;
                    foreach (var cid in input.cIds)
                        db.Entry(new UserCompany() { CompanyId = cid, UserId = foundItem.Id }).State = EntityState.Added;

                    db.SaveChanges();
                    tr.Commit();

                    return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        public GridResultVM<UserManagerForUserMainGridResultVM> GetListForUser(UserManagerForUserMainGrid searchInput, LoginUserVM loginUserVM, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new UserManagerForUserMainGrid();

            MyValidations.SiteSettingValidation(loginUserVM?.siteSettingId, siteSettingId);
            var childIds = GetChildsUserId(loginUserVM.UserId);

            var qureResult = db.Users.Where(t => t.SiteSettingId == siteSettingId && (childIds == null || childIds.Contains(t.Id)));

            if (!string.IsNullOrEmpty(searchInput.fistname))
                qureResult = qureResult.Where(t => t.Firstname.Contains(searchInput.fistname));
            if (!string.IsNullOrEmpty(searchInput.lastname))
                qureResult = qureResult.Where(t => t.Lastname.Contains(searchInput.lastname));
            if (!string.IsNullOrEmpty(searchInput.username))
                qureResult = qureResult.Where(t => t.Username.Contains(searchInput.username));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive.Value);
            if (!string.IsNullOrEmpty(searchInput.mobile))
                qureResult = qureResult.Where(t => t.Mobile.Contains(searchInput.mobile));
            if (searchInput.roleIds.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.UserRoles.Any(tt => tt.RoleId == searchInput.roleIds));

            int row = searchInput.skip;

            return new GridResultVM<UserManagerForUserMainGridResultVM>
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take).Select(t => new
                {
                    id = t.Id,
                    username = t.Username,
                    fistname = t.Firstname,
                    lastname = t.Lastname,
                    mobile = t.Mobile,
                    isActive = t.IsActive,
                    roleIds = t.UserRoles.Select(tt => tt.Role.Title).ToList()
                }).ToList()
                .Select(t => new UserManagerForUserMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    username = t.username,
                    fistname = t.fistname,
                    lastname = t.lastname,
                    mobile = t.mobile,
                    isActive = t.isActive == true ? IsActive.Active.GetAttribute<DisplayAttribute>()?.Name : IsActive.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    roleIds = string.Join(',', t.roleIds)
                })
                .ToList()
            };
        }

        public object GetUserInfoByUserId(long? userId)
        {
            return db.Users.Where(t => t.Id == userId).Select(t => new
            {
                firstname = t.Firstname,
                lastname = t.Lastname,
                username = t.Username,
                pic = GlobalConfig.FileAccessHandlerUrl + t.UserPic
            }).FirstOrDefault();
        }

        public long GetUserIdByNationalEmailMobleEcode(string nationalCode, string mobile, string email, string eCode, List<long?> childUserIds, int? siteSettingId)
        {
            return db.Users
                .Where(t =>
                    t.Nationalcode == nationalCode && t.Mobile == mobile && t.Email == email && t.InsuranceECode == eCode && t.SiteSettingId == siteSettingId &&
                    t.CreateByUserId != null && (childUserIds == null || childUserIds.Contains(t.CreateByUserId)))
                .Select(t => t.Id)
                .FirstOrDefault();
        }

        public void DeleteFlag(long userId, int? siteSettingId, List<long> childUserIds)
        {
            List<long?> childUserIdsNull = childUserIds == null ? null : childUserIds.Select(t => t as long?).ToList();
            var foundItem = db.Users.Where(t => t.Id == userId && t.SiteSettingId == siteSettingId && (childUserIdsNull == null || childUserIdsNull.Contains(t.CreateByUserId))).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.User_Not_Found);

            foundItem.IsDelete = true;
            foundItem.IsActive = false;
            db.SaveChanges();
        }

        public bool IsValidUser(long userId, int? siteSettingId, List<long> childUserIds, RoleType? Type)
        {
            List<long?> childUserIdsNullable = null;
            if (childUserIds != null)
                childUserIdsNullable = childUserIds.Select(t => t as long?).ToList();
            return db.Users.Any(t => t.SiteSettingId == siteSettingId && t.Id == userId && t.UserRoles.Any(tt => tt.Role.Type == Type) && (childUserIdsNullable == null || childUserIdsNullable.Contains(t.CreateByUserId)));
        }

        public object GetSelect2ListByType(Select2SearchVM searchInput, RoleType? rType)
        {
            List<object> result = new List<object>();
            long? loginUserId = GetLoginUser()?.UserId;

            var si = new
            {
                childUserIds = GetChildsUserId(loginUserId.ToLongReturnZiro()),
                siteSettingId = SiteSettingManager.GetSiteSetting()?.Id
            };

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            int? siteSettingId = si.siteSettingId;
            List<long?> childUserIds = null;
            if (si.childUserIds != null)
                childUserIds = si.childUserIds.Select(t => t as long?).ToList();

            var qureResult = db.Users.OrderByDescending(t => t.Id).Where(t => t.SiteSettingId == siteSettingId && t.UserRoles.Any(tt => tt.Role.Type == rType));
            if (childUserIds != null)
                qureResult = qureResult.Where(t => childUserIds.Contains(t.CreateByUserId));
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => (t.Firstname + " " + t.Lastname).Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Firstname + " " + t.Lastname }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public object GetSelect2ListByPPFAndCompanyId(Select2SearchVM searchInput, int? siteSettingId, int proposalFormId, int companyId, ProvinceAndCityVM provinceAndCityInput)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.Users.OrderByDescending(t => t.Id).Where(t => t.SiteSettingId == siteSettingId && !t.UserRoles.Any(tt => tt.Role.Name == "user") && t.IsActive == true && t.IsDelete != true);

            if (companyId > 0)
                qureResult = qureResult.Where(t => t.UserCompanies.Any(tt => tt.CompanyId == companyId));
            if (proposalFormId > 0)
                qureResult = qureResult.Where(t => t.UserRoles.Any(tt => tt.Role.RoleProposalForms.Any(ttt => ttt.ProposalFormId == proposalFormId)));
            if (provinceAndCityInput != null && provinceAndCityInput.provinceId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.ProvinceId == provinceAndCityInput.provinceId);
            if (provinceAndCityInput != null && provinceAndCityInput.cityId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.CityId == provinceAndCityInput.cityId);

            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => (t.Firstname + " " + t.Lastname + " " + (String.IsNullOrEmpty(t.Address) ? "" : ("(" + t.Address + ")"))).Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = (t.Firstname + " " + t.Lastname + " " + (String.IsNullOrEmpty(t.Address) ? "" : ("(" + t.Address + ")"))) }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public bool IsValidAgent(long id, int? siteSettingId, int proposalFormId, int companyId)
        {
            return 
                db.Users
                .Any(t => 
                        t.SiteSettingId == siteSettingId && !t.UserRoles.Any(tt => tt.Role.Name == "user") && t.Id == id &&
                        t.IsActive == true && t.IsDelete != true && t.UserCompanies.Any(tt => tt.CompanyId == companyId) &&
                        t.UserRoles.Any(tt => tt.Role.RoleProposalForms.Any(ttt => ttt.ProposalFormId == proposalFormId))
                    );
        }

        public bool IsValidAgent(long id, int? siteSettingId, int proposalFormId)
        {
            return
               db.Users
               .Any(t =>
                       t.SiteSettingId == siteSettingId && !t.UserRoles.Any(tt => tt.Role.Name == "user") && 
                       t.IsActive == true && t.IsDelete != true  && t.Id == id &&
                       t.UserRoles.Any(tt => tt.Role.RoleProposalForms.Any(ttt => ttt.ProposalFormId == proposalFormId))
                   );
        }
    }
}
