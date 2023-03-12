using Oje.AccountService.Interfaces;
using Oje.AccountService.Services.EContext;

namespace Oje.AccountService.Services
{
    public class WalletTransactionService: IWalletTransactionService
    {
        readonly AccountDBContext db = null;
        public WalletTransactionService(AccountDBContext db)
        {
            this.db = db;
        }
    }
}
