using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.View
{
    public class ProposalFilledFormCompanyPriceMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public string id { get; set; }
        [Display(Name = "شرکت")]
        public string cId { get; set; }
        [Display(Name = "مبلغ")]
        public string price { get; set; }
        [Display(Name = "کاربر ایجاد کننده")]
        public string createUser { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "کاربر ویرایش کننده")]
        public string updateUser { get; set; }
        [Display(Name = "تاریخ ویرایش")]
        public string updateDate { get; set; }
        [Display(Name = "انتخاب شده؟")]
        public string isSelected { get; set; }
        public bool isSelectedBool { get; set; }
    }
}
