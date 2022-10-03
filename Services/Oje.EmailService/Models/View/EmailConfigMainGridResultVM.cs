using System.ComponentModel.DataAnnotations;

namespace Oje.EmailService.Models.View
{
    public class EmailConfigMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "نام کاربری")]
        public string eUsername { get; set; }
        [Display(Name = "پورت")]
        public int? smtpPort { get; set; }
        [Display(Name = "هاست")]
        public string smtpHost { get; set; }
        [Display(Name = "اس اس ال")]
        public string enableSsl { get; set; }
        [Display(Name = "زمان انصراف")]
        public int timeout { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
