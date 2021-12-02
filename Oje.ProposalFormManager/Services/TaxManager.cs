using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class TaxManager: ITaxManager
    {
        readonly ProposalFormDBContext db = null;
        public TaxManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public Tax GetLastItem()
        {
            return db.Taxs.OrderByDescending(t => t.Year).Where(t => t.IsActive == true).AsNoTracking().FirstOrDefault();
        }
    }
}
