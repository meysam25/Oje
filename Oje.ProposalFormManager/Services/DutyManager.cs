using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class DutyManager: IDutyManager
    {
        readonly ProposalFormDBContext db = null;
        public DutyManager(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public Duty GetLastItem()
        {
            return db.Duties.Where(t => t.IsActive == true).OrderByDescending(t => t.Year).FirstOrDefault();
        }
    }
}
