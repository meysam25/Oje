using System.ComponentModel.DataAnnotations;

namespace Oje.Section.GlobalForms.Models.View
{
    public class GeneralFormStatusGridColumnMainGridResult
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "فرم")]
        public string fgTitle { get; set; }
        [Display(Name = "وضعیت")]
        public string sTitle { get; set; }
        [Display(Name = "کلید")]
        public string kTitle { get; set; }
    }
}
