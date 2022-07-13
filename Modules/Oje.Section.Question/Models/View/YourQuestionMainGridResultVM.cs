using System.ComponentModel.DataAnnotations;

namespace Oje.Section.Question.Models.View
{
    public class YourQuestionMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
