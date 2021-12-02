using Oje.Infrastructure.Models;
using Oje.Section.Security.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Security.Interfaces
{
    public interface IIpLimitationWhiteListManager
    {
        ApiResult Create(CreateUpdateIpLimitationWhiteListVM input);
        ApiResult Delete(int? id);
        CreateUpdateIpLimitationWhiteListVM GetById(int? id);
        ApiResult Update(CreateUpdateIpLimitationWhiteListVM input);
        GridResultVM<IpLimitationWhiteListMainGridResultVM> GetList(IpLimitationWhiteListMainGrid searchInput);
    }
}
