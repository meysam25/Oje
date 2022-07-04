using System;

namespace Oje.Sanab.Models.View
{
    public class CarDiscountResultVM
    {
        //تعداد محور
        public string AxelNo { get; set; }
        //تعداد چرخ
        public string WheelNo { get; set; }
        //ظرفیت وسیله نقلیه
        public string Capacity { get; set; }
        //تعداد سیلندر وسیله نقلیه
        public string CylinderNo { get; set; }
        //رنگ وسیله نقلیه
        public string MainColor { get; set; }
        // رنگ فرعی وسیله نقلیه
        public string SecondColor { get; set; }
        //کد کاربری شرکت بیمه
        public string MapUsgCod { get; set; }
        //کد کاربری وسیله نقلیه
        public string UsgCodByCii { get; set; }
        //نام کاربری وسیله نقلیه(در شرکت بیمه)
        public string MapUsgNam { get; set; }
        //نام کاربری وسیله نقلیه
        public string UsageByNaja { get; set; }
        //مدل وسیله نقلیه
        public string ModelbyNaja { get; set; }
        //کد یکتای بیمه نامه
        public string UniqueCode { get; set; }
        //شماره چاپی بیمه نامه
        public string PrintNumber { get; set; }
        //نام شرکت
        public string CompanyTitle { get; set; }
        //کد شرکت
        public int? CompanyId { get; set; }
        //تعداد سال تخفیف جانی
        public string DisLfYrNum { get; set; }
        //تعداد سال تخفیف راننده
        public string DisPrsnYrNum { get; set; }
        //تعداد سال تخفیف مالی
        public string DisFnYrNum { get; set; }
        //درصد تخفیف جانی
        public string DisLfYrPrcnt { get; set; }
        //درصد تخفیف راننده
        public string DisPrsnYrPrc { get; set; }
        //درصد تخفیف مالی
        public string DisFnYrPrcnt { get; set; }
        //تاریخ نصب پلاک
        public string PlateInstall { get; set; }
        //کاربری دوم
        public string SubUsage { get; set; }
        //نام نوع وسیله نقلیه
        public string MapCarTypNam { get; set; }
        //کد نوع وسیله نقلیه
        public string MapCarTypCod { get; set; }
        //کد نوع گروهبندی وسیله نقلیه
        public string CarGrpCod { get; set; }
        //نام سیستم وسیله نقلیه 
        public string SystemByCii { get; set; }

    }
}
