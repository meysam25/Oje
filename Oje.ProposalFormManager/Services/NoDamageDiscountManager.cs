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
    public class NoDamageDiscountManager : INoDamageDiscountManager
    {
        readonly ProposalFormDBContext db = null;
        public NoDamageDiscountManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public NoDamageDiscount GetByFormId(int? ProposalFormId, int? Id)
        {
            return db.NoDamageDiscounts
                .Include(t => t.NoDamageDiscountCompanies)
                .OrderBy(t => t.Order)
                .Where(t => t.ProposalFormId == ProposalFormId && t.Id == Id && t.IsActive == true)
                .AsNoTracking()
                .FirstOrDefault();
        }

        public object GetLightList(int? ProposalFormId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            if (ProposalFormId.ToIntReturnZiro() > 0)
                result.AddRange(
                    db.NoDamageDiscounts
                    .Where(t => t.IsActive == true && t.ProposalFormId == ProposalFormId)
                    .Select(t => new { id = t.Id, title = t.Title }).ToList()
                    );

            return result;
        }
    }
}
