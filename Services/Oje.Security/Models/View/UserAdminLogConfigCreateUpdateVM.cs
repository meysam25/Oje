﻿using Oje.Infrastructure.Models;

namespace Oje.Security.Models.View
{
    public class UserAdminLogConfigCreateUpdateVM: GlobalSiteSetting
    {
        public long? id { get; set; }
        public long? aId { get; set; }
        public bool? isActive { get; set; }
    }
}
