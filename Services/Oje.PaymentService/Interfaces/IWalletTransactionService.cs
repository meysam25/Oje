using Oje.Infrastructure.Models;
using Oje.PaymentService.Models.View;

namespace Oje.PaymentService.Interfaces
{
    public interface IWalletTransactionService
    {
        GridResultVM<WalletTransactionMainGridResultVM> GetList(WalletTransactionMainGrid searchInput, int? siteSettingId, long? userId);
        void Create(long price, int? siteSettingId, long userId, string description, string traceNo);
        ApiResult GeneratePaymentUrl(WalletTransactionCreateUpdateVM input, int? siteSettingId, long? userId, string url, long agentUserId);
        string GetUserBlance(int? siteSettingId, long? userId);
        ApiResult Pay(WalletTransactionPayVM input, int? siteSettingId, long? userId);
    }
}
