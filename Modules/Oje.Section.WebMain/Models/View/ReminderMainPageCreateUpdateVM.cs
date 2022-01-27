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
    public class ReminderMainPageCreateUpdateVM
    {
        [Display(Name = "لوگوی یادآوری")]
        public IFormFile mainImage { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "توضیحات")]
        [IgnoreStringEncode]
        public MyHtmlString desc { get; set; }
    }
}
