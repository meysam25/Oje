﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Models.View
{
    public class ThirdPartyDriverHistoryDamagePenaltyMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "درصد")]
        public int percent { get; set; }
        [Display(Name = "وضعیت")]
        public string isActive { get; set; }
    }
}
