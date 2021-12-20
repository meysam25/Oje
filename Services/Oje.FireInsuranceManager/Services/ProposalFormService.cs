using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Models.DB;
using Oje.FireInsuranceService.Services.EContext;
using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Services
{
    public class ProposalFormService : IProposalFormService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public ProposalFormService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public ProposalForm GetByType(ProposalFormType type, int? siteSettingId)
        {
            return db.ProposalForms
                .Where(t => t.Type == type && (t.SiteSettingId == siteSettingId || t.SiteSettingId == null) && t.IsActive == true)
                .AsNoTracking()
                .FirstOrDefault();
        }
    }
}
