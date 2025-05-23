﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Oje.Section.WebMain.Models.View
{
    public class OurPrideCreateUpdateVM
    {
        [Display(Name = "عنوان")]
        public string title { get; set; }

        [Display(Name = "لوگوی 1")]
        public IFormFile image1 { get; set; }
        [Display(Name = "زیرعنوان لوگوی 1")]
        public string title1 { get; set; }

        [Display(Name = "لوگوی 2")]
        public IFormFile image2 { get; set; }
        [Display(Name = "زیرعنوان لوگوی 2")]
        public string title2 { get; set; }

        [Display(Name = "لوگوی 3")]
        public IFormFile image3 { get; set; }
        [Display(Name = "زیرعنوان لوگوی 3")]
        public string title3 { get; set; }

        [Display(Name = "لوگوی 4")]
        public IFormFile image4 { get; set; }
        [Display(Name = "زیرعنوان لوگوی 4")]
        public string title4 { get; set; }
    }
}
