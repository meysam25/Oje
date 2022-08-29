using Microsoft.EntityFrameworkCore;
using Oje.Security.Interfaces;
using Oje.Security.Models.DB;
using Oje.Security.Services.EContext;

namespace Oje.Security.Services
{
    public class DebugInfoService : IDebugInfoService
    {
        readonly SecurityDBContext db = null;
        public DebugInfoService
            (
                SecurityDBContext db
            )
        {
            this.db = db;
        }

        public void Create(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                db.Entry(new DebugInfo()
                {
                    Description = input
                }).State = EntityState.Added;
                db.SaveChanges();
            }
        }
    }
}
