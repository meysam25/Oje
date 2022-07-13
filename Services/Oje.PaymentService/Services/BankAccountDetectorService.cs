using Oje.Infrastructure.Enums;
using Oje.PaymentService.Interfaces;

namespace Oje.PaymentService.Services
{
    public class BankAccountDetectorService: IBankAccountDetectorService
    {
        readonly IBankAccountSizpayService BankAccountSizpayService = null;
        public BankAccountDetectorService
            (
                IBankAccountSizpayService BankAccountSizpayService
            )
        {
            this.BankAccountSizpayService = BankAccountSizpayService;
        }

        public BankAccountType GetByType(int bankAccountId, int? siteSettingId)
        {
            if (BankAccountSizpayService.Exist(bankAccountId, siteSettingId))
                return BankAccountType.sizpay;
            else
                return BankAccountType.titec;

        }
    }
}
