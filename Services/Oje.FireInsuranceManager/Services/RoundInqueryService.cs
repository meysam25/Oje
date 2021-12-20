using Microsoft.EntityFrameworkCore;
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
    public class RoundInqueryService: IRoundInqueryService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public RoundInqueryService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public RoundInquery GetBy(int? proposalFormId, int? siteSettingId)
        {
            return db.RoundInqueriess.Where(t => t.SiteSettingId == siteSettingId && t.ProposalFormId == proposalFormId).AsNoTracking().FirstOrDefault();
        }
    }
}
