﻿using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Models.View
{
    public class ThirdPartyCarCreateDatePercentMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public int? fromYear { get; set; }
        public int? toYear { get; set; }
        public int? percent { get; set; }
        public bool? isActive { get; set; }
    }
}
