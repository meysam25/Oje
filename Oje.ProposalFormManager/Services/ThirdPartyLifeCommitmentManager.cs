using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class ThirdPartyLifeCommitmentManager : IThirdPartyLifeCommitmentManager
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyLifeCommitmentManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public ThirdPartyLifeCommitment GetLastActiveItem()
        {
            return db.ThirdPartyLifeCommitments.Where(t => t.IsActive == true).OrderByDescending(t => t.Year).AsNoTracking().FirstOrDefault();
        }
    }
}
