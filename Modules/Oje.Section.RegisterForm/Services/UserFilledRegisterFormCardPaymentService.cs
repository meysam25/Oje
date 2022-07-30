using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Models.View;
using Oje.Section.RegisterForm.Services.EContext;
using System;
using System.Linq;

namespace Oje.Section.RegisterForm.Services
{
    public class UserFilledRegisterFormCardPaymentService : IUserFilledRegisterFormCardPaymentService
    {
        readonly IUploadedFileService UploadedFileService = null;
        readonly RegisterFormDBContext db = null;
        static readonly string validExtension = ".jpg,.jpeg,.png";

        public UserFilledRegisterFormCardPaymentService
            (
                RegisterFormDBContext db,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
        }

        public ApiResult Create(int? siteSettingId, UserFilledRegisterFormCardPaymentCreateUpdateVM input, long? loginUserId)
        {
            createUpdateValidation(siteSettingId, input, loginUserId);

            UserFilledRegisterFormCardPayment newItem = new UserFilledRegisterFormCardPayment()
            {
                CardNo = input.card,
                CreateDate = DateTime.Now,
                PayDate = input.pDate.ToEnDate().Value,
                Price = input.price.Value,
                RefferCode = input.refferCode,
                SiteSettingId = siteSettingId.Value,
                UserFilledRegisterFormId = input.fid.Value,
                UserId = loginUserId.Value
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.mainImage != null && input.mainImage.Length > 0)
            {
                newItem.ImageUrl = UploadedFileService.UploadNewFile(FileType.UserRigisterPaymentFile, input.mainImage, loginUserId, siteSettingId, newItem.Id, validExtension, true);
                db.SaveChanges();
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(int? siteSettingId, UserFilledRegisterFormCardPaymentCreateUpdateVM input, long? loginUserId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (input.fid.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (!db.UserFilledRegisterForms.Any(t => t.Id == input.fid && t.UserId == loginUserId && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (string.IsNullOrEmpty(input.card))
                throw BException.GenerateNewException(BMessages.Please_Enter_BankCardNo);
            if (input.card.Length != 19)
                throw BException.GenerateNewException(BMessages.Invalid_CardNo);
            if (input.card.Replace(" ", "").ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_CardNo);
            if (string.IsNullOrEmpty(input.refferCode))
                throw BException.GenerateNewException(BMessages.Please_Enter_TraceCode);
            if (input.price.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            if (string.IsNullOrEmpty(input.pDate))
                throw BException.GenerateNewException(BMessages.Please_Select_Date);
            if (input.pDate.ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (input.mainImage != null && input.mainImage.Length > 0 && !input.mainImage.IsValidExtension(validExtension))
                throw BException.GenerateNewException(BMessages.File_Is_Not_Valid);
        }

        public object GetList(UserRegisterFormPaymentMainGrid searchInput, int? siteSettingId, bool? isPayed, bool? isDone)
        {
            searchInput = searchInput ?? new UserRegisterFormPaymentMainGrid();

            var quiryResult = db.UserFilledRegisterFormCardPayments.Where(t => t.SiteSettingId == siteSettingId && t.UserFilledRegisterFormId == searchInput.pKey);

            if (isPayed != null)
                if (isPayed == true)
                    quiryResult = quiryResult.Where(t => !string.IsNullOrEmpty(t.UserFilledRegisterForm.PaymentTraceCode));
                else
                    quiryResult = quiryResult.Where(t => string.IsNullOrEmpty(t.UserFilledRegisterForm.PaymentTraceCode));
            if (isDone != null)
                if (isDone == true)
                    quiryResult = quiryResult.Where(t => t.UserFilledRegisterForm.IsDone == true);
                else if (isDone == false)
                    quiryResult = quiryResult.Where(t => t.UserFilledRegisterForm.IsDone == false || t.UserFilledRegisterForm.IsDone == null);

            if (!string.IsNullOrEmpty(searchInput.card))
                quiryResult = quiryResult.Where(t => t.CardNo.Contains(searchInput.card));
            if (!string.IsNullOrEmpty(searchInput.refferCode))
                quiryResult = quiryResult.Where(t => t.RefferCode.Contains(searchInput.refferCode));
            if (searchInput.price.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Price == searchInput.price);
            if (!string.IsNullOrEmpty(searchInput.pDate) && searchInput.pDate.ToEnDate() != null)
            {
                var targetDate = searchInput.pDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.PayDate.Year == targetDate.Year && t.PayDate.Month == targetDate.Month && t.PayDate.Day == targetDate.Day);
            }

            int row = searchInput.skip;

            return new
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    card = t.CardNo,
                    refferCode = t.RefferCode,
                    price = t.Price,
                    pDate = t.PayDate,
                    t.ImageUrl
                })
                .ToList()
                .Select(t => new
                {
                    row = ++row,
                    t.id,
                    t.card,
                    t.refferCode,
                    price = t.price.ToString("###,###"),
                    pDate = t.pDate.ToFaDate(),
                    src = !string.IsNullOrEmpty(t.ImageUrl) ? GlobalConfig.FileAccessHandlerUrl + t.ImageUrl : ""
                })
                .ToList()
            };
        }

        public ApiResult Delete(long? id, int? siteSettingId, bool? isPayed, bool? isDone)
        {
            var quiryResult = db.UserFilledRegisterFormCardPayments.Where(t => t.Id == id && t.SiteSettingId == siteSettingId);

            if (isPayed != null)
                if (isPayed == true)
                    quiryResult = quiryResult.Where(t => !string.IsNullOrEmpty(t.UserFilledRegisterForm.PaymentTraceCode));
                else
                    quiryResult = quiryResult.Where(t => string.IsNullOrEmpty(t.UserFilledRegisterForm.PaymentTraceCode));
            if (isDone != null)
                if (isDone == true)
                    quiryResult = quiryResult.Where(t => t.UserFilledRegisterForm.IsDone == true);
                else if (isDone == false)
                    quiryResult = quiryResult.Where(t => t.UserFilledRegisterForm.IsDone == false || t.UserFilledRegisterForm.IsDone == null);

            var foundItem = quiryResult.FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
