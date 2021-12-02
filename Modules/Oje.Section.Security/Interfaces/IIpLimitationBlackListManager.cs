using Oje.Infrastructure.Models;
using Oje.Section.Security.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Security.Interfaces
{
    public interface IIpLimitationBlackListManager
    {
        ApiResult Create(CreateUpdateIpLimitationBlackListVM input);
        ApiResult Delete(int? id);
        CreateUpdateIpLimitationBlackListVM GetById(int? id);
        ApiResult Update(CreateUpdateIpLimitationBlackListVM input);
        GridResultVM<IpLimitationBlackListMainGridResultVM> GetList(IpLimitationBlackListMainGrid searchInput);
    }
}
