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
    public class FireInsuranceCoverageManager : IFireInsuranceCoverageManager
    {
        readonly FireInsuranceManagerDBContext db = null;
        public FireInsuranceCoverageManager(FireInsuranceManagerDBContext db)
        {
            this.db = db;
        }

        public List<FireInsuranceCoverage> GetList(int? proposalFormId, List<int> companyIds)
        {
            return db.FireInsuranceCoverages
                .Where(t => t.IsActive == true && t.ProposalFormId == proposalFormId && t.FireInsuranceCoverageCompanies.Any(tt => companyIds.Contains(tt.CompanyId)))
                .Include(t => t.FireInsuranceCoverageCompanies)
                .AsNoTracking()
                .ToList();
        }
    }
}
