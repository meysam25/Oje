using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Services
{
    public class ThirdPartyBodyNoDamageDiscountHistoryManager: IThirdPartyBodyNoDamageDiscountHistoryManager
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyBodyNoDamageDiscountHistoryManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public ThirdPartyBodyNoDamageDiscountHistory GetByIdActived(int? id)
        {
            return db.ThirdPartyBodyNoDamageDiscountHistories.Where(t => t.IsActive == true && t.Id == id).AsNoTracking().FirstOrDefault();
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(
                db.ThirdPartyBodyNoDamageDiscountHistories
                .Where(t => t.IsActive == true)
                .Select(t => new { id = t.Id, title = t.Title }).ToList()
                );

            return result;
        }
    }
}
