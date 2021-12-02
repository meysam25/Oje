using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Services
{
    public class InqueryDescriptionManager: IInqueryDescriptionManager
    {
        readonly ProposalFormDBContext db = null;
        public InqueryDescriptionManager(
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
