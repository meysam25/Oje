using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Interfaces
{
    public interface IPropertyService
    {
        T GetBy<T>(PropertyType type, int? siteSettingId) where T : class, new();
        ApiResult CreateUpdate(object input, int? siteSettingId, PropertyType type);
        void RemoveBy(PropertyType type, int? siteSettingId);
    }
}
