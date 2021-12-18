using Oje.Infrastructure.Models;
using Oje.Section.BaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Interfaces
{
    public interface ITaxService
    {
        ApiResult Create(CreateUpdateTaxVM input);
        ApiResult Delete(int? id);
        CreateUpdateTaxVM GetById(int? id);
        ApiResult Update(CreateUpdateTaxVM input);
        GridResultVM<TaxMainGridResultVM> GetList(TaxMainGrid searchInput);
    }
}
