using System.ComponentModel.DataAnnotations;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "بانک")]
        public string bankId { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "نام کاربر")]
        public string userfullanme { get; set; }
        [Display(Name = "شماره کارت")]
        public string cardNo { get; set; }
        [Display(Name = "شماره حساب")]
        public string hesabNo { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
