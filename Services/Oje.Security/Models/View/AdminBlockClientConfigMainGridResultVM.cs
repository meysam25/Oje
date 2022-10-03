
using System.ComponentModel.DataAnnotations;

namespace Oje.Security.Models.View
{
    public class AdminBlockClientConfigMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "بخش")]
        public string action { get; set; }
        [Display(Name = "حداکثر")]
        public string value { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
