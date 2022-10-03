using System.ComponentModel.DataAnnotations;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractInsuranceContractTypeMaxPriceMainGridResultVM 
    {
        [Display(Name = "شناسه")]
        public string id { get; set; }
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "نوع")]
        public string typeId { get; set; }
        [Display(Name = "قرارداد")]
        public string cid { get; set; }
        [Display(Name = "مبلغ")]
        public string price { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
