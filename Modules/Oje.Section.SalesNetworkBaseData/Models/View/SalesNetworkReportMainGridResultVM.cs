using System.ComponentModel.DataAnnotations;

namespace Oje.Section.SalesNetworkBaseData.Models.View
{
    public class SalesNetworkReportMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public string id { get; set; }
        [Display(Name = "نام")]
        public string firstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string lastName { get; set; }
        [Display(Name = "سطح")]
        public string level { get; set; }
        [Display(Name = "پورسانت")]
        public string commission { get; set; }
        [Display(Name = "پورسانت")]
        public long commissionNumber { get; set; }
        [Display(Name = "نوع شخص")]
        public string typeOfCalc { get; set; }
        [Display(Name = "جمع کل فروش")]
        public string saleSum { get; set; }
        [Display(Name = "جمع کل فروش")]
        public long saleSumNumber { get; set; }
        [Display(Name = "نقش")]
        public string role { get; set; }
    }
}
