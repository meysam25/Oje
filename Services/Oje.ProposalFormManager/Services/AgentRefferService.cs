using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class AgentRefferService: IAgentRefferService
    {
        readonly ProposalFormDBContext db = null;
        public AgentRefferService
            (
                ProposalFormDBContext db
            )
        {
            this.db = db;
        }

        public AgentReffer GetBy(int? companyId, int? siteSettingId)
        {
            return db.AgentReffers.Where(t => t.CompanyId == companyId && t.SiteSettingId == siteSettingId).FirstOrDefault();
        }
    }
}
