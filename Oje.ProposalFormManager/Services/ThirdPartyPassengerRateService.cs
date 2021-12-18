using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ThirdPartyPassengerRateService : IThirdPartyPassengerRateService
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyPassengerRateService(ProposalFormDBContext db)
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
