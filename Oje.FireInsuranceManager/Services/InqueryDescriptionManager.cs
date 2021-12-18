using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Models.DB;
using Oje.FireInsuranceService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Services
{
    public class InqueryDescriptionService: IInqueryDescriptionService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public InqueryDescriptionService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public List<InqueryDescription> GetBy(int? proposalFormId, int? siteSettingId)
        {
            return db.InqueryDescriptions.Where(t => t.ProposalFormId == proposalFormId && t.IsActive == true && (t.SiteSettingId == siteSettingId)).ToList();
        }
    }
}
