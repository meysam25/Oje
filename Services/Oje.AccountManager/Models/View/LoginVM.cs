﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class LoginVM : SecurityCodeVM
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool? rememberMe { get; set; }
    }
}
