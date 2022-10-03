using System.ComponentModel.DataAnnotations;

namespace Oje.Section.Ticket.Models.View
{
    public class TicketUserAdminMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "پاسخ داده شده")]
        public string isAnswer { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "تاریخ به روز رسانی")]
        public string updateDate { get; set; }
        [Display(Name = "گروه بندی")]
        public string categoryTitle { get; set; }
        [Display(Name = "نام کاربر")]
        public string userfullname { get; set; }
        [Display(Name = "کاربر پاسخ دهنده")]
        public string updateUserFullname { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
