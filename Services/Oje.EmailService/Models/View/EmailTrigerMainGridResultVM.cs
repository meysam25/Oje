using System.ComponentModel.DataAnnotations;

namespace Oje.EmailService.Models.View
{
    public class EmailTrigerMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
        [Display(Name = "نقش")]
        public string roleName { get; set; }
        [Display(Name = "کاربر")]
        public string userName { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
