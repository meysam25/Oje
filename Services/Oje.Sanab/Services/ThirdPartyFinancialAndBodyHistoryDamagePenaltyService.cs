using Oje.Sanab.Interfaces;
using Oje.Sms.Services.EContext;

namespace Oje.Sanab.Services
{
    public class ThirdPartyFinancialAndBodyHistoryDamagePenaltyService: IThirdPartyFinancialAndBodyHistoryDamagePenaltyService
    {
        readonly SanabDBContext db = null;
        public ThirdPartyFinancialAndBodyHistoryDamagePenaltyService(SanabDBContext db)
        {
            this.db = db;
        }

        public string GetFinancialIdBy(int? count)
        {
            if (count == null || count == 0)
                return "no";

            return db.ThirdPartyFinancialAndBodyHistoryDamagePenalties
                .Where(t => t.IsFinancial == true && t.Count == count).Select(t => t.Id)
                .FirstOrDefault() + "";
        }

        public string GetNotFinancialIdBy(int? count)
        {
            if (count == null || count == 0)
                return "no";

            return db.ThirdPartyFinancialAndBodyHistoryDamagePenalties
                .Where(t => (t.IsFinancial == null || t.IsFinancial == false) && t.Count == count).Select(t => t.Id)
                .FirstOrDefault() + "";
        }
    }
}
