using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class MainPageTopLeftIconCreateUpdateVM
    {
        [Display(Name = "عنوان اول")]
        public string title1 { get; set; }
        [Display(Name = "تصویر اول")]
        public IFormFile mainFile1 { get; set; }
        [Display(Name = "توضیحات اول")]
        [IgnoreStringEncode]
        public MyHtmlString description1 { get; set; }

        [Display(Name = "عنوان دوم")]
        public string title2 { get; set; }
        [Display(Name = "فایل دوم")]
        public IFormFile mainFile2 { get; set; }
        [Display(Name = "توضیحات دوم")]
        [IgnoreStringEncode]
        public MyHtmlString description2 { get; set; }

        [Display(Name = "عنوان سوم")]
        public string title3 { get; set; }
        [Display(Name = "فایل سوم")]
        public IFormFile mainFile3 { get; set; }
        [Display(Name = "توضیحات سوم")]
        [IgnoreStringEncode]
        public MyHtmlString description3 { get; set; }

    }
}
