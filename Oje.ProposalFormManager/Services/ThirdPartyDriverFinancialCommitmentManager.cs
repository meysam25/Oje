using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class ThirdPartyDriverFinancialCommitmentManager: IThirdPartyDriverFinancialCommitmentManager
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyDriverFinancialCommitmentManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public ThirdPartyDriverFinancialCommitment GetLastActiveItem()
        {
            return db.ThirdPartyDriverFinancialCommitments.Where(t => t.IsActive == true).AsNoTracking().OrderByDescending(t => t.Year).FirstOrDefault();
        }
    }
}
