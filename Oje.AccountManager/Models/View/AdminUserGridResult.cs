using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class AdminUserGridResult
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "نام کاربری")]
        public string username { get; set; }
        [Display(Name = "نام")]
        public string fistname { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string lastname { get; set; }
        [Display(Name = "همراه")]
        public string mobile { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "نقش")]
        public string roleIds { get; set; }
    }
}
