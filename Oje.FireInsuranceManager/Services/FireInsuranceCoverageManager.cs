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
    public class FireInsuranceCoverageService : IFireInsuranceCoverageService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public FireInsuranceCoverageService(FireInsuranceServiceDBContext db)
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
