﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class NoDamageDiscountMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "شرکت")]
        public string cId { get; set; }
        [Display(Name = "فرم پیشنهاد")]
        public string formTitle { get; set; }
        [Display(Name = "درصد")]
        public int pecent { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
