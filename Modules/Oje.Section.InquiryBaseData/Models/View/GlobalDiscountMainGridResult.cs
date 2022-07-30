using System.ComponentModel.DataAnnotations;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class GlobalDiscountMainGridResult
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "شرکت")]
        public string company { get; set; }
        [Display(Name = "فرم پیشنهاد")]
        public string ppfTitle { get; set; }
        [Display(Name = "کاربر")]
        public string createUser { get; set; }
        [Display(Name = "تاریخ  ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "از تاریخ")]
        public string fromDate { get; set; }
        [Display(Name = "تا تاریخ")]
        public string toDate { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
