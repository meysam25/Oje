using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Services
{
    public class ProposalFilledFormKeyManager: IProposalFilledFormKeyManager
    {
        readonly ProposalFormDBContext db = null;
        public ProposalFilledFormKeyManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public int CreateIfNeeded(string name)
        {
            if (string.IsNullOrEmpty(name))
                return 0;
            int result = db.ProposalFilledFormKeys.Where(t => t.Key == name).Select(t => t.Id).FirstOrDefault();

            if (result > 0)
                return result;

            var newItem = new ProposalFilledFormKey() { Key = name };
            db.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();

            return newItem.Id;
        }
    }
}
