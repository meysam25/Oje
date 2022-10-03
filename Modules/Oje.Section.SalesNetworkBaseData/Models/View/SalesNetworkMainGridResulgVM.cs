using System.ComponentModel.DataAnnotations;

namespace Oje.Section.SalesNetworkBaseData.Models.View
{
    public class SalesNetworkMainGridResulgVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "کاربر")]
        public string createUser { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "فرم پیشنهاد")]
        public string ppfIds { get; set; }
        [Display(Name = "شرکت")]
        public string cIds { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
        [Display(Name = "نوع محاسبه")]
        public string calceType { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
