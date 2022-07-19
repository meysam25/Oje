using System.ComponentModel.DataAnnotations;

namespace Oje.Section.WebMain.Models.View
{
    public class PageSliderMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "صفحه")]
        public string page { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
