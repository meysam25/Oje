﻿using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Interfaces
{
    public interface IUserService
    {
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
