using System.ComponentModel.DataAnnotations;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractUserBaseInsuranceMainGridResultVM
    {
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "کد")]
        public string code { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
