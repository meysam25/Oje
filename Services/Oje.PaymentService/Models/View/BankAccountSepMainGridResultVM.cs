using System.ComponentModel.DataAnnotations;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountSepMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "حساب بانکی")]
        public string bankAcount { get; set; }
        [Display(Name = "ترمینال")]
        public string terminalId { get; set; }
    }
}
