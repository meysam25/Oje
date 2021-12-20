using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Services
{
    public class InqueryDescriptionService: IInqueryDescriptionService
    {
        readonly ProposalFormDBContext db = null;
        public InqueryDescriptionService(
            ProposalFormDBContext db
            )
        {
            this.db = db;
        }

        public List<InqueryDescription> GetByProposalFormId(int? proposalFormId, int? siteSettingId)
        {
            return db.InqueryDescriptions.Where(t => t.ProposalFormId == proposalFormId && t.IsActive == true && (t.SiteSettingId == siteSettingId)).ToList();
        }
    }
}
