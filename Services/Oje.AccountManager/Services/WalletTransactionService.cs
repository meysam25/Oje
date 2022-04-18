using Oje.AccountService.Interfaces;
using Oje.AccountService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
