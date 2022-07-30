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
    public class BankAccountSadadService : IBankAccountSadadService
    {
        readonly PaymentDBContext db = null;
        public BankAccountSadadService(PaymentDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(BankAccountSadadCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new BankAccountSadad()
            {
                BankAccountId = input.baId.Value,
                MerchantId = input.merchantId,
                SiteSettingId = siteSettingId.Value,
                TerminalId = input.terminalId,
                TerminalKey = input.terminalKey
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(BankAccountSadadCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.baId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_BankAccount);
            if (!db.BankAccounts.Any(t => t.SiteSettingId == siteSettingId && t.Id == input.baId))
                throw BException.GenerateNewException(BMessages.Please_Select_BankAccount);
            if (string.IsNullOrEmpty(input.merchantId))
                throw BException.GenerateNewException(BMessages.Please_Enter_MerchandId);
            if (string.IsNullOrEmpty(input.terminalId))
                throw BException.GenerateNewException(BMessages.Please_Enter_TerimanId);
            if (input.id.ToIntReturnZiro() <= 0 && string.IsNullOrEmpty(input.terminalKey))
                throw BException.GenerateNewException(BMessages.Please_Enter_Private_Key);
            if (db.BankAccountSadads.Any(t => t.Id != input.id && t.SiteSettingId == siteSettingId && t.BankAccountId == input.baId))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);

        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.BankAccountSadads.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.BankAccountSadads
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    baId = t.BankAccountId,
                    baId_Title = t.BankAccount.Title,
                    merchantId = t.MerchantId,
                    terminalId = t.TerminalId
                })
                .FirstOrDefault();
        }

        public GridResultVM<BankAccountSadadMainGridResultVM> GetList(BankAccountSadadMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new BankAccountSadadMainGrid();
            var quiryResult = db.BankAccountSadads.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.bankAcount))
                quiryResult = quiryResult.Where(t => (t.BankAccount.Title + "(" + t.BankAccount.CardNo + "-" + t.BankAccount.HesabNo + ")").Contains(searchInput.bankAcount));
            if (!string.IsNullOrEmpty(searchInput.merchantId))
                quiryResult = quiryResult.Where(t => t.MerchantId.Contains(searchInput.merchantId));
            if (!string.IsNullOrEmpty(searchInput.terminalId))
                quiryResult = quiryResult.Where(t => t.TerminalId.Contains(searchInput.terminalId));

            int row = searchInput.skip;

            return new GridResultVM<BankAccountSadadMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    bankAcount = t.BankAccount.Title + "(" + t.BankAccount.CardNo + "-" + t.BankAccount.HesabNo + ")",
                    merchantId = t.MerchantId,
                    terminalId = t.TerminalId
                })
                .ToList()
                .Select(t => new BankAccountSadadMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    bankAcount = t.bankAcount,
                    merchantId = t.merchantId,
                    terminalId = t.terminalId
                })
                .ToList()
            };
        }

        public ApiResult Update(BankAccountSadadCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.BankAccountSadads.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.BankAccountId = input.baId.Value;
            foundItem.MerchantId = input.merchantId;
            foundItem.TerminalId = input.terminalId;
            if (!string.IsNullOrEmpty(input.terminalKey))
                foundItem.TerminalKey = input.terminalKey;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public bool Exist(int bankAccountId, int? siteSettingId)
        {
            return db.BankAccountSadads.Any(t => t.BankAccountId == bankAccountId && t.SiteSettingId == siteSettingId && t.BankAccount.IsActive == true && t.BankAccount.IsForPayment == true);
        }

        public BankAccountSadad GetBy(int bankAccountId, int? siteSettingId)
        {
            return db.BankAccountSadads.AsNoTracking().Where(t => t.BankAccountId == bankAccountId && t.SiteSettingId == siteSettingId && t.BankAccount.IsForPayment == true && t.BankAccount.IsActive == true).FirstOrDefault();
        }
    }
}
