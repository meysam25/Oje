using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.DB;
using Oje.PaymentService.Models.View;
using Oje.PaymentService.Services.EContext;

namespace Oje.PaymentService.Services
{
    public class WalletTransactionService : IWalletTransactionService
    {
        readonly IProposalFilledFormService ProposalFilledFormService = null;
        readonly PaymentDBContext db = null;

        static object lockObj = null;

        public WalletTransactionService
            (
                PaymentDBContext db,
                IProposalFilledFormService ProposalFilledFormService
            )
        {
            this.db = db;
            this.ProposalFilledFormService = ProposalFilledFormService;
        }

        public void Create(long price, int? siteSettingId, long userId, string description, string traceNo)
        {
            if (price > 0 && siteSettingId.ToIntReturnZiro() > 0 && userId.ToLongReturnZiro() > 0)
            {
                db.Entry(new WalletTransaction()
                {
                    CreateDate = DateTime.Now,
                    Descrption = description,
                    Price = price,
                    SiteSettingId = siteSettingId.Value,
                    UserId = userId,
                    TraceNo = traceNo
                }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.SaveChanges();
            }
        }

        public long CreateMinusPrice(long price, int? siteSettingId, long userId, string description, string traceNo)
        {
            if (price < 0 && siteSettingId.ToIntReturnZiro() > 0 && userId.ToLongReturnZiro() > 0)
            {
                var newItem = new WalletTransaction()
                {
                    CreateDate = DateTime.Now,
                    Descrption = description,
                    Price = price,
                    SiteSettingId = siteSettingId.Value,
                    UserId = userId,
                    TraceNo = traceNo
                };
                db.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.SaveChanges();

                return newItem.Id;
            }

            return 0;
        }

        public ApiResult GeneratePaymentUrl(WalletTransactionCreateUpdateVM input, int? siteSettingId, long? userId, string url, long agentUserId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.price.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            if (agentUserId <= 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);


            return new ApiResult()
            {
                isSuccess = true,
                message = BMessages.Operation_Was_Successfull.GetEnumDisplayName(),
                data =
                new
                {
                    action = "redirect",
                    url = "/Payment/Payment/GetWayList?status=" + System.Net.WebUtility.UrlEncode(("3," + userId + "," + input.price.Value + "," + url + "," + agentUserId).Encrypt2())
                }
            };
        }

        public GridResultVM<WalletTransactionMainGridResultVM> GetList(WalletTransactionMainGrid searchInput, int? siteSettingId, long? userId)
        {
            searchInput = searchInput ?? new WalletTransactionMainGrid();

            var quiryResult = db.WalletTransactions.Where(t => t.SiteSettingId == siteSettingId && t.UserId == userId);

            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (searchInput.price != null && searchInput.price != 0)
                quiryResult = quiryResult.Where(t => t.Price == searchInput.price);
            if (!string.IsNullOrEmpty(searchInput.descrption))
                quiryResult = quiryResult.Where(t => t.Descrption.Contains(searchInput.descrption));

            int row = searchInput.skip;

            return new GridResultVM<WalletTransactionMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    createDate = t.CreateDate,
                    price = t.Price,
                    descrption = t.Descrption
                })
                .ToList()
                .Select(t => new WalletTransactionMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    createDate = t.createDate.ToFaDate() + " " + t.createDate.ToString("HH:mm"),
                    descrption = t.descrption,
                    price = t.price.ToString("###,###")
                })
                .ToList()
            };
        }

        public string GetUserBlance(int? siteSettingId, long? userId)
        {
            var result = db.WalletTransactions.Where(t => t.SiteSettingId == siteSettingId && t.UserId == userId).Select(t => t.Price).Sum();
            return "موجودی " + (result > 0 ? result.ToString("###,###") : "0") + " ریال";
        }

        public ApiResult Pay(WalletTransactionPayVM input, int? siteSettingId, long? userId)
        {
            lockObj = lockObj ?? new object();

            payValidation(input, siteSettingId, userId);

            lock (lockObj)
            {
                if (input.type == Infrastructure.Enums.BankAccountFactorType.ProposalFilledForm)
                {
                    var ppfPrice = ProposalFilledFormService.ValidateForPayment(siteSettingId, input.id.ToLongReturnZiro());
                    var userBalance = db.WalletTransactions.Where(t => t.UserId == userId && t.SiteSettingId == siteSettingId).Select(t => t.Price).Sum();
                    if (userBalance < ppfPrice)
                        throw BException.GenerateNewException(BMessages.Inventory_Is_Not_Enough);
                    using (var tr = db.Database.BeginTransaction())
                    {
                        var wtrId = CreateMinusPrice(ppfPrice * -1, siteSettingId, userId.Value, "پرداخت بابت بیمه نامه با شناسه " + input.id, null);
                        if (wtrId > 0)
                        {
                            ProposalFilledFormService.UpdateTraceCode(input.id.ToLongReturnZiro(), wtrId + "");
                            tr.Commit();
                            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
                        }
                    }
                }
            }

            return ApiResult.GenerateNewResult(false, BMessages.UnknownError);
        }

        private void payValidation(WalletTransactionPayVM input, int? siteSettingId, long? userId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (string.IsNullOrEmpty(input.id))
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input.type != Infrastructure.Enums.BankAccountFactorType.ProposalFilledForm)
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }
    }
}
