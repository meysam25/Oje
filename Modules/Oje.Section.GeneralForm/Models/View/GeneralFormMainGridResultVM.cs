using System.ComponentModel.DataAnnotations;

namespace Oje.Section.GlobalForms.Models.View
{
    public class GeneralFormMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "نام")]
        public string name { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "تنظیمات")]
        public string setting { get; set; }
    }
}
