using System.ComponentModel.DataAnnotations;

namespace Oje.Section.Blog.Models.View
{
    public class BlogReviewMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public string id { get; set; }
        [Display(Name = "عنوان بلاگ")]
        public string blogTitle { get; set; }
        [Display(Name = "نام")]
        public string userFullname { get; set; }
        [Display(Name = "همراه")]
        public string userMobile { get; set; }
        [Display(Name = "ایمیل")]
        public string userEmail { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "فعال شده ؟")]
        public bool iA{ get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
