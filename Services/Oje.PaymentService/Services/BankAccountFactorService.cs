using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.DB;
using Oje.PaymentService.Services.EContext;

namespace Oje.PaymentService.Services
{
    public class BankAccountFactorService : IBankAccountFactorService
    {
        readonly PaymentDBContext db = null;
        readonly IProposalFilledFormService ProposalFilledFormService = null;
        readonly IBankAccountService BankAccountService = null;
        readonly IWalletTransactionService WalletTransactionService = null;
        readonly IBankAccountDetectorService BankAccountDetectorService = null;
        readonly IUserFilledRegisterFormService UserFilledRegisterFormService = null;

        public BankAccountFactorService(
                PaymentDBContext db,
                IProposalFilledFormService ProposalFilledFormService,
                IBankAccountService BankAccountService,
                IWalletTransactionService WalletTransactionService,
                IBankAccountDetectorService BankAccountDetectorService,
                IUserFilledRegisterFormService UserFilledRegisterFormService
            )
        {
            this.db = db;
            this.BankAccountService = BankAccountService;
            this.ProposalFilledFormService = ProposalFilledFormService;
            this.WalletTransactionService = WalletTransactionService;
            this.BankAccountDetectorService = BankAccountDetectorService;
            this.UserFilledRegisterFormService = UserFilledRegisterFormService;
        }

        public string Create(int? bankAccountId, PaymentFactorVM payModel, int? siteSettingId, long? loginUserId)
        {
            if (payModel != null && payModel.price.ToLongReturnZiro() > 0 && payModel.objectId.ToLongReturnZiro() > 0 && !string.IsNullOrEmpty(payModel.returnUrl) && siteSettingId.ToIntReturnZiro() > 0)
            {
                if (!BankAccountService.Exist(payModel.userId, bankAccountId, siteSettingId))
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                var newHashCode = (bankAccountId + ((int)payModel.type) + "_" + payModel.objectId + "_" + DateTime.Now.Ticks).GetHashCode32();
                if (newHashCode < 0)
                    newHashCode = newHashCode * -1;
                var newItem = new BankAccountFactor()
                {
                    BankAccountId = bankAccountId.ToIntReturnZiro(),
                    CreateDate = DateTime.Now,
                    ObjectId = payModel.objectId,
                    Price = payModel.price,
                    SiteSettingId = siteSettingId.ToIntReturnZiro(),
                    TargetLink = payModel.returnUrl,
                    Type = payModel.type,
                    UserId = loginUserId,
                    BankAccountType = BankAccountDetectorService.GetByType(bankAccountId.ToIntReturnZiro(), siteSettingId),
                    KeyHash = newHashCode
                };
                db.Entry(newItem).State = EntityState.Added;
                db.SaveChanges();

                return newItem.BankAccountId + "_" + ((int)newItem.Type) + "_" + newItem.ObjectId + "_" + newItem.CreateDate.Ticks;
            }
            throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        public bool ExistBy(string traceNo)
        {
            return db.BankAccountFactors.Any(t => t.TraceCode == traceNo);
        }

        public BankAccountFactor GetBy(string keyHash, int? siteSettingId)
        {
            var intKeyHAsh = keyHash.GetHashCode32();
            return db.BankAccountFactors.Where(t => t.SiteSettingId == siteSettingId && t.KeyHash == intKeyHAsh).AsNoTracking().FirstOrDefault();
        }

        public BankAccountFactor GetById(string bankAccountFactorId, int? siteSettingId)
        {
            if (string.IsNullOrEmpty(bankAccountFactorId))
                return null;
            if (siteSettingId.ToIntReturnZiro() <= 0)
                return null;
            if (bankAccountFactorId.IndexOf("_") == -1)
                return null;
            var splitIds = bankAccountFactorId.Split('_');
            if (splitIds.Length != 4)
                return null;

            int bankAccoundId = splitIds[0].ToIntReturnZiro();
            int bankAccoundType = splitIds[1].ToIntReturnZiro();
            BankAccountFactorType? type = null;
            try { type = (BankAccountFactorType)bankAccoundType; } catch { return null; }
            long objectId = splitIds[2].ToLongReturnZiro();
            DateTime createDate = new DateTime(splitIds[3].ToLongReturnZiro());

            return db.BankAccountFactors.Where(t => t.BankAccountId == bankAccoundId && t.Type == type.Value && t.ObjectId == objectId && t.CreateDate == createDate).FirstOrDefault();
        }

        public List<ProposalFilledFormPaymentVM> GetListBy(BankAccountFactorType type, long objectId, int? siteSettingId)
        {
            return db.BankAccountFactors
                .Where(t => t.Type == type && t.ObjectId == objectId && t.SiteSettingId == siteSettingId && t.PayDate != null && t.IsPayed == true)
                .Select(t => new
                {
                    bankTitle = t.BankAccount.Bank.Title,
                    accTitle = t.BankAccount.Title,
                    userFullName = t.BankAccount.UserId > 0 ? t.BankAccount.User.Firstname + " " + t.BankAccount.User.Lastname : "",
                    price = t.Price,
                    traceCode = t.TraceCode,
                    payDate = t.PayDate
                })
                .ToList()
                .Select(t => new ProposalFilledFormPaymentVM
                {
                    fullName = t.bankTitle + " " + t.accTitle + (!string.IsNullOrEmpty(t.userFullName) ? ("(" + t.userFullName + ")") : ""),
                    payDate = t.payDate.Value,
                    price = t.price,
                    traceCode = t.traceCode
                })
                .ToList()
                ;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void UpdatePaymentInfor(BankAccountFactor foundAccount, string traceNo, int? siteSettingId, DateTime? payDate = null)
        {
            if (foundAccount == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (string.IsNullOrEmpty(traceNo))
                throw BException.GenerateNewException(BMessages.Validation_Error);

            var foundItem = db.BankAccountFactors.Where(t => t.BankAccountId == foundAccount.BankAccountId && t.Type == foundAccount.Type && t.ObjectId == foundAccount.ObjectId && t.CreateDate == foundAccount.CreateDate).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (foundItem.Type == BankAccountFactorType.ProposalFilledForm)
                ProposalFilledFormService.UpdateTraceCode(foundItem.ObjectId, traceNo);
            else if (foundItem.Type == BankAccountFactorType.Wallet)
                WalletTransactionService.Create(foundItem.Price, siteSettingId, foundItem.ObjectId, "افزایش موجودی", traceNo);
            else if (foundItem.Type == BankAccountFactorType.UserRegister)
                UserFilledRegisterFormService.UpdateTraceCode(foundItem.ObjectId, siteSettingId, traceNo);
            else
                throw BException.GenerateNewException(BMessages.No_Imprement_Yet);

            foundItem.TraceCode = traceNo;
            foundItem.IsPayed = true;
            foundItem.PayDate = payDate != null ? payDate.Value : DateTime.Now;
            db.SaveChanges();

        }
    }
}
