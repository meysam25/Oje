using System.ComponentModel.DataAnnotations;

namespace Oje.AccountService.Models.View
{
    public class HolydayMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "تاریخ")]
        public string targetDate { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
