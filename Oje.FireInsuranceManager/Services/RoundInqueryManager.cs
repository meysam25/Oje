using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceManager.Interfaces;
using Oje.FireInsuranceManager.Models.DB;
using Oje.FireInsuranceManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Services
{
    public class RoundInqueryManager: IRoundInqueryManager
    {
        readonly FireInsuranceManagerDBContext db = null;
        public RoundInqueryManager(FireInsuranceManagerDBContext db)
        {
            this.db = db;
        }

        public RoundInquery GetBy(int? proposalFormId, int? siteSettingId)
        {
            return db.RoundInqueriess.Where(t => t.SiteSettingId == siteSettingId && t.ProposalFormId == proposalFormId).AsNoTracking().FirstOrDefault();
        }
    }
}
