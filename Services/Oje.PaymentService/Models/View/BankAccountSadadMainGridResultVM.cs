using System.ComponentModel.DataAnnotations;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountSadadMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "حساب بانکی")]
        public string bankAcount { get; set; }
        [Display(Name = "شماره پذیرنده")]
        public string merchantId { get; set; }
        [Display(Name = "شناسه ترمینال")]
        public string terminalId { get; set; }
    }
}
