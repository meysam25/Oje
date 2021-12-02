using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class RoundInqueryManager: IRoundInqueryManager
    {
        readonly ProposalFormDBContext db = null;
        public RoundInqueryManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public RoundInquery GetBySiteSettingAndProposalForm(int? siteSettingId, int? proposalFormId)
        {
            return db.RoundInqueries.Where(t => t.SiteSettingId == siteSettingId && t.ProposalFormId == proposalFormId).AsNoTracking().FirstOrDefault();
        }
    }
}
