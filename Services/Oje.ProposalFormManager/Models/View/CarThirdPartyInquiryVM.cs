using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Oje.ProposalFormService.Models.View
{
    public class CarThirdPartyInquiryVM
    {
        [Display(Name = "بیمه نامه سال قبل", Description = "carSpecifications")]
        public int? havePrevInsurance { get; set; }
        [Display(Name = "بیمه نامه سال قبل", Description = "carSpecifications")]
        public string havePrevInsurance_Title { get; set; }
        [Display(Name = "آیا خودرو صفر کیلومتر است", Description = "carSpecifications")]
        public bool? isNewCar { get; set; }
        [Display(Name = "آیا خودرو صفر کیلومتر است", Description = "carSpecifications")]
        public string isNewCar_Title { get; set; }
        [Display(Name = "نوع خودرو", Description = "carSpecifications")]
        public int? vehicleTypeId { get; set; }
        [Display(Name = "نوع خودرو", Description = "carSpecifications")]
        public string vehicleTypeId_Title { get; set; }
        [Display(Name = "برند خودرو", Description = "carSpecifications")]
        public int? brandId { get; set; }
        [Display(Name = "برند خودرو", Description = "carSpecifications")]
        public string brandId_Title { get; set; }
        [Display(Name = "کاربری خودرو", Description = "carSpecifications")]
        public int? carTypeId { get; set; }
        [Display(Name = "کاربری خودرو", Description = "carSpecifications")]
        public string carTypeId_Title { get; set; }
        [Display(Name = "سال ساخت", Description = "carSpecifications")]
        public int? createYear { get; set; }
        [Display(Name = "سال ساخت", Description = "carSpecifications")]
        public string createYear_Title { get; set; }
        [Display(Name = "خصوصیت خودرو", Description = "carSpecifications")]
        public int? specId { get; set; }
        [Display(Name = "خصوصیت خودرو", Description = "carSpecifications")]
        public string specId_Title { get; set; }
        [Display(Name = "سابقه خسارت جانی", Description = "carSpecifications")]
        public int? bodyDamageHistoryId { get; set; }
        [Display(Name = "سابقه خسارت جانی", Description = "carSpecifications")]
        public string bodyDamageHistoryId_Title { get; set; }
        [Display(Name = "سابقه خسارت مالی", Description = "carSpecifications")]
        public int? financialDamageHistoryId { get; set; }
        [Display(Name = "سابقه خسارت مالی", Description = "carSpecifications")]
        public string financialDamageHistoryId_Title { get; set; }
        [Display(Name = "سابقه خسارت راننده", Description = "carSpecifications")]
        public int? driverDamageHistoryId { get; set; }
        [Display(Name = "سابقه خسارت راننده", Description = "carSpecifications")]
        public string driverDamageHistoryId_Title { get; set; }
        [Display(Name = "درصد عدم خسارت جانی (مندرج در بیمه نامه )", Description = "carSpecifications")]
        public int? bodyNoDamagePercentId { get; set; }
        [Display(Name = "درصد عدم خسارت جانی (مندرج در بیمه نامه )", Description = "carSpecifications")]
        public string bodyNoDamagePercentId_Title { get; set; }
        [Display(Name = "درصد عدم خسارت راننده ( مندرج در بیمه نامه )", Description = "carSpecifications")]
        public int? driverNoDamageDiscountHistory { get; set; }
        [Display(Name = "درصد عدم خسارت راننده ( مندرج در بیمه نامه )", Description = "carSpecifications")]
        public string driverNoDamageDiscountHistory_Title { get; set; }
        [Display(Name = "انتخاب روش پرداخت")]
        public int? showStatus { get; set; }
        [Display(Name = "انتخاب روش پرداخت")]
        public string showStatus_Title { get; set; }
        [Display(Name = "تاریخ شروع بیمه نامه قبل", Description = "carSpecifications")]
        public string prevStartDate { get; set; }
        [Display(Name = "تاریخ اتمام بیمه نامه قبلی", Description = "carSpecifications")]
        public string prevEndDate { get; set; }
        [Display(Name = "دارای اطاق بار", Description = "carSpecifications")]
        public bool? hasRoom { get; set; }
        [Display(Name = "دارای اطاق بار", Description = "carSpecifications")]
        public string hasRoom_Title { get; set; }
        [Display(Name = "تخفیف تفاهم نامه", Description = "carSpecifications")]
        public int? discountContractId { get; set; }
        [Display(Name = "تخفیف تفاهم نامه", Description = "carSpecifications")]
        public string discountContractId_Title { get; set; }
        [Display(Name = "خرید روزانه", Description = "carSpecifications")]
        public int? dayLimitation { get; set; }
        [Display(Name = "خرید روزانه", Description = "carSpecifications")]
        public string dayLimitation_Title { get; set; }
        [Display(Name = "کد تخفیف", Description = "carSpecifications")]
        public string discountCode { get; set; }


        public List<int> coverIds { get; set; }
        public List<string> coverIds_Title { get; set; }
        public List<int> dynamicCTRLs { get; set; }
        public List<string> dynamicCTRLs_Title { get; set; }
        public List<RightFilterInqueryVM> exteraQuestions { get; set; }
        public List<int> comIds { get; set; }
    }
}
