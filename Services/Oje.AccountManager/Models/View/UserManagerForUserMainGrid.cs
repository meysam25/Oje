﻿using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class UserServiceForUserMainGrid: GlobalGrid
    {
        public string username { get; set; }
        public string fistname { get; set; }
        public string lastname { get; set; }
        public string mobile { get; set; }
        public bool? isActive { get; set; }
        public int? roleIds { get; set; }
    }
}
