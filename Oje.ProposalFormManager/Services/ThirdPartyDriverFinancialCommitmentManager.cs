using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ThirdPartyDriverFinancialCommitmentService: IThirdPartyDriverFinancialCommitmentService
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyDriverFinancialCommitmentService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public ThirdPartyDriverFinancialCommitment GetLastActiveItem()
        {
            return db.ThirdPartyDriverFinancialCommitments.Where(t => t.IsActive == true).AsNoTracking().OrderByDescending(t => t.Year).FirstOrDefault();
        }
    }
}
