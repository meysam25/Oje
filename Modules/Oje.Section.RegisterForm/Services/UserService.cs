using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.RegisterForm.Services
{
    public class UserService : IUserService
    {
        readonly RegisterFormDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        public UserService(RegisterFormDBContext db, IHttpContextAccessor HttpContextAccessor)
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult CreateNewUser(UserFilledRegisterForm userFilledRegisterForm, int? siteSettingId, long? parentUserId, List<int> roleIds)
        {
            CreateNewUserValidation(userFilledRegisterForm, siteSettingId, parentUserId, roleIds);

            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    string username = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mobile").Select(t => t.Value).FirstOrDefault();
                    var newUser = db.Users.Include(t => t.UserCompanies).Include(t => t.UserRoles).Where(t => t.Username == username && t.SiteSettingId == siteSettingId).FirstOrDefault();
                    if (newUser == null)
                    {
                        newUser = new User();
                        db.Entry(newUser).State = EntityState.Added;
                    }

                    newUser.SiteSettingId = siteSettingId;
                    newUser.Username = username;
                    newUser.BankShaba = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "shabaNo").Select(t => t.Value).FirstOrDefault();
                    newUser.BankAccount = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "hesabNo").Select(t => t.Value).FirstOrDefault();
                    newUser.Firstname = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "firstName").Select(t => t.Value).FirstOrDefault();
                    newUser.BirthDate = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "birthDate").Select(t => t.Value).FirstOrDefault().ToEnDate();
                    newUser.IsActive = true;
                    newUser.Lastname = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "lastName").Select(t => t.Value).FirstOrDefault();
                    newUser.Password = RandomService.GeneratePassword(20);
                    newUser.CreateDate = DateTime.Now;
                    newUser.ParentId = parentUserId;
                    newUser.Nationalcode = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "nationalCode").Select(t => t.Value).FirstOrDefault();
                    newUser.Mobile = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mobile").Select(t => t.Value).FirstOrDefault();
                    newUser.Email = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "email").Select(t => t.Value).FirstOrDefault();
                    newUser.Tell = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "tell").Select(t => t.Value).FirstOrDefault();
                    newUser.PostalCode = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "postalCode").Select(t => t.Value).FirstOrDefault();
                    newUser.Address = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "address").Select(t => t.Value).FirstOrDefault();
                    newUser.AgentCode = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "licenceNumber").Select(t => t.Value).FirstOrDefault().ToLongReturnZiro();
                    newUser.ProvinceId = userFilledRegisterForm.ProvinceId;
                    newUser.CityId = userFilledRegisterForm.CityId;
                    newUser.MapZoom = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapZoomRecivePlace").Select(t => t.Value).FirstOrDefault().ToByteReturnZiro() > 0 ? userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapZoomRecivePlace").Select(t => t.Value).FirstOrDefault().ToByteReturnZiro() : null;
                    newUser.MapLat = !string.IsNullOrEmpty(userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapLatRecivePlace").Select(t => t.Value).FirstOrDefault()) && userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapLatRecivePlace").Select(t => t.Value).FirstOrDefault().ToDoubleReturnNull() != null ? Convert.ToDecimal(userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapLatRecivePlace").Select(t => t.Value).FirstOrDefault()) : null;
                    newUser.MapLon = !string.IsNullOrEmpty(userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapLonRecivePlace").Select(t => t.Value).FirstOrDefault()) && userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapLonRecivePlace").Select(t => t.Value).FirstOrDefault().ToDoubleReturnNull() != null ? Convert.ToDecimal(userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapLonRecivePlace").Select(t => t.Value).FirstOrDefault()) : null;
                    newUser.MapLocation = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapLatRecivePlace").Select(t => t.Value).FirstOrDefault().ToDoubleReturnNull() != null && userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapLonRecivePlace").Select(t => t.Value).FirstOrDefault().ToDoubleReturnNull() != null ?
                                    new Point(userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapLatRecivePlace").Select(t => t.Value).FirstOrDefault().ToDoubleReturnNull().Value, userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapLonRecivePlace").Select(t => t.Value).FirstOrDefault().ToDoubleReturnNull().ToDoubleReturnNull().Value) { SRID = 4326 } :
                                    null;

                    db.SaveChanges();

                    if (userFilledRegisterForm.UserFilledRegisterFormCompanies != null && userFilledRegisterForm.UserFilledRegisterFormCompanies.Count > 0)
                    {
                        foreach (var company in userFilledRegisterForm.UserFilledRegisterFormCompanies)
                        {
                            if (company.CompanyId > 0)
                            {
                                if (newUser.UserCompanies == null || !newUser.UserCompanies.Any(t => t.CompanyId == company.CompanyId))
                                    db.Entry(new UserCompany()
                                    {
                                        CompanyId = company.CompanyId,
                                        UserId = newUser.Id
                                    }).State = EntityState.Added;
                            }
                        }
                        db.SaveChanges();
                    }

                    if (roleIds != null && roleIds.Count > 0)
                    {
                        foreach (var roleId in roleIds)
                            if (newUser.UserRoles == null || !newUser.UserRoles.Any(t => t.RoleId == roleId))
                                db.Entry(new UserRole() { RoleId = roleId, UserId = newUser.Id }).State = EntityState.Added;
                        db.SaveChanges();
                    }

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateNewUserValidation(UserFilledRegisterForm userFilledRegisterForm, int? siteSettingId, long? parentUserId, List<int> roleIds)
        {
            if (userFilledRegisterForm == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (siteSettingId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (parentUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (userFilledRegisterForm.UserFilledRegisterFormValues == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (roleIds == null || roleIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_One_Or_More_Role);

            string mobileNumber = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mobile").Select(t => t.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(mobileNumber))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile);
            if (!mobileNumber.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            //if (db.Users.Any(t => t.Username.Trim() == mobileNumber.Trim()))
            //    throw BException.GenerateNewException(BMessages.Dublicate_Mobile);
            if (string.IsNullOrEmpty(userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "firstName").Select(t => t.Value).FirstOrDefault()))
                throw BException.GenerateNewException(BMessages.Please_Enter_Firstname);
            if (string.IsNullOrEmpty(userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "lastName").Select(t => t.Value).FirstOrDefault()))
                throw BException.GenerateNewException(BMessages.Please_Enter_Lastname);
            if (string.IsNullOrEmpty(userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "nationalCode").Select(t => t.Value).FirstOrDefault()))
                throw BException.GenerateNewException(BMessages.Please_Enter_NationalCode);
            if (!userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "nationalCode").Select(t => t.Value).FirstOrDefault().IsCodeMeli())
                throw BException.GenerateNewException(BMessages.Invalid_NationaCode);
            if (string.IsNullOrEmpty(userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "email").Select(t => t.Value).FirstOrDefault()))
                throw BException.GenerateNewException(BMessages.Please_Enter_Email);
            if (!userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "email").Select(t => t.Value).FirstOrDefault().IsValidEmail())
                throw BException.GenerateNewException(BMessages.Invalid_Email);

            string postalCode = userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "postalCode").Select(t => t.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(postalCode) && postalCode.Length != 10)
                throw BException.GenerateNewException(BMessages.PostalCode_Is_Not_Valid);

            if (userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapLatRecivePlace").Select(t => t.Value).FirstOrDefault().ToDoubleReturnNull() != null &&
                userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapLonRecivePlace").Select(t => t.Value).FirstOrDefault().ToDoubleReturnNull() != null)
            {
                var tempPoint = new Point(userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapLatRecivePlace").Select(t => t.Value).FirstOrDefault().ToDoubleReturnNull().Value, userFilledRegisterForm.UserFilledRegisterFormValues.Where(t => t.UserFilledRegisterFormKey != null && t.UserFilledRegisterFormKey.Key == "mapLonRecivePlace").Select(t => t.Value).FirstOrDefault().ToDoubleReturnNull().ToDoubleReturnNull().Value) { SRID = 4326 };
                if (!tempPoint.IsValid)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
            }
        }

        public bool Exist(string userName, int? siteSettingId)
        {
            return db.Users.Any(t => t.Username.Trim() == userName.Trim() && t.SiteSettingId == siteSettingId);
        }

        public object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.Users.OrderByDescending(t => t.Id).Where(t => t.SiteSettingId == siteSettingId);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => (t.Username + "(" + t.Firstname + " " + t.Lastname + ")").Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Username + "(" + t.Firstname + " " + t.Lastname + ")" }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public void TemproryLogin(long? userId, int? siteSettingId, DateTime expireDate)
        {
            string cookiValue = userId + "," + siteSettingId + "," + expireDate + "," + HttpContextAccessor.GetIpAddress();
            var cOption = new CookieOptions() { HttpOnly = true };
            cOption.Expires = expireDate;
            HttpContextAccessor.HttpContext.Response.Cookies.Append("tLogin", cookiValue.Encrypt2(), cOption);
        }

    }
}
