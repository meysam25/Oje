using System.ComponentModel.DataAnnotations;

namespace Oje.Section.Tender.Models.View
{
    public class TenderFilledFormPriceMainGridResultVM
    {
        public TenderFilledFormPriceMainGridResultVM()
        {
            desc = "";
        }
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "بیمه")]
        public string insurance { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "شرکت")]
        public string company { get; set; }
        [Display(Name = "کاربر")]
        public string user { get; set; }
        [Display(Name = "مبلغ")]
        public string price { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "شناسه مناقصه")]
        public long fid { get; set; }
        [Display(Name = "توضیحات")]
        public string desc { get; set; }
        [Display(Name = "مدرک")]
        public string downloadFileUrl { get; set; }
        [Display(Name = "انتشار")]
        public bool isPub { get; set; }
    }
}
