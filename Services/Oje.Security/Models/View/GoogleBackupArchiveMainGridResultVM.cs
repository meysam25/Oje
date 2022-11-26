using System.ComponentModel.DataAnnotations;

namespace Oje.Security.Models.View
{
    public class GoogleBackupArchiveMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public long id { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string createDate { get; set; }
        [Display(Name = "حجم")]
        public string size { get; set; }
        [Display(Name = "محل ذخیره شده")]
        public string location { get; set; }
    }
}
