using Oje.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace Oje.Section.Question.Models.View
{
    public class UserRegisterFormYourQuestionMainGridResultVM: GlobalGrid
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "فرم")]
        public string form { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
