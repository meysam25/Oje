using System.ComponentModel.DataAnnotations;

namespace Oje.AccountService.Models.View
{
    public class AdminUserGridResult
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "کاربر والد")]
        public string parent { get; set; }
        [Display(Name = "نام کاربری")]
        public string username { get; set; }
        [Display(Name = "نام")]
        public string fistname { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string lastname { get; set; }
        [Display(Name = "همراه")]
        public string mobile { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "نقش")]
        public string roleIds { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
