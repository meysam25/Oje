using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Services.EContext;

namespace Oje.AccountService.Services
{
    public class TempSqlCommService: ITempSqlCommService
    {
        readonly AccountDBContext db = null;
        public TempSqlCommService(AccountDBContext db)
        {
            this.db = db;
        }

        public void SetFlagForGooglePointPerformanceProblem()
        {
            db.Database.ExecuteSqlRaw("DBCC TRACEON (4326);DBCC TRACESTATUS;");
        }
    }
}
