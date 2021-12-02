using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class ThirdPartyRequiredFinancialCommitmentManager : IThirdPartyRequiredFinancialCommitmentManager
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyRequiredFinancialCommitmentManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public List<ThirdPartyRequiredFinancialCommitment> GetByIds(List<int> coverIds)
        {
            return db.ThirdPartyRequiredFinancialCommitments
                            .Where(t => t.IsActive == true)
                            .Include(t => t.ThirdPartyExteraFinancialCommitments).ThenInclude(t => t.ThirdPartyExteraFinancialCommitmentComs)
                            .Include(t => t.ThirdPartyRequiredFinancialCommitmentCompanies)
                            .AsNoTracking()
                            .Where(t => coverIds.Contains(t.Id))
                            .ToList();
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.ThirdPartyRequiredFinancialCommitments.Where(t => t.IsActive == true).OrderBy(t => t.Order).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
