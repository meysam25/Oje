using Oje.Sms.Interfaces;
using Oje.Sms.Services.EContext;

namespace Oje.Sms.Services
{
    public class BlockLoginUserService: IBlockLoginUserService
    {
        readonly SmsDBContext db = null;
        public BlockLoginUserService
            (
                SmsDBContext db
            )
        {
            this.db = db;
        }

        public bool IsValidDay(DateTime targetDate, int? siteSettingId)
        {
            return !db.BlockLoginUsers.Any(t => t.SiteSettingId == siteSettingId && t.IsActive == true && t.StartDate <= targetDate && t.EndDate >= targetDate);
        }
    }
}
