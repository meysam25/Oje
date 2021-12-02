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
    public class ThirdPartyDriverNoDamageDiscountHistoryManager: IThirdPartyDriverNoDamageDiscountHistoryManager
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyDriverNoDamageDiscountHistoryManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public ThirdPartyDriverNoDamageDiscountHistory GetById(int? id)
        {
            return db.ThirdPartyDriverNoDamageDiscountHistories.Where(t => t.Id == id).AsNoTracking().FirstOrDefault();
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(
                db.ThirdPartyDriverNoDamageDiscountHistories
                .Where(t => t.IsActive == true)
                .Select(t => new { id = t.Id, title = t.Title }).ToList()
                );

            return result;
        }
    }
}
