using System.ComponentModel.DataAnnotations;

namespace Oje.Sms.Models.View
{
    public class SmsTemplateMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string subject { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
        [Display(Name = "برای")]
        public string pffUserType { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
