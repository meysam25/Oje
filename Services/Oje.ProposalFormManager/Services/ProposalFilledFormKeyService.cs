using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFilledFormKeyService: IProposalFilledFormKeyService
    {
        readonly ProposalFormDBContext db = null;
        static Dictionary<string, int> cacheKeyIds = new Dictionary<string, int>();

        public ProposalFilledFormKeyService(ProposalFormDBContext db)
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

            int result = db.ProposalFilledFormKeys.Where(t => t.Key == name).Select(t => t.Id).FirstOrDefault();

            if (result > 0)
            {
                cacheKeyIds[name] = result;
                return result;
            }

            var newItem = new ProposalFilledFormKey() { Key = name };
            db.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();
            newItem.FilledSignature();
            db.SaveChanges();

            cacheKeyIds[name] = newItem.Id;
            return newItem.Id;
        }
    }
}
