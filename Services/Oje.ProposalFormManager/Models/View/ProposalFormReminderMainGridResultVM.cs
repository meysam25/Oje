using System.ComponentModel.DataAnnotations;

namespace Oje.ProposalFormService.Models.View
{
    public class ProposalFormReminderMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "فرم پیشنهاد")]
        public string ppfTitle { get; set; }
        [Display(Name = "شماره همراه")]
        public string mobile { get; set; }
        [Display(Name = "تاریخ یادآوری")]
        public string td { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
