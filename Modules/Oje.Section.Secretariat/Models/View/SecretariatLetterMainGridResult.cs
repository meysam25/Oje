using System.ComponentModel.DataAnnotations;

namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatLetterMainGridResult
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "زیرعنوان")]
        public string subTitle { get; set; }
        [Display(Name = "موضوع")]
        public string subject { get; set; }
        [Display(Name = "کاربر")]
        public string user { get; set; }
        [Display(Name = "امضا کننده")]
        public string sUser { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "تایید شده؟")]
        public bool isC { get; set; }
    }
}
