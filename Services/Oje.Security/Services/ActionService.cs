using Oje.Infrastructure.Models;
using Oje.Security.Interfaces;
using Oje.Security.Services.EContext;

namespace Oje.Security.Services
{
    public class ActionService : IActionService
    {
        readonly SecurityDBContext db = null;
        public ActionService
            (
                SecurityDBContext db
            )
        {
            this.db = db;
        }

        public List<long> GetList()
        {
            return db.Actions.OrderByDescending(t => t.Id).Select(t => t.Id).ToList();
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

            var qureResult = db.Actions.OrderByDescending(t => t.Id).AsQueryable();
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => (t.Controller.Section.Title + "/" + t.Controller.Title + "/" + t.Title).Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Controller.Section.Title + "/" + t.Controller.Title + "/" + t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}
