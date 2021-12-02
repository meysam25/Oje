using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class ThirdPartyFinancialCommitmentManager: IThirdPartyFinancialCommitmentManager
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyFinancialCommitmentManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public ThirdPartyFinancialCommitment GetLastActiveItem()
        {
            return db.ThirdPartyFinancialCommitments.Where(t => t.IsActive == true).OrderByDescending(t => t.Year).AsNoTracking().FirstOrDefault();
        }
    }
}
