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
    public class ThirdPartyDriverHistoryDamagePenaltyManager: IThirdPartyDriverHistoryDamagePenaltyManager
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyDriverHistoryDamagePenaltyManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public ThirdPartyDriverHistoryDamagePenalty GetById(int? id)
        {
            return db.ThirdPartyDriverHistoryDamagePenalties.Where(t => t.Id == id && t.IsActive == true).AsNoTracking().FirstOrDefault();
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

                result.AddRange(
                    db.ThirdPartyDriverHistoryDamagePenalties
                    .Where(t => t.IsActive == true)
                    .Select(t => new { id = t.Id, title = t.Title }).ToList()
                    );

            return result;
        }
    }
}
