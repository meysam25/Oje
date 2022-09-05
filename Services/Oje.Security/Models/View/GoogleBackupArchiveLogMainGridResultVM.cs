using Oje.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Oje.Security.Models.View
{
    public class GoogleBackupArchiveLogMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "تاریخ")]
        public string createDate { get; set; }
        [Display(Name = "پیام")]
        public string message { get; set; }
        [Display(Name = "نوع")]
        public string type { get; set; }
    }
}
