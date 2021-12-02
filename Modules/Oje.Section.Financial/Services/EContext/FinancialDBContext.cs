using Oje.Section.Financial.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace Oje.Section.Financial.Services.EContext
{
    public class FinancialDBContext: DbContext
    {
        public FinancialDBContext(DbContextOptions<FinancialDBContext> options) : base(options)
        {

        }

        public DbSet<Bank> Banks { get; set; }
    }
}
