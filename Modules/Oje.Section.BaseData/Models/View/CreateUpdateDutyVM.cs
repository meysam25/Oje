﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Models.View
{
    public class CreateUpdateDutyVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int? year { get; set; }
        public bool? isActive { get; set; }
        public byte? percent { get; set; }
    }
}
