using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class UserNotificationTemplateMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string subject { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
        [Display(Name = "برای")]
        public string pffUserType { get; set; }
    }
}
