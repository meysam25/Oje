using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class UserNotificationMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public string id { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
        [Display(Name = "توسط کاربر")]
        public string fromUser { get; set; }
        [Display(Name = "برای کاربر")]
        public string toUser { get; set; }
        [Display(Name = "موضوع")]
        public string subject { get; set; }
        [Display(Name = "توضیحات")]
        public string description { get; set; }
        [Display(Name = "تاریخ مشاهده")]
        public string viewDate { get; set; }
        [Display(Name = "مالکیت")]
        public string justMyNotification { get; set; }
        [Display(Name = "لینک")]
        public string link { get; set; }
        [Display(Name = "دیده نشده ها")]
        public string notSeen { get; set; }
    }
}
