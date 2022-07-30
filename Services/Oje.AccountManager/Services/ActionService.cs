using Oje.AccountService.Interfaces;
using Oje.AccountService.Services.EContext;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Services
{
    public class ActionService : IActionService
    {
        readonly AccountDBContext db = null;
        public ActionService(AccountDBContext db)
        {
            this.db = db;
        }

        public object GetightListForSelect2(Select2SearchVM searchInput, int? siteSettingId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.Actions.Where(t => t.Icon == "fa-cog").OrderByDescending(t => t.Id).AsQueryable();
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => (t.Controller.Section.Title + "/" + t.Controller.Title + "/" + t.Title + "(" + t.Name + ")").Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Controller.Section.Title + "/" + t.Controller.Title + "/" + t.Title + "(" + t.Name + ")" }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}
