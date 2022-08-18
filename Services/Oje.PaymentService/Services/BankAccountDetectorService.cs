using Oje.Infrastructure.Enums;
using Oje.PaymentService.Interfaces;

namespace Oje.PaymentService.Services
{
    public class BankAccountDetectorService : IBankAccountDetectorService
    {
        readonly IBankAccountSizpayService BankAccountSizpayService = null;
        readonly IBankAccountSadadService BankAccountSadadService = null;
        readonly IBankAccountSepService BankAccountSepService = null;
        public BankAccountDetectorService
            (
                IBankAccountSizpayService BankAccountSizpayService,
                IBankAccountSadadService BankAccountSadadService,
                IBankAccountSepService BankAccountSepService
            )
        {
            this.BankAccountSizpayService = BankAccountSizpayService;
            this.BankAccountSadadService = BankAccountSadadService;
            this.BankAccountSepService = BankAccountSepService;
        }

        public BankAccountType GetByType(int bankAccountId, int? siteSettingId)
        {
            if (BankAccountSizpayService.Exist(bankAccountId, siteSettingId))
                return BankAccountType.sizpay;
            else if (BankAccountSadadService.Exist(bankAccountId, siteSettingId))
                return BankAccountType.sadad;
            else if (BankAccountSepService.Exist(bankAccountId, siteSettingId))
                return BankAccountType.Sep;
            else
                return BankAccountType.titec;

        }
    }
}
