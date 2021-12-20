using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models
{
    public class RoleGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "نام")]
        public string name { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "وب سایت")]
        public string siteSetting { get; set; }
        [Display(Name = "مقدار")]
        public string value { get; set; }
        [Display(Name = "تعداد کاربر")]
        public int userCount { get; set; }
    }
}
