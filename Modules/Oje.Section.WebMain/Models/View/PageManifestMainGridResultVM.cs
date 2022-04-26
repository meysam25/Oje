using System.ComponentModel.DataAnnotations;

namespace Oje.Section.WebMain.Models.View
{
    public class PageManifestMainGridResultVM
    {
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "عنوان صفحه")]
        public string pageTitle { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
