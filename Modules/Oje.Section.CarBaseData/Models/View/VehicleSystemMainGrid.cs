﻿using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class VehicleSystemMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public int? order { get; set; }
        public int? carTypeId { get; set; }
        public bool? isActive { get; set; }
        public int? types { get; set; }
    }
}
