using System.ComponentModel.DataAnnotations;

namespace Oje.AccountService.Models.View
{
    public class UserMessageMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "نام کاربر")]
        public string userfullname { get; set; }
        [Display(Name = "اخرین زمان پاسخ")]
        public string lastAnswerDate { get; set; }
    }
}
