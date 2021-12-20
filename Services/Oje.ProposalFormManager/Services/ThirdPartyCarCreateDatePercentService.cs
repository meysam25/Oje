using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ThirdPartyCarCreateDatePercentService: IThirdPartyCarCreateDatePercentService
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyCarCreateDatePercentService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public List<ThirdPartyCarCreateDatePercent> GetActiveList()
        {
            return db.ThirdPartyCarCreateDatePercents.Where(t => t.IsActive == true).AsNoTracking().ToList();
        }
    }
}
