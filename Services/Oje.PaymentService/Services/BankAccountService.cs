using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.DB;
using Oje.PaymentService.Models.View;
using Oje.PaymentService.Services.EContext;

namespace Oje.PaymentService.Services
{
    public class BankAccountService : IBankAccountService
    {
        readonly PaymentDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public BankAccountService
            (
                PaymentDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(BankAccountCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            var newItem = new BankAccount()
            {
                BankId = input.bankId.ToIntReturnZiro(),
                CardNo = input.cardNo.ToLongReturnZiro(),
                HesabNo = input.hesabNo.ToLongReturnZiro(),
                IsActive = input.isActive.ToBooleanReturnFalse(),
                IsForPayment = input.isForPayment.ToBooleanReturnFalse(),
                ShabaNo = input.shabaNo,
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Title = input.title,
                UserId = input.userId
            };
            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            newItem.FilledSignature();
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(BankAccountCreateUpdateVM input, int? siteSettingId, bool? canSetSiteSetting)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.bankId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Bank);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.cardNo.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_CardNo);
            if (string.IsNullOrEmpty(input.shabaNo))
                throw BException.GenerateNewException(BMessages.Please_Enter_ShabaNo);
            if (input.hesabNo.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_HesabNo);
            if (input.cardNo.Value.FormatCardNo().Length != 19)
                throw BException.GenerateNewException(BMessages.Invalid_CardNo);
            if (input.shabaNo.Length != 24)
                throw BException.GenerateNewException(BMessages.Invalid_ShabaNo);
            if(input.userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_One_User);
            if (!db.Users.Any(t => t.Id == input.userId && t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value)))
                throw BException.GenerateNewException(BMessages.User_Not_Found);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.BankAccounts
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundItem.IsSignature())
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.BankAccounts
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    bankId = t.BankId,
                    title = t.Title,
                    cardNo = t.CardNo,
                    shabaNo = t.ShabaNo,
                    hesabNo = t.HesabNo,
                    userId = t.UserId,
                    userId_Title = t.UserId > 0 ? t.User.Username + "(" + t.User.Firstname + " " + t.User.Lastname + ")" : "",
                    isForPayment = t.IsForPayment,
                    isActive = t.IsActive,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                }).FirstOrDefault();
        }

        public GridResultVM<BankAccountMainGridResultVM> GetList(BankAccountMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new BankAccountMainGrid();

            var qureResult = db.BankAccounts
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (searchInput.bankId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.BankId == searchInput.bankId);
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.userfullanme))
                qureResult = qureResult.Where(t => t.UserId > 0 && (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.userfullanme));
            if (searchInput.hesabNo.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.HesabNo == searchInput.hesabNo);
            if (searchInput.cardNo.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.CardNo == searchInput.cardNo);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<BankAccountMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    bankId = t.Bank.Title,
                    title = t.Title,
                    userfullanme = t.UserId > 0 ? t.User.Firstname + " " + t.User.Lastname : "",
                    cardNo = t.CardNo,
                    hesabNo = t.HesabNo,
                    isActive = t.IsActive,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new BankAccountMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    bankId = t.bankId,
                    cardNo = t.cardNo.FormatCardNo(),
                    hesabNo = t.hesabNo.ToString(),
                    title = t.title,
                    userfullanme = t.userfullanme,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(BankAccountCreateUpdateVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            var foundItem = db.BankAccounts
                .Where(t => t.Id == input.id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundItem.IsSignature())
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

            foundItem.BankId = input.bankId.ToIntReturnZiro();
            foundItem.CardNo = input.cardNo.ToLongReturnZiro();
            foundItem.HesabNo = input.hesabNo.ToLongReturnZiro();
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.IsForPayment = input.isForPayment.ToBooleanReturnFalse();
            foundItem.ShabaNo = input.shabaNo;
            foundItem.Title = input.title;
            foundItem.UserId = input.userId;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
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

            var qureResult = db.BankAccounts
                .OrderByDescending(t => t.Id)
                .Where(t => t.IsForPayment == true && t.SiteSettingId == siteSettingId)
                ;
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => (t.Title + "(" + t.CardNo + "-" + t.HesabNo + ")").Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title + "(" + t.CardNo + "-" + t.HesabNo + ")" }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public bool Exist(int? siteSettingId, int? id)
        {
            return db.BankAccounts.Any(t => t.Id == id && t.SiteSettingId == siteSettingId);
        }

        public List<BankAccountPaymentUserVM> GetAllAcountForPayment(long? userId, int? siteSettingId)
        {
            var qureResult = db.BankAccounts.Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true && t.IsForPayment == true && t.Bank.IsActive == true);

            if (userId.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.UserId == userId);

            return qureResult
                .Where(t => t.IsActive == true && t.IsForPayment == true)
                .OrderByDescending(t => t.Id)
                .Select(t => new
                {
                    bankAccountId = t.Id,
                    bankTitle = t.Bank.Title,
                    userFullname = t.UserId > 0 ? t.User.Firstname + " " + t.User.Lastname : "",
                    accountTitle = t.Title,
                    bankIcon = t.Bank.Pic
                })
                .Select(t => new BankAccountPaymentUserVM
                {
                    accountTitle = t.accountTitle,
                    bankAccountId = t.bankAccountId,
                    bankIcon = GlobalConfig.FileAccessHandlerUrl + t.bankIcon,
                    bankTitle = t.bankTitle,
                    userFullname = t.userFullname
                }).ToList();
        }

        public bool Exist(long? userId, long? bankAccountId, int? siteSettingId)
        {
            var qureResult = db.BankAccounts.Where(t => t.Id == bankAccountId && t.SiteSettingId == siteSettingId);

            if (userId.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.UserId == userId);

            return qureResult.Any();
        }

        public BankAccountUserInfoVM GetUserInfo(int id, int? siteSettingId)
        {
            return db.BankAccounts
                .OrderByDescending(t => t.Id)
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .Select(t => new BankAccountUserInfoVM
                {
                    shabaNO = "IR" + t.ShabaNo,
                    mobileNo = t.User.Username,
                    fullname = t.User.Firstname + " " + t.User.Lastname
                })
                .FirstOrDefault();
        }
    }
}
