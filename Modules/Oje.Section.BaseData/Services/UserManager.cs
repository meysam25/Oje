using Oje.Infrastructure.Models;
using Oje.Section.BaseData.Interfaces;
using Oje.Section.BaseData.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.BaseData.Services
{
    public class UserService : IUserService
    {
        readonly BaseDataDBContext db = null;
        public UserService(BaseDataDBContext db)
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

            var qureResult = db.Users.OrderByDescending(t => t.Id).AsQueryable();
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => (t.Username + "(" + t.Firstname + " " + t.Lastname + ")").Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Username + "(" + t.Firstname + " " + t.Lastname + ")" }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}
