using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ThirdPartyFinancialCommitmentService: IThirdPartyFinancialCommitmentService
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyFinancialCommitmentService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public ThirdPartyFinancialCommitment GetLastActiveItem()
        {
            return db.ThirdPartyFinancialCommitments.Where(t => t.IsActive == true).OrderByDescending(t => t.Year).AsNoTracking().FirstOrDefault();
        }
    }
}
