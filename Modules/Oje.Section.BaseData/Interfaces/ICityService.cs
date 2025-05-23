﻿using Oje.Infrastructure.Models;
using Oje.Section.BaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Interfaces
{
    public interface ICityService
    {
        ApiResult Create(CreateUpdateCityVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(CreateUpdateCityVM input);
        GridResultVM<CityMainGriResultVM> GetList(CityMainGrid searchInput);
    }
}
