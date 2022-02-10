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
    public class ThirdPartyDriverNoDamageDiscountHistoryService: IThirdPartyDriverNoDamageDiscountHistoryService
    {
        readonly ProposalFormDBContext db = null;
        public ThirdPartyDriverNoDamageDiscountHistoryService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public ThirdPartyDriverNoDamageDiscountHistory GetById(int? id)
        {
            return db.ThirdPartyDriverNoDamageDiscountHistories.Where(t => t.Id == id).AsNoTracking().FirstOrDefault();
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "0", title = BMessages.No_Discount.GetEnumDisplayName() } };

            result.AddRange(
                db.ThirdPartyDriverNoDamageDiscountHistories
                .Where(t => t.IsActive == true)
                .Select(t => new { id = t.Id, title = t.Title }).ToList()
                );

            return result;
        }
    }
}
