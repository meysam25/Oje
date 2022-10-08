using Oje.Infrastructure.Models;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountCreateUpdateVM : GlobalSiteSetting
    {
        public int? id { get; set; }
        public int? bankId { get; set; }
        public string title { get; set; }
        public long? cardNo { get; set; }
        public string shabaNo { get; set; }
        public long hesabNo { get; set; }
        public long? userId { get; set; }
        public bool? isForPayment { get; set; }
        public bool? isActive { get; set; }
    }
}
