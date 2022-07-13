using System.ComponentModel.DataAnnotations;

namespace Oje.Section.WebMain.Models.View
{
    public class ShortLinkMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "کد")]
        public string code { get; set; }
        [Display(Name = "لینک")]
        public string link { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
