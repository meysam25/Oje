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
    public class BankAccountSepService : IBankAccountSepService
    {
        readonly PaymentDBContext db = null;
        public BankAccountSepService
            (
                PaymentDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(BankAccountSepCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new BankAccountSep()
            {
                BankAccountId = input.baId.Value,
                SiteSettingId = siteSettingId.Value,
                TerminalId = input.terminalId
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(BankAccountSepCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.baId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_BankAccount);
            if (!db.BankAccounts.Any(t => t.Id == input.baId && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Please_Select_BankAccount);
            if (string.IsNullOrEmpty(input.terminalId))
                throw BException.GenerateNewException(BMessages.Please_Enter_TerimanId);
            if (db.BankAccountSeps.Any(t => t.SiteSettingId == siteSettingId && t.Id != input.id && t.BankAccountId == input.baId))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.BankAccountSeps.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.BankAccountSeps
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    terminalId = t.TerminalId,
                    baId = t.BankAccountId,
                    baId_Title = t.BankAccount.Title
                }).FirstOrDefault();
        }

        public GridResultVM<BankAccountSepMainGridResultVM> GetList(BankAccountSepMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new BankAccountSepMainGrid();
            var quiryResult = db.BankAccountSeps.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.bankAcount))
                quiryResult = quiryResult.Where(t => (t.BankAccount.Title + "(" + t.BankAccount.CardNo + "-" + t.BankAccount.HesabNo + ")").Contains(searchInput.bankAcount));
            if (!string.IsNullOrEmpty(searchInput.terminalId))
                quiryResult = quiryResult.Where(t => t.TerminalId.Contains(searchInput.terminalId));

            int row = searchInput.skip;

            return new GridResultVM<BankAccountSepMainGridResultVM>()
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
                    terminalId = t.TerminalId
                })
                .ToList()
                .Select(t => new BankAccountSepMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    bankAcount = t.bankAcount,
                    terminalId = t.terminalId
                })
                .ToList()
            };
        }

        public ApiResult Update(BankAccountSepCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.BankAccountSeps.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.BankAccountId = input.baId.Value;
            foundItem.TerminalId = input.terminalId;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public BankAccountSep GetBy(int bankAccountId, int? siteSettingId)
        {
            return db.BankAccountSeps.Where(t => t.BankAccountId == bankAccountId && t.SiteSettingId == siteSettingId).AsNoTracking().FirstOrDefault();
        }

        public bool Exist(int bankAccountId, int? siteSettingId)
        {
            return db.BankAccountSeps.Any(t => t.BankAccountId == bankAccountId && t.SiteSettingId == siteSettingId);
        }
    }
}
