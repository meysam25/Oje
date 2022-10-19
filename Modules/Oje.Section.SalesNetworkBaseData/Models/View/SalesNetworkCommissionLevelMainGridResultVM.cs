using System.ComponentModel.DataAnnotations;

namespace Oje.Section.SalesNetworkBaseData.Models.View
{
    public class SalesNetworkCommissionLevelMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "شبکه فروش")]
        public string snId { get; set; }
        [Display(Name = "لول")]
        public string step { get; set; }
        [Display(Name = "نرخ")]
        public string rate { get; set; }
        [Display(Name = "وبسایت")]
        public string siteTitleMN2 { get; set; }
    }
}
