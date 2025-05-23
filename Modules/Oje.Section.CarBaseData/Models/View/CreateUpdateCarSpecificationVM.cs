﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class CreateUpdateCarSpecificationVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public decimal? carRoomRate { get; set; }
        public bool? isActive { get; set; }
        public List<int> vehicleSpecIds { get; set; }
    }
}
