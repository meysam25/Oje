using Oje.Infrastructure.Services;
using Oje.Sanab.Interfaces;
using Oje.Sms.Services.EContext;

namespace Oje.Sanab.Services
{
    public class ThirdPartyBodyNoDamageDiscountHistoryService : IThirdPartyBodyNoDamageDiscountHistoryService
    {
        readonly SanabDBContext db = null;
        public ThirdPartyBodyNoDamageDiscountHistoryService(SanabDBContext db)
        {
            this.db = db;
        }

        public string GetIdBy(string percent)
        {
            int percentInt = percent.ToIntReturnZiro();
            if (string.IsNullOrEmpty(percent) || percent == "0" || percentInt == 0)
                return "0";

            return db.ThirdPartyBodyNoDamageDiscountHistories
                .Where(t => t.Percent == percentInt)
                .Select(t => t.Id)
                .FirstOrDefault() + "";
        }
    }
}
