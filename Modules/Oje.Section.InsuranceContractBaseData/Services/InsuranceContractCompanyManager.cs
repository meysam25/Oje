using Oje.AccountManager.Interfaces;
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
    public class InsuranceContractCompanyManager : IInsuranceContractCompanyManager
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IUserManager UserManager = null;
        readonly ISiteSettingManager SiteSettingManager = null;
        public InsuranceContractCompanyManager
            (
                InsuranceContractBaseDataDBContext db,
                IUserManager UserManager,
                ISiteSettingManager SiteSettingManager
            )
        {
            this.db = db;
            this.UserManager = UserManager;
            this.SiteSettingManager = SiteSettingManager;
        }

        public ApiResult Create(CreateUpdateInsuranceContractCompanyVM input)
        {
            long? loginUserId = UserManager.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;

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
                SiteSettingId = siteSettingId.Value,
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
                throw BException.GenerateNewException(BMessages.Manager_Name_Can_Not_Be_More_Then_100_chars);
        }

        public ApiResult Delete(int? id)
        {
            long? loginUserId = UserManager.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUsers = UserManager.GetChildsUserId(loginUserId.ToIntReturnZiro());

            var foundItem = db.InsuranceContractCompanies.Where(t => t.Id == id && t.SiteSettingId == siteSettingId && (childUsers == null || childUsers.Contains(t.CreateUserId))).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateInsuranceContractCompanyVM GetById(int? id)
        {
            long? loginUserId = UserManager.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUsers = UserManager.GetChildsUserId(loginUserId.ToIntReturnZiro());


            return db.InsuranceContractCompanies
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId && (childUsers == null || childUsers.Contains(t.CreateUserId)))
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
                    title = t.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<InsuranceContractCompanyMainGridResultVM> GetList(InsuranceContractCompanyMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new InsuranceContractCompanyMainGrid();
            long? loginUserId = UserManager.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUsers = UserManager.GetChildsUserId(loginUserId.ToIntReturnZiro());

            var qureResult = db.InsuranceContractCompanies.Where(t => t.SiteSettingId == siteSettingId && (childUsers == null || childUsers.Contains(t.CreateUserId)));

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
                    title = t.Title
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
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateInsuranceContractCompanyVM input)
        {
            long? loginUserId = UserManager.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;

            CreateValidation(input, loginUserId, siteSettingId);

            var childUsers = UserManager.GetChildsUserId(loginUserId.Value);

            var foundItem = db.InsuranceContractCompanies.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId && (childUsers == null || childUsers.Contains(t.CreateUserId))).FirstOrDefault();
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

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public bool Exist(int id, int? siteSettingId, List<long> childUserIds)
        {
            return db.InsuranceContractCompanies.Any(t => t.Id == id && t.SiteSettingId == siteSettingId && (childUserIds == null || childUserIds.Contains(t.CreateUserId)));
        }

        public object GetLightList()
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            long? loginUserId = UserManager.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            var childUsers = UserManager.GetChildsUserId(loginUserId.Value);

            result.AddRange(db.InsuranceContractCompanies.Where(t => t.SiteSettingId == siteSettingId && (childUsers == null || childUsers.Contains(t.CreateUserId)))
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
