using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.View
{
    public class FireInsuranceRateMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "شرکت")]
        public string company { get; set; }
        [Display(Name = "اسکلت ساختمان")]
        public string bBody { get; set; }
        [Display(Name = "نوع ساختمان")]
        public string bType { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "حداکثر")]
        public string maxValue { get; set; }
        [Display(Name = "حداقل")]
        public string minValue { get; set; }
        [Display(Name = "نرخ")]
        public decimal? rate { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
