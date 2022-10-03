using System.ComponentModel.DataAnnotations;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountSizpayMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "شناسه ترمینال")]
        public string terminalId { get; set; }
        [Display(Name = "شناسه پذیرنده")]
        public string merchandId { get; set; }
        [Display(Name = "شماره شبا")]
        public string shbaNo { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
