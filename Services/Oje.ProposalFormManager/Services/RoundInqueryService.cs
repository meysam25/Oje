using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class RoundInqueryService: IRoundInqueryService
    {
        readonly ProposalFormDBContext db = null;
        public RoundInqueryService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public RoundInquery GetBySiteSettingAndProposalForm(int? siteSettingId, int? proposalFormId)
        {
            return db.RoundInqueries.Where(t => t.SiteSettingId == siteSettingId && t.ProposalFormId == proposalFormId).AsNoTracking().FirstOrDefault();
        }
    }
}
