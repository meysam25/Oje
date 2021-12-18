using Oje.Infrastructure.Models;
using Oje.Section.BaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Interfaces
{
    public interface ICompanyService
    {
        ApiResult Create(CreateUpdateCompanyVM input, long? userId);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(CreateUpdateCompanyVM input, long? userId);
        GridResultVM<CompanyMainGridResultVM> GetList(CompanyMainGrid searchInput);
        object GetLightList();
    }
}
