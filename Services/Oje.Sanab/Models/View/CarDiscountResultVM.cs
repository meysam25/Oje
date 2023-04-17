using System.ComponentModel.DataAnnotations;

namespace Oje.Sanab.Models.View
{
    public class CarDiscountResultVM
    {
        [Display(Name = "تعدادمحور")]
        public string AxelNo { get; set; }
        [Display(Name = "تعدادچرخ")]
        public string WheelNo { get; set; }
        [Display(Name = "رفیت (تعداد سرنشین) وسیله نقلیه")]
        public string Capacity { get; set; }
        [Display(Name = "تعداد سیلندر وسیله نقلیه")]
        public string CylinderNo { get; set; }
        [Display(Name = "نام رنگ وسیله نقلیه (اصلی)")]
        public string MainColor { get; set; }
        [Display(Name = "نام رنگ وسیله نقلیه (دوم)")]
        public string SecondColor { get; set; }
        [Display(Name = "کد کاربری وسیله نقلیه (شرکت بیمه)")]
        public string UsageCodeByCii { get; set; }
        [Display(Name = "کد کاربری وسیله نقلیه(بیمه مرکزی)")]
        public string UsgCodByCii { get; set; }
        [Display(Name = "نام کاربری وسیله نقلیه (در شرکت بیمه)")]
        public string UsageNameByCii { get; set; }
        [Display(Name = "نام کاربری وسیله نقلیه(ناجا)")]
        public string UsageByNaja { get; set; }
        [Display(Name = "مدل وسیله نقلیه)ناجا)")]
        public string ModelbyNaja { get; set; }
        [Display(Name = "کد یکتای بیمه نامه")]
        public string UniqueCode { get; set; }
        [Display(Name = "شماره چاپی بیمه نامه")]
        public string PrintNumber { get; set; }
        [Display(Name = "نام شرکت  بیمه")]
        public string CompanyTitle { get; set; }
        [Display(Name = "کد شرکت  بیمه")]
        public int? CompanyId { get; set; }
        [Display(Name = "تعداد سال تخفیف جانی")]
        public string DiscountLifeYearNumber { get; set; }
        [Display(Name = "تعداد سال تخفیف راننده")]
        public string DiscountPersonYearNumber { get; set; }
        [Display(Name = "تعداد سال تخفیف مالی")]
        public string DiscountFinancialYearNumber { get; set; }
        [Display(Name = "درصد تخفیف جانی")]
        public string DiscountLifeYearPercent { get; set; }
        [Display(Name = "درصد تخفیف راننده")]
        public string DiscountPersonYearPercent { get; set; }
        [Display(Name = "درصد تخفیف مالی")]
        public string DiscountFinancialYearPercent { get; set; }
        [Display(Name = "تاریخ نصب پلاک")]
        public string PlateInstallDate { get; set; }
        [Display(Name = "کاربری دوم")]
        public string SubUsage { get; set; }
        [Display(Name = "نوع وسیله نقلیه(در شرکت بیمه)")]
        public string CarTypeNameByCii { get; set; }
        [Display(Name = "کد نوع وسیله نقلیه)در شرکت بیمه)")]
        public string CarTypCodeByCii { get; set; }
        [Display(Name = "کد گروه بندی وسیله نقلیه (بیمه مرکزی)")]
        public string CarGroupCode { get; set; }
        [Display(Name = "نام سیستم وسیله نقلیه (بیمه مرکزی)")]
        public string SystemByCii { get; set; }
        [Display(Name = "تیپ خودرو (در بیمه مرکزی)")]
        public string TypeByCii { get; set; }
        [Display(Name = "نام سیستم وسیله نقلیه (در ناجا)")]
        public string SystemByNaja { get; set; }
        [Display(Name = "تیپ خودرو (در ناجا)")]
        public string TypeByNaja { get; set; }
        [Display(Name = "نام تیپ خودرو در شرکت")]
        public string TypeCodeByCii { get; set; }
        [Display(Name = "کد سیستم خودرو")]
        public string VehicleSystemCode { get; set; }
      
        public string ChassisNumber { get; set; }
        public string EngineNumber { get; set; }
        public string Vin { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }

    }
}
