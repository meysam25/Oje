using System.ComponentModel.DataAnnotations;

namespace Oje.Section.CarThirdBaseData.Models.View
{
    public class ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "مبلغ")]
        public string price { get; set; }
        [Display(Name = "درصد")]
        public string percent { get; set; }
        [Display(Name = "عنوان نوع خودرو")]
        public string ctId { get; set; }
        [Display(Name = "شرکت")]
        public string comId { get; set; }
    }
}
