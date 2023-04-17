using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sanab.Models.DB
{
    [Table("SanabCarThirdPartyPlaqueInquiries")]
    public class SanabCarThirdPartyPlaqueInquiry
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        [Required, MaxLength(15)]
        public string NationalCode { get; set; }
        public int plaque_1 { get; set; }
        public int plaque_2 { get; set; }
        public int plaque_3 { get; set; }
        public int plaque_4 { get; set; }


        [MaxLength(100), Display(Name = "تعدادمحور")]
        public string AxelNo { get; set; }
        [MaxLength(100), Display(Name = "تعدادچرخ")]
        public string WheelNo { get; set; }
        [MaxLength(100), Display(Name = "رفیت (تعداد سرنشین) وسیله نقلیه")]
        public string Capacity { get; set; }
        [MaxLength(100), Display(Name = "تعداد سیلندر وسیله نقلیه")]
        public string CylinderNo { get; set; }
        [MaxLength(100), Display(Name = "نام رنگ وسیله نقلیه (اصلی)")]
        public string MainColor { get; set; }
        [MaxLength(100), Display(Name = "نام رنگ وسیله نقلیه (دوم)")]
        public string SecondColor { get; set; }
        [MaxLength(100), Display(Name = "کد کاربری وسیله نقلیه (شرکت بیمه)")]
        public string UsageCodeByCii { get; set; }
        [MaxLength(100), Display(Name = "کد کاربری وسیله نقلیه(بیمه مرکزی)")]
        public string UsgCodByCii { get; set; }
        [MaxLength(100), Display(Name = "نام کاربری وسیله نقلیه (در شرکت بیمه)")]
        public string UsageNameByCii { get; set; }
        [MaxLength(100), Display(Name = "نام کاربری وسیله نقلیه(ناجا)")]
        public string UsageByNaja { get; set; }
        [MaxLength(100), Display(Name = "مدل وسیله نقلیه)ناجا)")]
        public string ModelbyNaja { get; set; }
        [MaxLength(100), Display(Name = "کد یکتای بیمه نامه")]
        public string UniqueCode { get; set; }
        [MaxLength(100), Display(Name = "شماره چاپی بیمه نامه")]
        public string PrintNumber { get; set; }
        [MaxLength(100), Display(Name = "نام شرکت  بیمه")]
        public string CompanyTitle { get; set; }
        [Display(Name = "کد شرکت  بیمه")]
        public int? CompanyId { get; set; }
        [MaxLength(100), Display(Name = "تعداد سال تخفیف جانی")]
        public string DiscountLifeYearNumber { get; set; }
        [MaxLength(100), Display(Name = "تعداد سال تخفیف راننده")]
        public string DiscountPersonYearNumber { get; set; }
        [MaxLength(100), Display(Name = "تعداد سال تخفیف مالی")]
        public string DiscountFinancialYearNumber { get; set; }
        [MaxLength(100), Display(Name = "درصد تخفیف جانی")]
        public string DiscountLifeYearPercent { get; set; }
        [MaxLength(100), Display(Name = "درصد تخفیف راننده")]
        public string DiscountPersonYearPercent { get; set; }
        [MaxLength(100), Display(Name = "درصد تخفیف مالی")]
        public string DiscountFinancialYearPercent { get; set; }
        [MaxLength(100), Display(Name = "تاریخ نصب پلاک")]
        public string PlateInstallDate { get; set; }
        [MaxLength(100), Display(Name = "کاربری دوم")]
        public string SubUsage { get; set; }
        [MaxLength(100), Display(Name = "نوع وسیله نقلیه(در شرکت بیمه)")]
        public string CarTypeNameByCii { get; set; }
        [MaxLength(100), Display(Name = "کد نوع وسیله نقلیه)در شرکت بیمه)")]
        public string CarTypCodeByCii { get; set; }
        [MaxLength(100), Display(Name = "کد گروه بندی وسیله نقلیه (بیمه مرکزی)")]
        public string CarGroupCode { get; set; }
        [MaxLength(100), Display(Name = "نام سیستم وسیله نقلیه (بیمه مرکزی)")]
        public string SystemByCii { get; set; }
        [MaxLength(100), Display(Name = "تیپ خودرو (در بیمه مرکزی)")]
        public string TypeByCii { get; set; }
        [MaxLength(100), Display(Name = "نام سیستم وسیله نقلیه (در ناجا)")]
        public string SystemByNaja { get; set; }
        [MaxLength(100), Display(Name = "تیپ خودرو (در ناجا)")]
        public string TypeByNaja { get; set; }
        [MaxLength(100), Display(Name = "نام تیپ خودرو در شرکت")]
        public string TypeCodeByCii { get; set; }
        [MaxLength(100), Display(Name = "کد سیستم خودرو")]
        public string VehicleSystemCode { get; set; }

        [MaxLength(100)]
        public string ChassisNumber { get; set; }
        [MaxLength(100)]
        public string EngineNumber { get; set; }
        [MaxLength(100)]
        public string Vin { get; set; }
        [MaxLength(100)]
        public string BeginDate { get; set; }
        [MaxLength(100)]
        public string EndDate { get; set; }
    }
}
