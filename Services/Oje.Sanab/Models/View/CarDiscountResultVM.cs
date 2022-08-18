using System.ComponentModel.DataAnnotations;

namespace Oje.Sanab.Models.View
{
    public class CarDiscountResultVM
    {
        public string LastCompanyDocumentNumber { get; set; }
        public long? UsageCode { get; set; }
        public long? MapUsageCode { get; set; }
        public string MapUsageName { get; set; }
        public long? Plk1 { get; set; }
        public long? Plk2 { get; set; }
        public long? Plk3 { get; set; }
        public long? PlkSrl { get; set; }
        public string PrintEndorsCompanyDocumentNumber { get; set; }
        public string InsuranceFullName { get; set; }
        public string EndorseDate { get; set; }
        [Display(Name = "نام سیستم وسیله نقلیه (در ناجا)")]
        public string SystemField { get; set; }
        [Display(Name = "تیپ خودرو (در ناجا)")]
        public string TypeField { get; set; }
        [Display(Name = "کاربری وسیله نقلیه")]
        public string UsageField { get; set; }
        [Display(Name = "نام رنگ وسیله نقلیه (اصلی)")]
        public string MainColorField { get; set; }
        [Display(Name = "نام رنگ وسیله نقلیه (دوم)")]
        public string SecondColorField { get; set; }
        [Display(Name = "سال تولید")]
        public string ModelField { get; set; }
        [Display(Name = "ظرفیت (تعداد سرنشین) وسیله نقلیه")]
        public string CapacityField { get; set; }
        [Display(Name = "تعداد سیلندر وسیله نقلیه")]
        public string CylinderNumberField { get; set; }
        [Display(Name = "شماره موتور")]
        public string EngineNumberField { get; set; }
        [Display(Name = "شماره شاسی")]
        public string ChassisNumberField { get; set; }
        [Display(Name = "شماره VIN")]
        public string VinNumberField { get; set; }
        [Display(Name = "تاریخ نصب پلاک")]
        public string InstallDateField { get; set; }
        [Display(Name = "تعداد محور")]
        public string AxelNumberField { get; set; }
        [Display(Name = "تعداد چرخ")]
        public string wheelNumberField { get; set; }
        [Display(Name = "نام شرکت بیمه")]
        public string CompanyName { get; set; }
        [Display(Name = "کد شرکت بیمه")]
        public int? CompanyCode { get; set; }
        [Display(Name = "تاریخ صدور بیمه نامه")]
        public string IssueDate { get; set; }
        [Display(Name = "تاریخ شروع بیمه نامه")]
        public string SatrtDate { get; set; }
        [Display(Name = "تاریخ پایان بیمه نامه")]
        public string EndDate { get; set; }
        [Display(Name = "Thrname")]
        public string Thrname { get; set; }
        [Display(Name = "کد کاربری در بیمه مرکزی")]
        public string MapUsgCod { get; set; }
        [Display(Name = "عنوان کاربری در بیمه مرکزی")]
        public string MapUsgName { get; set; }
        [Display(Name = "تعدا سیلندر")]
        public string CylinderCount { get; set; }
        [Display(Name = "الحاقیه")]
        public string EndorseText { get; set; }
        [Display(Name = "تعدا خسارت جانی بیمه نامه")]
        public int? PolicyHealthLossCount { get; set; }
        [Display(Name = "تعداد خسارت مالی بیمه نامه")]
        public int? PolicyFinancialLossCount { get; set; }
        [Display(Name = "تعداد خسارت راننده")]
        public int? PolicyPersonLossCount { get; set; }
        [Display(Name = "توناژ")]
        public decimal? Tonage { get; set; }
        [Display(Name = "کد بیمه نامه شخص ثالث")]
        public long? ThirdPolicyCode { get; set; }
        public long? DiscountPersonPercent { get; set; }
        public long? DiscountThirdPercent { get; set; }
        [Display(Name = "شماره چاپی بیمه نامه")]
        public string PrntPlcyCmpDocNo { get; set; }
        [Display(Name = "پلاک وسیله نقلیه")]
        public string plk { get; set; }
        [Display(Name = "عنوان نوع وسیله در بیمه مرکزی")]
        public string MapTypNam { get; set; }
        [Display(Name = "شماره موتور خودرو")]
        public string MtrNum { get; set; }
        [Display(Name = "شماره شاسی خودرو")]
        public string ShsNum { get; set; }
        [Display(Name = "تاریخ شروع بیمه نامه زیان دیده")]
        public string HBgnDte { get; set; }
        [Display(Name = "تاریخ پایان بیمه نامه زیان دیده")]
        public string HEndDte { get; set; }
        [Display(Name = "تعداد سال عدم خسارت مالی")]
        public string DisFnYrNum { get; set; }
        [Display(Name = "تعداد سال عدم خسارت جانی")]
        public string DisLfYrNum { get; set; }
        [Display(Name = "نامشخص")]
        public string DisPrsnYrNum { get; set; }
        [Display(Name = "درصد تخفیف یا افزایش نهایی اعمال شده در بیمه نامه جاری بابت عدم یا وجود خسارت حوادث راننده")]
        public string DisPrsnYrPrcnt { get; set; }
        [Display(Name = "درصد تخفیف عدم خسارت مالی بیمه نامه")]
        public string DisFnYrPrcnt { get; set; }
        [Display(Name = "مبلغ تعهد حوادث راننده")]
        public long? PrsnCvrCptl { get; set; }
        [Display(Name = "مبلغ تعهد جانی به ریال")]
        public long? LfCvrCptl { get; set; }
        [Display(Name = "مبلغ تعهد مالی به ریال")]
        public long? FnCvrCptl { get; set; }
        [Display(Name = "شماره بیمه نامه قبلی")]
        public string LstCmpDocNo { get; set; }
        [Display(Name = "شماره VIN (در بیمه نامه)")]
        public string vin { get; set; }
        [Display(Name = "کد نوع کاربری")]
        public int? UsgCod { get; set; }
        [Display(Name = "نام کاربری در شرکت بیمه")]
        public string MapUsgNam { get; set; }
        [Display(Name = "تعداد سیلندر در وسائل نقلیه ای (وسائل نقلیه ای که تعداد سیلندر شناسایی نشده و یا وجود ندارند (مانند خودروهای راهداری –کشاورزی –برخی از موتورسیکلتها) معادل این فیلد عدد صفر می باشد)")]
        public int? CylCnt { get; set; }
    }
}
