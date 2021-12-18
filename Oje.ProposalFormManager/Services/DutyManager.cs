using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class DutyService: IDutyService
    {
        readonly ProposalFormDBContext db = null;
        public DutyService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public Duty GetLastItem()
        {
            return db.Duties.Where(t => t.IsActive == true).OrderByDescending(t => t.Year).FirstOrDefault();
        }
    }
}
