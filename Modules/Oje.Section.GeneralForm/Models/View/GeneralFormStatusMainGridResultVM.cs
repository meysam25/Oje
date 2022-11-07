using System.ComponentModel.DataAnnotations;

namespace Oje.Section.GlobalForms.Models.View
{
    public class GeneralFormStatusMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "فرم")]
        public string fid { get; set; }
    }
}
