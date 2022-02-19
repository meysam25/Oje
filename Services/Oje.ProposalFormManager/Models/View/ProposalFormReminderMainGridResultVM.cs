using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.View
{
    public class ProposalFormReminderMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public string id { get; set; }
        [Display(Name = "فرم پیشنهاد")]
        public string ppfTitle { get; set; }
        [Display(Name = "شماره همراه")]
        public string mobile { get; set; }
        [Display(Name = "تاریخ یادآوری")]
        public string td { get; set; }
    }
}
