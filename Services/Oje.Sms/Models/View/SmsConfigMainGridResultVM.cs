using System.ComponentModel.DataAnnotations;

namespace Oje.Sms.Models.View
{
    public class SmsConfigMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "نام کاربری")]
        public string smsUsername { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "شماره")]
        public string ph { get;  set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
