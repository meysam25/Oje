using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public class ProposalFormCategoryService : IProposalFormCategoryService
    {
        readonly TenderDBContext db = null;
        public ProposalFormCategoryService(TenderDBContext db)
        {
            this.db = db;
        }

        public object GetListLight()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.ProposalFormCategories.Where(t => t.ProposalForms.Any(tt => tt.TenderProposalFormJsonConfigs.Any(ttt => ttt.IsActive == true))).Select(tt => new { id = tt.Id, title = tt.Title }).ToList());

            return result;
        }
    }
}
