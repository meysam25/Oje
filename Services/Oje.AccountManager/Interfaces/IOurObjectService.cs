using Oje.AccountService.Models.DB;
using System.Collections.Generic;

namespace Oje.AccountService.Interfaces
{
    public interface IOurObjectService
    {
        List<OurObject> GetList(int? siteSettingId);
    }
}
