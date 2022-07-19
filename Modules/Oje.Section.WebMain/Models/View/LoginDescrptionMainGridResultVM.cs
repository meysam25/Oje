using System.ComponentModel.DataAnnotations;

namespace Oje.Section.WebMain.Models.View
{
    public class LoginDescrptionMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "مسیر")]
        public string url { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
