using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace Oje.Section.Tender.Models.View
{
    public class MyTenderFilledFormMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "بیمه")]
        [IgnoreStringEncode]
        public MyHtmlString insurances { get; set; }
        [Display(Name = "تاریخ")]
        public string createDate { get; set; }
        [Display(Name = "وضعیت انتشار")]
        public bool isPub { get; set; }
        [Display(Name = "وضعیت")]
        public string status { get; set; }
        [Display(Name = "صادر کننده")]
        public string iu { get; set; }
        [Display(Name = "شرکت صادر کننده")]
        public string iuc { get; set; }
    }
}
