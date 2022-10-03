using System.ComponentModel.DataAnnotations;

namespace Oje.Section.WebMain.Models.View
{
    public class AutoAnswerOnlineChatMessageMainGridResultVM
    {
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "توضیحات")]
        public string description { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
        [Display(Name = "پیام می باشد؟")]
        public string isMessage { get; set; }
        [Display(Name = "تعداد پسند")]
        public int likeCount { get;  set; }
        [Display(Name = "تعداد نپسندیدن")]
        public int dislikeCount { get; set; }
        [Display(Name = "وب سایت")]
        public string siteTitleMN2 { get; set; }
    }
}
