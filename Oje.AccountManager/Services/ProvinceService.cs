using Oje.AccountManager.Interfaces;
using Oje.AccountManager.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountManager.Services
{
    public class ProvinceService : IProvinceService
    {
        readonly AccountDBContext db = null;
        public ProvinceService(AccountDBContext db)
        {
            this.db = db;
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.Provinces.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
