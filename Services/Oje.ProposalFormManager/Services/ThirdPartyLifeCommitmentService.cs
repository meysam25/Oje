using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ThirdPartyLifeCommitmentService : IThirdPartyLifeCommitmentService
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyLifeCommitmentService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public ThirdPartyLifeCommitment GetLastActiveItem()
        {
            return db.ThirdPartyLifeCommitments.Where(t => t.IsActive == true).OrderByDescending(t => t.Year).AsNoTracking().FirstOrDefault();
        }
    }
}
