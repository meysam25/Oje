using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormMainGridResultVM
    {
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "نام")]
        public string name { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "نام کاربر")]
        public string userfullname { get; set; }
        [Display(Name = "مسیر")]
        public string url { get; set; }
    }
}
