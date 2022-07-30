using System.ComponentModel.DataAnnotations;

namespace Oje.AccountService.Models.View
{
    public class UserRequestActionMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public string id { get; set; }
        [Display(Name = "کاربر")]
        public string user { get; set; }
        [Display(Name = "نقش")]
        public string role { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "بخش")]
        public string action { get; set; }
    }
}
