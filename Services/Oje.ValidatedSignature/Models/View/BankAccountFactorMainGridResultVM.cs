using System.ComponentModel.DataAnnotations;

namespace Oje.ValidatedSignature.Models.View
{
    public class BankAccountFactorMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public string id { get; set; }
        [Display(Name = "عنوان حساب")]
        public string bcTitle { get; set; }
        [Display(Name = "شناسه مرتبط")]
        public string objId { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "کاربر")]
        public string user { get; set; }
        [Display(Name = "مبلغ")]
        public string price { get; set; }
        [Display(Name = "پرداخت")]
        public string isPayed { get; set; }
        [Display(Name = "کد پیگیری")]
        public string traceCode { get; set; }
        [Display(Name = "وب سایت")]
        public string website { get; set; }
        [Display(Name = "مجاز")]
        public string isValid { get; set; }
    }
}
