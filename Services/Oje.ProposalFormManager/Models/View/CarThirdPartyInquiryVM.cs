using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.View
{
    public class CarThirdPartyInquiryVM
    {
        [Display(Name = "بیمه نامه سال قبل")]
        public int? havePrevInsurance { get; set; }
        [Display(Name = "بیمه نامه سال قبل")]
        public string havePrevInsurance_Title { get; set; }
        [Display(Name = "آیا خودرو صفر کیلومتر است")]
        public int? isNewCar { get; set; }
        [Display(Name = "آیا خودرو صفر کیلومتر است")]
        public string isNewCar_Title { get; set; }
        [Display(Name = "نوع خودرو")]
        public int? vehicleTypeId { get; set; }
        [Display(Name = "نوع خودرو")]
        public string vehicleTypeId_Title { get; set; }
        [Display(Name = "برند خودرو")]
        public int? brandId { get; set; }
        [Display(Name = "برند خودرو")]
        public string brandId_Title { get; set; }
        [Display(Name = "کاربری خودرو")]
        public int? carTypeId { get; set; }
        [Display(Name = "کاربری خودرو")]
        public string carTypeId_Title { get; set; }
        [Display(Name = "سال ساخت")]
        public int? createYear { get; set; }
        [Display(Name = "سال ساخت")]
        public string createYear_Title { get; set; }
        [Display(Name = "خصوصیت خودرو")]
        public int? specId { get; set; }
        [Display(Name = "خصوصیت خودرو")]
        public string specId_Title { get; set; }
        [Display(Name = "سابقه خسارت جانی")]
        public int? bodyDamageHistoryId { get; set; }
        [Display(Name = "سابقه خسارت جانی")]
        public string bodyDamageHistoryId_Title { get; set; }
        [Display(Name = "سابقه خسارت مالی")]
        public int? financialDamageHistoryId { get; set; }
        [Display(Name = "سابقه خسارت مالی")]
        public string financialDamageHistoryId_Title { get; set; }
        [Display(Name = "سابقه خسارت راننده")]
        public int? driverDamageHistoryId { get; set; }
        [Display(Name = "سابقه خسارت راننده")]
        public string driverDamageHistoryId_Title { get; set; }
        [Display(Name = "درصد عدم خسارت جانی (مندرج در بیمه نامه )")]
        public int? bodyNoDamagePercentId { get; set; }
        [Display(Name = "درصد عدم خسارت جانی (مندرج در بیمه نامه )")]
        public string bodyNoDamagePercentId_Title { get; set; }
        [Display(Name = "درصد عدم خسارت راننده ( مندرج در بیمه نامه )")]
        public int? driverNoDamageDiscountHistory { get; set; }
        [Display(Name = "درصد عدم خسارت راننده ( مندرج در بیمه نامه )")]
        public string driverNoDamageDiscountHistory_Title { get; set; }
        [Display(Name = "انتخاب روش پرداخت")]
        public int? showStatus { get; set; }
        [Display(Name = "انتخاب روش پرداخت")]
        public string showStatus_Title { get; set; }
        [Display(Name = "تاریخ شروع بیمه نامه قبل")]
        public string prevStartDate { get; set; }
        [Display(Name = "تاریخ اتمام بیمه نامه قبلی")]
        public string prevEndDate { get; set; }
        [Display(Name = "دارای اطاق بار")]
        public bool? hasRoom { get; set; }
        [Display(Name = "دارای اطاق بار")]
        public string hasRoom_Title { get; set; }
        [Display(Name = "تخفیف تفاهم نامه")]
        public int? discountContractId { get; set; }
        [Display(Name = "تخفیف تفاهم نامه")]
        public string discountContractId_Title { get; set; }
        [Display(Name = "خرید روزانه")]
        public int? dayLimitation { get; set; }
        [Display(Name = "خرید روزانه")]
        public string dayLimitation_Title { get; set; }
        [Display(Name = "کد تخفیف")]
        public string discountCode { get; set; }


        public List<int> coverIds { get; set; }
        public List<string> coverIds_Title { get; set; }
        public List<int> dynamicCTRLs { get; set; }
        public List<string> dynamicCTRLs_Title { get; set; }
        public List<RightFilterInqueryVM> exteraQuestions { get; set; }
        public List<int> comIds { get; set; }
    }
}
