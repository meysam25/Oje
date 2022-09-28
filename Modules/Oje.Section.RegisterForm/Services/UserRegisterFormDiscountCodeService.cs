using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Models.View;
using Oje.Section.RegisterForm.Services.EContext;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Oje.Section.RegisterForm.Services
{
    public class UserRegisterFormDiscountCodeService : IUserRegisterFormDiscountCodeService
    {
        readonly RegisterFormDBContext db = null;
        readonly IUserRegisterFormService UserRegisterFormService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public UserRegisterFormDiscountCodeService
            (
              RegisterFormDBContext db,
              IUserRegisterFormService UserRegisterFormService,
              IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.UserRegisterFormService = UserRegisterFormService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(UserRegisterFormDiscountCodeCreateUpdateVM input, int? siteSettingId)
        {
            createValidationValidation(input, siteSettingId);

            db.Entry(new UserRegisterFormDiscountCode()
            {
                Code = input.discountCode,
                CreateDate = DateTime.Now,
                FromDate = input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                MaxPrice = input.maxPrice.ToLongReturnZiro(),
                MaxUse = input.countUse.ToIntReturnZiro(),
                Percent = input.percent,
                Price = input.price,
                UserRegisterFormId = input.formId.Value,
                SiteSettingId = siteSettingId.Value,
                Title = input.title,
                ToDate = input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidationValidation(UserRegisterFormDiscountCodeCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.formId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (!UserRegisterFormService.Exist(input.formId.ToIntReturnZiro(), siteSettingId))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (string.IsNullOrEmpty(input.fromDate))
                throw BException.GenerateNewException(BMessages.Please_Enter_FromDate);
            if (input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (string.IsNullOrEmpty(input.toDate))
                throw BException.GenerateNewException(BMessages.Please_Enter_ToDate);
            if (input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (input.percent.ToIntReturnZiro() <= 0 && input.price.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Percent_Or_Price);
            if (input.percent.ToIntReturnZiro() > 0 && input.price.ToLongReturnZiro() > 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Percent_Or_Price);
            if (input.percent.ToIntReturnZiro() >= 100)
                throw BException.GenerateNewException(BMessages.Invalid_Percent);
            if (input.maxPrice.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_MaxPrice);
            if (input.countUse.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_CountUse);
            if (input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value > input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value)
                throw BException.GenerateNewException(BMessages.ToDate_Should_Be_Greader_Then_FromYear);
        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem = db.UserRegisterFormDiscountCodes
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
               .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetBy(long? id, int? siteSettingId)
        {
            return db.UserRegisterFormDiscountCodes
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    countUse = t.MaxUse,
                    discountCode = t.Code,
                    formId = t.UserRegisterFormId,
                    formId_Title = t.UserRegisterForm.Title,
                    fromDate = t.FromDate,
                    id = t.Id,
                    isActive = t.IsActive,
                    maxPrice = t.MaxPrice,
                    percent = t.Percent,
                    price = t.Price,
                    title = t.Title,
                    toDate = t.ToDate
                })
                .OrderByDescending(t => t.id)
                .Take(1)
                .ToList()
                .Select(t => new
                {
                    toDate = t.toDate.ToFaDate(),
                    title = t.title,
                    price = t.price,
                    countUse = t.countUse,
                    discountCode = t.discountCode,
                    formId = t.formId,
                    fromDate = t.fromDate.ToFaDate(),
                    isActive = t.isActive,
                    maxPrice = t.maxPrice,
                    id = t.id,
                    percent = t.percent,
                    formId_Title = t.formId_Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<UserRegisterFormDiscountCodeMainGridResultVM> GetList(UserRegisterFormDiscountCodeMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new UserRegisterFormDiscountCodeMainGrid();

            var qureResult = db.UserRegisterFormDiscountCodes.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.ppfTitle))
                qureResult = qureResult.Where(t => t.UserRegisterForm.Title.Contains(searchInput.ppfTitle));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
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
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<UserRegisterFormDiscountCodeMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    createDate = t.CreateDate,
                    fromDate = t.FromDate,
                    id = t.Id,
                    isActive = t.IsActive,
                    ppfTitle = t.UserRegisterForm.Title,
                    title = t.Title,
                    toDate = t.ToDate
                })
                .ToList()
                .Select(t => new UserRegisterFormDiscountCodeMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    ppfTitle = t.ppfTitle,
                    createDate = t.createDate.ToFaDate(),
                    title = t.title,
                    fromDate = t.fromDate.ToFaDate(),
                    toDate = t.toDate.ToFaDate(),
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public ApiResult Update(UserRegisterFormDiscountCodeCreateUpdateVM input, int? siteSettingId)
        {
            createValidationValidation(input, siteSettingId);

            var foundItem = db.UserRegisterFormDiscountCodes
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Code = input.discountCode;
            foundItem.FromDate = input.fromDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.MaxPrice = input.maxPrice.ToLongReturnZiro();
            foundItem.MaxUse = input.countUse.ToIntReturnZiro();
            foundItem.Percent = input.percent;
            foundItem.Price = input.price;
            foundItem.UserRegisterFormId = input.formId.Value;
            foundItem.Title = input.title;
            foundItem.ToDate = input.toDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public UserRegisterFormDiscountCode GetBy(string code, int? siteSettingId, int formId)
        {
            var curDate = DateTime.Now;

            return db.UserRegisterFormDiscountCodes
                .Where(t => t.Code == code && t.SiteSettingId == siteSettingId && t.FromDate <= curDate && t.ToDate >= curDate && t.UserRegisterFormDiscountCodeUses.Count < t.MaxUse && t.IsActive == true && t.UserRegisterFormId == formId)
                .FirstOrDefault();
        }

        public void DiscountUsed(int userRegisterFormDiscountCodeId, long? userId, long userFilledRegisterFormId)
        {
            if (userRegisterFormDiscountCodeId > 0 && userId > 0 && userFilledRegisterFormId > 0)
            {
                db.Entry(new UserRegisterFormDiscountCodeUse()
                {
                    UserFilledRegisterFormId = userFilledRegisterFormId,
                    UserId = userId.Value,
                    CreateDate = DateTime.Now,
                    UserRegisterFormDiscountCodeId = userRegisterFormDiscountCodeId
                }).State = EntityState.Added;
                db.SaveChanges();
            }
        }
    }
}
