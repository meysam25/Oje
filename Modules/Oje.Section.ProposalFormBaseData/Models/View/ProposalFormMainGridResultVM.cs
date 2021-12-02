using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class ProposalFormMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get;  set; }
        [Display(Name = "نام")]
        public string name { get;  set; }
        [Display(Name = "گروه بندی")]
        public string category { get;  set; }
        [Display(Name = "وضعیت")]
        public string isActive { get;  set; }
        [Display(Name = "تنظیمات")]
        public string setting { get;  set; }
    }
}
