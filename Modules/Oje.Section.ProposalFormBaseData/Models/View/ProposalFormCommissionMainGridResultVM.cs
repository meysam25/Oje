using System.ComponentModel.DataAnnotations;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class ProposalFormCommissionMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "فرم پیشنهاد")]
        public string ppf { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "از مبلغ")]
        public string fPrice { get; set; }
        [Display(Name = "تا مبلغ")]
        public string tPrice { get; set; }
        [Display(Name = "نرخ")]
        public string rate { get; set; }
    }
}
