using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFilledFormSiteSettingService: IProposalFilledFormSiteSettingService
    {
        readonly ProposalFormDBContext db = null;
        public ProposalFilledFormSiteSettingService
            (
                ProposalFormDBContext db
            )
        {
            this.db = db;
        }

        public void Create(long proposalFilledFormId, int? siteSettingId)
        {
            if(siteSettingId.ToIntReturnZiro() > 0 && proposalFilledFormId > 0)
            {
                if(!db.ProposalFilledFormSiteSettings.Any(t => t.SiteSettingId == siteSettingId && t.ProposalFilledFormId == proposalFilledFormId))
                {
                    var newItem = new ProposalFilledFormSiteSetting()
                    {
                        ProposalFilledFormId = proposalFilledFormId,
                        SiteSettingId = siteSettingId.Value
                    };
                    newItem.FilledSignature();
                    db.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    db.SaveChanges();
                }
            }
        }
    }
}
