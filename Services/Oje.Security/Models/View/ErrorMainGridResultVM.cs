using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace Oje.Security.Models.View
{
    public class ErrorMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "کاربر")]
        public string userFullname { get; set; }
        [Display(Name = "ای پی")]
        public string ip { get; set; }
        [Display(Name = "تاریخ")]
        public string createDate { get; set; }
        [Display(Name = "توضیحات")]
        [IgnoreStringEncode]
        public MyHtmlString description { get; set; }
        [Display(Name = "خط خطا ها")]
        [IgnoreStringEncode]
        public MyHtmlString lineNumber { get; set; }
        [Display(Name = "کلاس")]
        [IgnoreStringEncode]
        public MyHtmlString fileName { get; set; }
        [Display(Name = "کد")]
        public string bMessageCode { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
    }
}
