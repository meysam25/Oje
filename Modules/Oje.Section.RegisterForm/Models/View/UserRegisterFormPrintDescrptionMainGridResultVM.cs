using System.ComponentModel.DataAnnotations;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormPrintDescrptionMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "فرم پیشنهاد")]
        public string fTitle { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
