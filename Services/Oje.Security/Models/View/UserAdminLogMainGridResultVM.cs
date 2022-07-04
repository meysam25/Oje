using System.ComponentModel.DataAnnotations;

namespace Oje.Security.Models.View
{
    public class UserAdminLogMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "نام")]
        public string userFullname { get; set; }
        [Display(Name = "بخش")]
        public string action { get; set; }
        [Display(Name = "وضعیت")]
        public string isSuccess { get; set; }
        [Display(Name = "ای پی")]
        public string ip { get; set; }
        [Display(Name = "تاریخ")]
        public string createDate { get; set; }
        [Display(Name = "مدت زمان اجرا")]
        public string duration { get; set; }
        [Display(Name = "شناسه درخواست")]
        public string rid { get; set; }
    }
}
