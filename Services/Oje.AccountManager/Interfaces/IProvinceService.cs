﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Interfaces
{
    public interface IProvinceService
    {
        object GetLightList();
        int? GetBy(string title);
    }
}
