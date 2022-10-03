using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.DB;
using Oje.PaymentService.Models.View;
using Oje.PaymentService.Services.EContext;

namespace Oje.PaymentService.Services
{
    public class BankAccountSizpayService : IBankAccountSizpayService
    {
        readonly PaymentDBContext db = null;
        readonly IBankAccountService BankAccountService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public BankAccountSizpayService(
                PaymentDBContext db, 
                IBankAccountService BankAccountService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.BankAccountService = BankAccountService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(BankAccountSizpayCreateUpdateVM input, int? siteSettingId)
        {
            createValidation(input, siteSettingId);

            if (db.BankAccountSizpaies.Any(t => t.BankAccountId == input.bcId && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);


            db.Entry(new BankAccountSizpay()
            {
                BankAccountId = input.bcId.ToIntReturnZiro(),
                FirstKey = input.fistKey,
                MerchantId = input.merchId.ToLongReturnZiro(),
                SecondKey = input.secKey,
                SignKey = input.sKey,
                SiteSettingId = siteSettingId.Value,
                TerminalId = input.terminalId.ToIntReturnZiro()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(BankAccountSizpayCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.bcId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_BankAccount);
            if (string.IsNullOrEmpty(input.fistKey))
                throw BException.GenerateNewException(BMessages.Please_Enter_FirstKey);
            if (string.IsNullOrEmpty(input.secKey))
                throw BException.GenerateNewException(BMessages.Please_Enter_SecoundKey);
            if (string.IsNullOrEmpty(input.sKey))
                throw BException.GenerateNewException(BMessages.Please_Enter_SignKey);
            if (input.terminalId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_TerimanId);
            if (input.merchId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_MerchandId);
            if (!BankAccountService.Exist(siteSettingId, input.bcId))
                throw BException.GenerateNewException(BMessages.Please_Select_BankAccount);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.BankAccountSizpaies
                .Where(t => t.BankAccountId == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Detached;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.BankAccountSizpaies
                .Where(t => t.BankAccountId == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new
                {
                    bcId = t.BankAccountId,
                    bcId_Title = t.BankAccount.Title + "(" + t.BankAccount.CardNo + " " + t.BankAccount.HesabNo + ")",
                    fistKey = "",
                    secKey = "",
                    sKey = "",
                    terminalId = t.TerminalId,
                    merchId = t.MerchantId
                })
                .FirstOrDefault();
        }

        public GridResultVM<BankAccountSizpayMainGridResultVM> GetList(BankAccountSizpayMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new BankAccountSizpayMainGrid();

            var qureResult = db.BankAccountSizpaies
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.BankAccount.Title.Contains(searchInput.title));
            if (searchInput.terminalId.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.TerminalId == searchInput.terminalId);
            if (searchInput.merchandId.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.MerchantId == searchInput.merchandId);
            if (!string.IsNullOrEmpty(searchInput.shbaNo))
                qureResult = qureResult.Where(t => t.BankAccount.ShabaNo.Contains(searchInput.shbaNo));
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<BankAccountSizpayMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                    .OrderByDescending(t => t.BankAccountId)
                    .Skip(searchInput.skip)
                    .Take(searchInput.take)
                    .Select(t => new
                    {
                        id = t.BankAccountId,
                        title = t.BankAccount.Title,
                        terminalId = t.TerminalId,
                        merchandId = t.MerchantId,
                        shbaNo = t.BankAccount.ShabaNo,
                        siteTitleMN2 = t.SiteSetting.Title
                    })
                    .ToList()
                    .Select(t => new BankAccountSizpayMainGridResultVM
                    {
                        row = ++row,
                        id = t.id,
                        title = t.title,
                        terminalId = t.terminalId + "",
                        merchandId = t.merchandId + "",
                        shbaNo = "IR" + t.shbaNo,
                        siteTitleMN2 = t.siteTitleMN2
                    })
                    .ToList()
            };
        }

        public ApiResult Update(BankAccountSizpayCreateUpdateVM input, int? siteSettingId)
        {
            updateValidation(input, siteSettingId);

            var foundItem = db.BankAccountSizpaies
                .Where(t => t.BankAccountId == input.bcId)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!string.IsNullOrEmpty(input.fistKey))
                foundItem.FirstKey = input.fistKey;
            foundItem.MerchantId = input.merchId.ToLongReturnZiro();
            if (!string.IsNullOrEmpty(input.secKey))
                foundItem.SecondKey = input.secKey;
            if (!string.IsNullOrEmpty(input.sKey))
                foundItem.SignKey = input.sKey;
            foundItem.TerminalId = input.terminalId.ToIntReturnZiro();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void updateValidation(BankAccountSizpayCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.bcId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_BankAccount);
            if (input.terminalId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_TerimanId);
            if (input.merchId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_MerchandId);
            if (!BankAccountService.Exist(siteSettingId, input.bcId))
                throw BException.GenerateNewException(BMessages.Please_Select_BankAccount);
        }

        public BankAccountSizpay GetBy(int bankAccountId, int? siteSettingId)
        {
            return db.BankAccountSizpaies.AsNoTracking().Where(t => t.SiteSettingId == siteSettingId && t.BankAccountId == bankAccountId && t.BankAccount.IsActive == true && t.BankAccount.IsForPayment == true && t.BankAccount.Bank.IsActive == true).FirstOrDefault();
        }

        public bool Exist(int bankAccountId, int? siteSettingId)
        {
            return db.BankAccountSizpaies.Any(t => t.BankAccountId == bankAccountId && t.SiteSettingId == siteSettingId && t.BankAccount.IsActive == true && t.BankAccount.IsForPayment == true);
        }
    }
}
