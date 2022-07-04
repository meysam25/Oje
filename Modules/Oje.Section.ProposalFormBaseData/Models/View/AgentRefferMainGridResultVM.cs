using System;
using System.ComponentModel.DataAnnotations;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class AgentRefferMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "شرکت")]
        public string companyTitle { get; set; }
        [Display(Name = "کد")]
        public string code { get; set; }
        [Display(Name = "نام کامل")]
        public string fullname { get; set; }
        [Display(Name = "همراه")]
        public string mobile { get; set; }
    }
}
