using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class TaxService: ITaxService
    {
        readonly ProposalFormDBContext db = null;
        public TaxService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public Tax GetLastItem()
        {
            return db.Taxs.OrderByDescending(t => t.Year).Where(t => t.IsActive == true).AsNoTracking().FirstOrDefault();
        }
    }
}
