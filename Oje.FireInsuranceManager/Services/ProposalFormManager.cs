using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceManager.Interfaces;
using Oje.FireInsuranceManager.Models.DB;
using Oje.FireInsuranceManager.Services.EContext;
using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Services
{
    public class ProposalFormManager : IProposalFormManager
    {
        readonly FireInsuranceManagerDBContext db = null;
        public ProposalFormManager(FireInsuranceManagerDBContext db)
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
