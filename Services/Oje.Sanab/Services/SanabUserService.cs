using Oje.Sanab.Interfaces;
using Oje.Sanab.Models.DB;
using Oje.Sms.Services.EContext;

namespace Oje.Sanab.Services
{
    public class SanabUserService: ISanabUserService
    {
        readonly SanabDBContext db = null;
        public SanabUserService(SanabDBContext db)
        {
            this.db = db;
        }

        public SanabUser GetActive(int? siteSettingId)
        {
            return db.SanabUsers.Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true).FirstOrDefault();
        }
    }
}
