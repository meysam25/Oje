using System.ComponentModel.DataAnnotations;

namespace Oje.Security.Models.View
{
    public class InValidRangeIpMainGridResult
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public string id { get; set; }
        [Display(Name = "ای پی")]
        public string ip { get; set; }
        [Display(Name = "تاریخ")]
        public string lastDate { get; set; }
        [Display(Name = "تعداد")]
        public string count { get; set; }
        [Display(Name = "وضعیت")]
        public string isSuccess { get; set; }
        [Display(Name = "اخرین خطا")]
        public string message { get; set; }
        [Display(Name = "فایر وال؟")]
        public string iB { get; set; }
    }
}
