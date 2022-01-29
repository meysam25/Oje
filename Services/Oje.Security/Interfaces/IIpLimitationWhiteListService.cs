using Oje.Infrastructure.Models;
using Oje.Security.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Interfaces
{
    public interface IIpLimitationWhiteListService
    {
        ApiResult Create(CreateUpdateIpLimitationWhiteListVM input);
        ApiResult Delete(int? id);
        CreateUpdateIpLimitationWhiteListVM GetById(int? id);
        ApiResult Update(CreateUpdateIpLimitationWhiteListVM input);
        GridResultVM<IpLimitationWhiteListMainGridResultVM> GetList(IpLimitationWhiteListMainGrid searchInput);
    }
}
