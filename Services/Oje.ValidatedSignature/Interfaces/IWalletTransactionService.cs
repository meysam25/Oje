using Oje.Infrastructure.Models;
using Oje.ValidatedSignature.Models.View;

namespace Oje.ValidatedSignature.Interfaces
{
    public interface IWalletTransactionService
    {
        object GetBy(long? id);
        GridResultVM<WalletTransactionMainGridResultVM> GetList(WalletTransactionMainGrid searchInput);
    }
}
