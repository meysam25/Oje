using System.ComponentModel.DataAnnotations;

namespace Oje.Sms.Models.View
{
    public class SmsValidationHistoryMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public string id { get; set; }
        [Display(Name = "ای پی")]
        public string ip { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
        [Display(Name = "همراه")]
        public string mobile { get; set; }
        [Display(Name = "تعداد اشتباه")]
        public string invalidCount { get; set; }
        [Display(Name = "استفاده شده")]
        public string isUsed { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
