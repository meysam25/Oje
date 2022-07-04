using System.ComponentModel.DataAnnotations;

namespace Oje.Security.Models.View
{
    public class DebugEmailReceiverMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "ایمیل")]
        public string email { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
