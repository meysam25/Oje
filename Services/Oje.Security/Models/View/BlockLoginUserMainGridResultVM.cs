
using System.ComponentModel.DataAnnotations;

namespace Oje.Security.Models.View
{
    public class BlockLoginUserMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "تاریخ شروع")]
        public string startDate { get; set; }
        [Display(Name = "تاریخ پایان")]
        public string endDate { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
