using System.ComponentModel.DataAnnotations;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserFilledRegisterFormMainGridResultVM
    {
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "نام کاربری")]
        public string username { get; set; }
        [Display(Name = "نام")]
        public string firstname { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string lastname { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "نوع")]
        public string formTitle { get; set; }
        [Display(Name = "مبلغ")]
        public string price { get; set; }
        [Display(Name = "وضعیت پرداخت")]
        public string isPayed { get; set; }
        [Display(Name = "کد پیگیری پرداخت")]
        public string traceCode { get; set; }
        [Display(Name = "انجام شده؟")]
        public string isDone { get; set; }
        [Display(Name = "کاربر معرف")]
        public string refferUser { get; set; }
    }
}
