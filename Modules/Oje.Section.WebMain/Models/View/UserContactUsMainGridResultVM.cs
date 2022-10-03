using System.ComponentModel.DataAnnotations;

namespace Oje.Section.WebMain.Models.View
{
    public class UserContactUsMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public string id { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "نام")]
        public string fullname { get; set; }
        [Display(Name = "شماره تماس")]
        public string tell { get; set; }
        [Display(Name = "ایمیل")]
        public string email { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
