using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ThirdPartyFinancialAndBodyHistoryDamagePenaltyService : IThirdPartyFinancialAndBodyHistoryDamagePenaltyService
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyFinancialAndBodyHistoryDamagePenaltyService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public ThirdPartyFinancialAndBodyHistoryDamagePenalty GetById(int? id, bool isFinancial)
        {
            var qureResult = db.ThirdPartyFinancialAndBodyHistoryDamagePenalties.Where(t => t.Id == id && t.IsActive == true );
            if (isFinancial == true)
                qureResult = qureResult.Where(t => t.IsFinancial == true);
            else
                qureResult = qureResult.Where(t => t.IsFinancial == null || t.IsFinancial == false);
            return qureResult.AsNoTracking().FirstOrDefault();
        }

        public object GetLightListForBody()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.ThirdPartyFinancialAndBodyHistoryDamagePenalties.Where(t => t.IsActive == true && t.IsFinancial != true).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }

        public object GetLightListForFinancial()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.ThirdPartyFinancialAndBodyHistoryDamagePenalties.Where(t => t.IsActive == true && t.IsFinancial == true).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
