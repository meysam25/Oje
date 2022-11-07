using System.ComponentModel.DataAnnotations;

namespace Oje.Section.GlobalForms.Models.View
{
    public class GeneralFormRequiredDocumentMainGridResult
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "فرم")]
        public string fId { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "نام")]
        public string name { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
