using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Interfaces
{
    public interface ICityService
    {
        object GetLightList(int? provinceId);
        int? GetIdBy(string title);
        int? Create(int? provinceId, string title, bool isActive);
    }
}
