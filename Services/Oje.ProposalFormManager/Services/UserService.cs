using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    internal class UserService : IUserService
    {
        readonly ProposalFormDBContext db = null;
        readonly IRoleService RoleService = null;
        public UserService(ProposalFormDBContext db, IRoleService RoleService)
        {
            this.db = db;
            this.RoleService = RoleService;
        }

        public long CreateUserForProposalFormIfNeeded(IFormCollection form, int? siteSettingId, long? loginUserId)
        {
            string mobileNumber = form.GetStringIfExist("mobile");
            if (string.IsNullOrEmpty(mobileNumber))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile);
            if (!mobileNumber.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);

            User newUser = db.Users.Where(t => t.Username == mobileNumber && t.SiteSettingId == siteSettingId).AsNoTracking().FirstOrDefault();
            if (newUser != null)
                return newUser.Id;

            newUser = new User()
            {
                Username = mobileNumber,
                SiteSettingId = siteSettingId.Value,
                Address = form.GetStringIfExist("address"),
                CreateByUserId = loginUserId,
                CreateDate = DateTime.Now,
                Email = form.GetStringIfExist("email"),
                Firstname = form.GetStringIfExist("realOrLegaPerson") == "1" ? form.GetStringIfExist("firstName") : form.GetStringIfExist("firstAgentName"),
                IsActive = true,
                Lastname = form.GetStringIfExist("realOrLegaPerson") == "1" ? form.GetStringIfExist("lastName") : form.GetStringIfExist("lastAgentName"),
                Mobile = mobileNumber,
                Nationalcode = form.GetStringIfExist("realOrLegaPerson") == "1" ? form.GetStringIfExist("nationalCode") : null,
                PostalCode = form.GetStringIfExist("postalCode"),
                ParentId = loginUserId,
                Tell = form.GetStringIfExist("tell"),
                Password = RandomService.GeneratePassword(12).GetSha1()
            };


            createForProposalFormValidation(newUser);

            db.Entry(newUser).State = EntityState.Added;
            db.SaveChanges();

            newUser.FilledSignature();
            db.SaveChanges();

            RoleService.AddUserToUserRole(newUser.Id);

            return newUser.Id;
        }

        public object GetSelect2List(Select2SearchVM searchInput, int? roleId, int? companyId, int? provinceId, int? cityId, int? siteSettingId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.Users.Where(t => t.SiteSettingId == siteSettingId);
            if (roleId > 0)
                qureResult = qureResult.Where(t => t.UserRoles.Any(tt => tt.RoleId == roleId));
            if (companyId > 0)
                qureResult = qureResult.Where(t => t.UserCompanies.Any(tt => tt.CompanyId == companyId));
            if (provinceId > 0)
                qureResult = qureResult.Where(t => t.ProvinceId == provinceId);
            if (cityId > 0)
                qureResult = qureResult.Where(t => t.CityId == cityId);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => (t.Firstname + " " + t.Lastname + "(" + t.Username + ")").Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Firstname + " " + t.Lastname + "(" + t.Username + ")" }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public List<int> GetUserCompanies(long? userId)
        {
            return db.Users.Where(t => t.Id == userId).SelectMany(t => t.UserCompanies).Select(t => t.CompanyId).ToList();
        }

        public long GetUserWalletBalance(long? userId, int? siteSettingId)
        {
            return db.Users.Where(t => t.SiteSettingId == siteSettingId && t.Id == userId && t.IsActive == true && t.IsDelete != true).SelectMany(t => t.WalletTransactions).Sum(t => t.Price);
        }

        private void createForProposalFormValidation(User newUser)
        {
            if (string.IsNullOrEmpty(newUser.Username))
                throw BException.GenerateNewException(BMessages.Please_Enter_Username);
            if (newUser.Username.Length > 50)
                throw BException.GenerateNewException(BMessages.Username_Can_Not_Be_More_Then_50_chars);
            if (string.IsNullOrEmpty(newUser.Firstname))
                throw BException.GenerateNewException(BMessages.Please_Enter_Firstname);
            if (newUser.Firstname.Length > 50)
                throw BException.GenerateNewException(BMessages.FirstName_CanNot_Be_MoreThen_50_Char);
            if (string.IsNullOrEmpty(newUser.Lastname))
                throw BException.GenerateNewException(BMessages.Please_Enter_Lastname);
            if (newUser.Lastname.Length > 50)
                throw BException.GenerateNewException(BMessages.Last_CanNot_Be_MoreThen_50_Char);
            if (string.IsNullOrEmpty(newUser.Password))
                throw BException.GenerateNewException(BMessages.Please_Enter_Password);
            if (newUser.Password.Length > 200)
                throw BException.GenerateNewException(BMessages.Password_Can_Not_Be_More_Then_200_Chars);
            if (!string.IsNullOrEmpty(newUser.Nationalcode) && newUser.Nationalcode.Length > 12)
                throw BException.GenerateNewException(BMessages.Invalid_NationaCode);
            if (!string.IsNullOrEmpty(newUser.Mobile) && newUser.Mobile.Length > 14)
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (!string.IsNullOrEmpty(newUser.Email) && newUser.Email.Length > 100)
                throw BException.GenerateNewException(BMessages.Email_CanNot_Be_More_Then_100_Chars);
            if (!string.IsNullOrEmpty(newUser.Tell) && newUser.Tell.Length > 50)
                throw BException.GenerateNewException(BMessages.Tell_CanNot_Be_MoreThen_50_Chars);
            if (!string.IsNullOrEmpty(newUser.PostalCode) && newUser.PostalCode.Length > 12)
                throw BException.GenerateNewException(BMessages.PostalCode_Is_Not_Valid);
            if (!string.IsNullOrEmpty(newUser.Address) && newUser.Address.Length > 1000)
                throw BException.GenerateNewException(BMessages.Address_Length_Is_Not_Valid);

        }
    }
}
