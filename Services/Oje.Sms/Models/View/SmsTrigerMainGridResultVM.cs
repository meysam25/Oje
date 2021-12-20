using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.View
{
    public class SmsTrigerMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
        [Display(Name = "نقش")]
        public string roleName { get; set; }
        [Display(Name = "کاربر")]
        public string userName { get; set; }
    }
}
