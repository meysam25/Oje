using System.ComponentModel.DataAnnotations;

namespace Oje.ProposalFormService.Models.View
{
    public class ProposalFilledFormMainGridResult
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "شرکت")]
        public string cId { get; set; }
        [Display(Name = "فرم")]
        public string ppfTitle { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "مبلغ")]
        public string price { get; set; }
        [Display(Name = "نماینده")]
        public string agentFullname { get; set; }
        [Display(Name = "بیمه گذار")]
        public string targetUserfullname { get; set; }
        [Display(Name = "نام کاربری بیمه گذار")]
        public string targetUserMobileNumber { get; set; }
        [Display(Name = "ثبت کننده")]
        public string createUserfullname { get; set; }
        [Display(Name = "تاریخ صدور")]
        public string issueDate { get; set; }
        [Display(Name = "تاریخ شروع")]
        public string startDate { get; set; }
        [Display(Name = "تاریخ پایان")]
        public string endDate { get; set; }
        [Display(Name = "نماینده")]
        public bool isAgent { get; set; }
        [Display(Name = "کد ملی بیمه گذار")]
        public string targetUserNationalCode { get; set; }
    }
}
