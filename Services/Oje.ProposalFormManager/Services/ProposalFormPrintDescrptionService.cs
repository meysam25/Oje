using Oje.Infrastructure.Models.Pdf.ProposalFilledForm;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFormPrintDescrptionService : IProposalFormPrintDescrptionService
    {
        readonly ProposalFormDBContext db = null;
        public ProposalFormPrintDescrptionService
            (
                ProposalFormDBContext db
            )
        {
            this.db = db;
        }

        public List<ProposalFormPrintDescrptionVM> GetList(int? siteSettingId, int proposalFormId)
        {
            return db.ProposalFormPrintDescrptions
                .Where(t => t.IsActive == true && t.SiteSettingId == siteSettingId && t.ProposalFormId == proposalFormId)
                .Select(t => new ProposalFormPrintDescrptionVM { descption = t.Description, type = t.Type })
                .ToList();
        }
    }
}
