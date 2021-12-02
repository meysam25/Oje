using Oje.Infrastructure.Models;
using Oje.Section.CarBaseData.Interfaces;
using Oje.Section.CarBaseData.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.CarBaseData.Services
{
    public class ProposalFormManager: IProposalFormManager
    {
        readonly CarDBContext db = null;
        public ProposalFormManager(CarDBContext db)
        {
            this.db = db;
        }

        public object GetSelect2List(Select2SearchVM searchInput)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.ProposalForms.OrderByDescending(t => t.Id).AsQueryable();
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}
