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

        public int CreateIfNeeded(string name)
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

            var newItem = new GeneralFilledFormKey() { Key = name };
            db.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();

            cacheKeyIds[name] = newItem.Id;
            return newItem.Id;
        }
    }
}
