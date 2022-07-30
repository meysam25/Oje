using Oje.Infrastructure.Enums;
using Oje.PaymentService.Interfaces;

namespace Oje.PaymentService.Services
{
    public class BankAccountDetectorService : IBankAccountDetectorService
    {
        readonly IBankAccountSizpayService BankAccountSizpayService = null;
        readonly IBankAccountSadadService BankAccountSadadService = null;
        public BankAccountDetectorService
            (
                IBankAccountSizpayService BankAccountSizpayService,
                IBankAccountSadadService BankAccountSadadService
            )
        {
            this.BankAccountSizpayService = BankAccountSizpayService;
            this.BankAccountSadadService = BankAccountSadadService;
        }

        public BankAccountType GetByType(int bankAccountId, int? siteSettingId)
        {
            if (BankAccountSizpayService.Exist(bankAccountId, siteSettingId))
                return BankAccountType.sizpay;
            else if (BankAccountSadadService.Exist(bankAccountId, siteSettingId))
                return BankAccountType.sadad;
            else
                return BankAccountType.titec;

        }
    }
}
