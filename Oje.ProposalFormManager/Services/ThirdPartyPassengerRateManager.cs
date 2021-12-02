using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class ThirdPartyPassengerRateManager : IThirdPartyPassengerRateManager
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyPassengerRateManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public List<ThirdPartyPassengerRate> GetActiveItems()
        {
            return db.ThirdPartyPassengerRates
                .Where(t => t.IsActive == true)
                .Include(t => t.ThirdPartyPassengerRateCompanies)
                .AsNoTracking()
                .ToList();
        }
    }
}
