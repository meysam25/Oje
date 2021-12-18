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
    public class InsuranceContractTypeService : IInsuranceContractTypeService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IUserService UserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public InsuranceContractTypeService(
                InsuranceContractBaseDataDBContext db,
                IUserService UserService,
                ISiteSettingService SiteSettingService
            )
        {
            this.db = db;
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
        }

        public ApiResult Create(CreateUpdateInsuranceContractTypeVM input)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            CreateValidation(input, loginUserId, siteSettingId);

            db.Entry(new InsuranceContractType()
            {
                Title = input.title,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId.Value,
                SiteSettingId = siteSettingId.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateInsuranceContractTypeVM input, long? loginUserId, int? siteSettingId)
        {
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (db.InsuranceContractTypes.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (db.InsuranceContractTypes.Any(t => t.Id != input.id && t.SiteSettingId == siteSettingId && t.Title == input.title))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            List<long> childUserIds = UserService.GetChildsUserId(loginUserId.ToIntReturnZiro());

            var foundItem = db.InsuranceContractTypes.Where(t => t.Id == id && t.SiteSettingId == siteSettingId && (childUserIds == null || childUserIds.Contains(t.CreateUserId))).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateInsuranceContractTypeVM GetById(int? id)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            List<long> childUserIds = UserService.GetChildsUserId(loginUserId.ToIntReturnZiro());

            return db.InsuranceContractTypes
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId && (childUserIds == null || childUserIds.Contains(t.CreateUserId)))
                .Select(t => new CreateUpdateInsuranceContractTypeVM
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive
                }).FirstOrDefault();
        }

        public GridResultVM<InsuranceContractTypeMainGridResultVM> GetList(InsuranceContractTypeMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new InsuranceContractTypeMainGrid();
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            List<long> childUserIds = UserService.GetChildsUserId(loginUserId.Value);

            var qureResult = db.InsuranceContractTypes.Where(t => t.SiteSettingId == siteSettingId && (childUserIds == null || childUserIds.Contains(t.CreateUserId)));

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.createUser))
                qureResult = qureResult.Where(t => (t.CreateUser.Firstname + " " + t.CreateUser.Lastname).Contains(searchInput.createUser));
            if(!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                DateTime targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<InsuranceContractTypeMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    createUser = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    createDate = t.CreateDate,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new InsuranceContractTypeMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    createUser = t.createUser,
                    createDate = t.createDate.ToFaDate(),
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateInsuranceContractTypeVM input)
        {
            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            CreateValidation(input, loginUserId, siteSettingId);

            List<long> childUserIds = UserService.GetChildsUserId(loginUserId.Value);

            var foundItem = db.InsuranceContractTypes.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId && (childUserIds == null || childUserIds.Contains(t.CreateUserId))).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Title = input.title;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.UpdateDate = DateTime.Now;
            foundItem.UpdateUserId = loginUserId.Value;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList()
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            long? loginUserId = UserService.GetLoginUser()?.UserId;
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var childUsers = UserService.GetChildsUserId(loginUserId.Value);

            result.AddRange(db.InsuranceContractTypes.Where(t => t.SiteSettingId == siteSettingId && (childUsers == null || childUsers.Contains(t.CreateUserId))).Select(t => new
            {
                id = t.Id,
                title = t.Title
            }).ToList());

            return result;
        }

        public bool Exist(int id, int? siteSettingId, List<long> childUserIds)
        {
            return db.InsuranceContractTypes.Any(t => t.Id == id && t.SiteSettingId == siteSettingId && (childUserIds == null || childUserIds.Contains(t.CreateUserId)));
        }
    }
}
