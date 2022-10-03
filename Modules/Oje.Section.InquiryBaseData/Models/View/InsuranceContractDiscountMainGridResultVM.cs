using System.ComponentModel.DataAnnotations;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class InsuranceContractDiscountMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "شرکت")]
        public string company { get; set; }
        [Display(Name = "قرارداد")]
        public string contract { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "درصد")]
        public int percent { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
