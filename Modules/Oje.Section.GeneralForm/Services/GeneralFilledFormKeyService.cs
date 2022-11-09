using Oje.Infrastructure.Models;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Models.DB;
using Oje.Section.GlobalForms.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.GlobalForms.Services
{
    public class GeneralFilledFormKeyService: IGeneralFilledFormKeyService
    {
        readonly GeneralFormDBContext db = null;
        static Dictionary<string, int> cacheKeyIds = new Dictionary<string, int>();

        public GeneralFilledFormKeyService
            (
                GeneralFormDBContext db
            )
        {
            this.db = db;
        }

        public int CreateIfNeeded(string name, string title)
        {
            if (string.IsNullOrEmpty(name))
                return 0;

            if (cacheKeyIds == null)
                cacheKeyIds = new Dictionary<string, int>();

            if (cacheKeyIds.ContainsKey(name))
                return cacheKeyIds[name];

            int result = db.GeneralFilledFormKeys.Where(t => t.Key == name).Select(t => t.Id).FirstOrDefault();

            if (result > 0)
            {
                cacheKeyIds[name] = result;
                return result;
            }

            var newItem = new GeneralFilledFormKey() { Key = name, Title = !string.IsNullOrEmpty(title) ? title : "خالی" };
            db.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();

            cacheKeyIds[name] = newItem.Id;
            return newItem.Id;
        }

        public object GetSelect2List(Select2SearchVM searchInput, long? generalFormId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.GeneralFilledFormKeys.Where(t => t.GeneralFilledFormValues.Any(tt => tt.GeneralFilledForm.GeneralFormId == generalFormId));
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Key.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title + "(" + t.Key + ")" }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}
