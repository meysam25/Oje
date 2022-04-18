using System.ComponentModel.DataAnnotations;

namespace Oje.PaymentService.Models.View
{
    public class WalletTransactionMainGridResultVM
    {
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "مبلغ")]
        public string price { get; set; }
        [Display(Name = "توضیحات")]
        public string descrption { get; set; }
    }
}
