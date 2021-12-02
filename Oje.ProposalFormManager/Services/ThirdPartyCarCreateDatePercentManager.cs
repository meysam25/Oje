using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class ThirdPartyCarCreateDatePercentManager: IThirdPartyCarCreateDatePercentManager
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyCarCreateDatePercentManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public List<ThirdPartyCarCreateDatePercent> GetActiveList()
        {
            return db.ThirdPartyCarCreateDatePercents.Where(t => t.IsActive == true).AsNoTracking().ToList();
        }
    }
}
