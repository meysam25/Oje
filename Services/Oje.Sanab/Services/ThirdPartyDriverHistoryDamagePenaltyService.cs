using Oje.Sanab.Interfaces;
using Oje.Sms.Services.EContext;

namespace Oje.Sanab.Services
{
    public class ThirdPartyDriverHistoryDamagePenaltyService: IThirdPartyDriverHistoryDamagePenaltyService
    {
        readonly SanabDBContext db = null;
        public ThirdPartyDriverHistoryDamagePenaltyService(SanabDBContext db)
        {
            this.db = db;
        }

        public string GetIdBy(int? count)
        {
            if (count == null || count == 0)
                return "no";

            return db.ThirdPartyDriverHistoryDamagePenalties
                .Where(t => t.Count == count)
                .Select(t => t.Id)
                .FirstOrDefault() + "";
        }
    }
}
