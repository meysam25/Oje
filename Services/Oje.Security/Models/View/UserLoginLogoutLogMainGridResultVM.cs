using System.ComponentModel.DataAnnotations;

namespace Oje.Security.Models.View
{
    public class UserLoginLogoutLogMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public string id { get; set; }
        [Display(Name = "نام")]
        public string userfullname { get; set; }
        [Display(Name = "تاریخ")]
        public string createDate { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
        [Display(Name = "ای پی")]
        public string ip { get; set; }
        [Display(Name = "پیغام")]
        public string message { get; set; }
        [Display(Name = "موفقیت؟")]
        public string isSuccess { get; set; }
    }
}
