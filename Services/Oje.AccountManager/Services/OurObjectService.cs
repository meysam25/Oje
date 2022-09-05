using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.AccountService.Services
{
    public class OurObjectService: IOurObjectService
    {
        readonly AccountDBContext db = null;
        public OurObjectService(AccountDBContext db)
        {
            this.db = db;
        }

        public List<OurObject> GetList(int? siteSettingId)
        {
            return db.OurObjects.Where(t => t.IsActive == true && t.SiteSettingId == siteSettingId).AsNoTracking().ToList();
        }
    }
}
