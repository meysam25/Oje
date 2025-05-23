﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.View
{
    public class CarBodyInquiryVM
    {
        public CarBodyInquiryVM()
        {
            comIds = new();
            exteraQuestions = new();
        }

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
        [Display(Name ="ارزش خودروی شما (ریال)")]
        public long? carValue { get; set; }
        [Display(Name ="ارزش وسایل اضافه (ریال)")]
        public long? exteraValue { get; set; }
        [Display(Name = "سابقه عدم خسارت")]
        public int? noDamageDiscountBody { get; set; }
        [Display(Name = "سابقه عدم خسارت")]
        public string noDamageDiscountBody_Title { get; set; }
        [Display(Name = "سابقه عدم خسارت اضافی")]
        public int? noDamageDiscountBodyExtera { get; set; }
        [Display(Name = "سابقه عدم خسارت اضافی")]
        public string noDamageDiscountBodyExtera_Title { get; set; }
        [Display(Name = "انتخاب روش پرداخت")]
        public int? showStatus { get; set; }
        [Display(Name = "انتخاب روش پرداخت")]
        public string showStatus_Title { get; set; }
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


        public List<int> dynamicCTRLs { get; set; }
        public List<string> dynamicCTRLs_Title { get; set; }
        public List<RightFilterInqueryVM> exteraQuestions { get; set; }
        public List<int> comIds { get; set; }
    }
}
