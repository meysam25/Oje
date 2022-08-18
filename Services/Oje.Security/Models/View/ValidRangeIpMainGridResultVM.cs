using System.ComponentModel.DataAnnotations;

namespace Oje.Security.Models.View
{
    public class ValidRangeIpMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "از ای پی")]
        public string fromIp { get; set; }
        [Display(Name = "تا ای پی")]
        public string toIp { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
