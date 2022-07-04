using System.ComponentModel.DataAnnotations;

namespace Oje.Security.Models.View
{
    public class BlockAutoIpMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public string id { get; set; }
        [Display(Name = "ای پی")]
        public string ip { get; set; }
        [Display(Name = "تاریخ")]
        public string createDate { get; set; }
        [Display(Name = "نام")]
        public string fullUsername { get; set; }
        [Display(Name = "بخش")]
        public string section { get; set; }
        [Display(Name = "درخواست")]
        public string rid { get; set; }
        [Display(Name = "مدت زمان")]
        public string duration { get; set; }
        [Display(Name = "موفقیت؟")]
        public string isSuccess { get; set; }
    }
}
