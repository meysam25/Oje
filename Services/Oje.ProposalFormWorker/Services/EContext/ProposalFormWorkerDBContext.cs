using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;
using Oje.ProposalFormWorker.Models.DB;

namespace Oje.ProposalFormWorker.Services.EContext
{
    public class ProposalFormWorkerDBContext : MyBaseDbContext
    {
        public ProposalFormWorkerDBContext(DbContextOptions<ProposalFormWorkerDBContext> options) : base(options)
        {

        }

        public DbSet<ProposalFormReminder> ProposalFormReminders { get; set; }
        public DbSet<ProposalForm> ProposalForms { get; set; }
    }
}
