using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace Oje.Section.WebMain.Models.View
{
    public class AboutUsMainPageCreateUpdateVM
    {
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "زیرعنوان")]
        public string subTitle { get; set; }
        [Display(Name = "توضیحات")]
        [IgnoreStringEncode]
        public MyHtmlString desc { get; set; }
        [Display(Name = "تصویر سمت راست")]
        public IFormFile rightFile { get; set; }
        [Display(Name = "زیر عنوان تصویر سمت راست")]
        public string rightFileTitle { get; set; }
        [Display(Name = "تصویر سمت وسط")]
        public IFormFile centerFile { get; set; }
        [Display(Name = "زیرعنوان تصویر وسط")]
        public string centerFileTitle { get; set; }
        [Display(Name = "تصویر سمت چپ")]
        public IFormFile leftFile { get; set; }
        [Display(Name = "زیر عنوان تصویر سمت چپ")]
        public string leftFileTitle { get; set; }
    }
}
