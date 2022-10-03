using System.ComponentModel.DataAnnotations;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormCompanyMainGridResultVM
    {
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شرکت")]
        public string company { get; set; }
        [Display(Name = "فرم ثبت نام")]
        public string form { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
