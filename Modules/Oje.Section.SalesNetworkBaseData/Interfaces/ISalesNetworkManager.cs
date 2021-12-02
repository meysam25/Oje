using Oje.Infrastructure.Models;
using Oje.Section.SalesNetworkBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.SalesNetworkBaseData.Interfaces
{
    public interface ISalesNetworkManager
    {
        ApiResult Create(CreateUpdateSalesNetworkVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(CreateUpdateSalesNetworkVM input);
        GridResultVM<SalesNetworkMainGridResulgVM> GetList(SalesNetworkMainGrid searchInput);
    }
}
