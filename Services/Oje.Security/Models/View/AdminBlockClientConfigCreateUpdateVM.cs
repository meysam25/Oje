﻿using Oje.Infrastructure.Models;

namespace Oje.Security.Models.View
{
    public class AdminBlockClientConfigCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public long? aid { get; set; }
        public int? value { get; set; }
        public bool? isActive { get; set; }
    }
}
