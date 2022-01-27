using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class FooterDescrptionCreateUpdateVM
    {
        [Display(Name = "لوگوی 1")]
        public IFormFile logo1 { get; set; }
        [Display(Name = "عنوان لوگوی 1")]
        public string logoTitle1 { get; set; }
        [Display(Name = "توضیحات لوگوی 1")]
        public string logoDescription1 { get; set; }

        [Display(Name = "لوگوی 2")]
        public IFormFile logo2 { get; set; }
        [Display(Name = "عنوان لوگوی 2")]
        public string logoTitle2 { get; set; }
        [Display(Name = "توضیحات لوگوی 2")]
        public string logoDescription2 { get; set; }

        [Display(Name = "لوگوی 3")]
        public IFormFile logo3 { get; set; }
        [Display(Name = "عنوان لوگوی 3")]
        public string logoTitle3 { get; set; }
        [Display(Name = "توضیحات لوگوی 3")]
        public string logoDescription3 { get; set; }
    }
}
