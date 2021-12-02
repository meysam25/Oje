using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Oje.FireInsuranceManager.Models.View
{
    public class FireInsuranceInquiryVM
    {
        [Display(Name = "استان")]
        public int? provinceId { get; set; }
        [Display(Name = "استان")]
        public string provinceId_Title { get; set; }
        [Display(Name = "شهر")]
        public int? cityId { get; set; }
        [Display(Name = "شهر")]
        public string cityId_Title { get; set; }
        [Display(Name = "ارزش هر متر مربع")]
        public int? value { get; set; }
        [Display(Name = "ارزش هر متر مربع")]
        public string value_Title { get; set; }
        [Display(Name = "متراژ بنا")]
        public int? metrazh { get; set; }
        [Display(Name = "نوع ساختمان")]
        public int? typeId { get; set; }
        [Display(Name = "نوع ساختمان")]
        public string typeId_Title { get; set; }
        [Display(Name = "نوع اسکلت")]
        public int? bodyId { get; set; }
        [Display(Name = "نوع اسکلت")]
        public string bodyId_Title { get; set; }
        [Display(Name = "لوازم منزل")]
        public long? assetValue { get; set; }
        [Display(Name = "روش پرداخت")]
        public int? showStatus { get; set; }
        [Display(Name = "روش پرداخت")]
        public string showStatus_Title { get; set; }
        [Display(Name = "بیمه نامه روزانه")]
        public int? dayLimitation { get; set; }
        [Display(Name = "بیمه نامه روزانه")]
        public string dayLimitation_Title { get; set; }
        [Display(Name = "کد تخفیف")]
        public string discountCode { get; set; }
        [Display(Name = "تخفیف تفاهم نامه")]
        public int? discountContractId { get; set; }
        [Display(Name = "تخفیف تفاهم نامه")]
        public string discountContractId_Title { get; set; }
        [Display(Name = "سن بنا")]
        public int? buildingAge { get; set; }
        [Display(Name = "سن بنا")]
        public string buildingAge_Title { get; set; }
        [Display(Name = "شرکت")]
        public List<int> comIds { get; set; }
    }
}
