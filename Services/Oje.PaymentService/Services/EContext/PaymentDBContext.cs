using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Models.DB;

namespace Oje.PaymentService.Services.EContext
{
    public class PaymentDBContext : MyBaseDbContext
    {
        public PaymentDBContext(DbContextOptions<PaymentDBContext> options) : base(options)
        {

        }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankAccountSizpay> BankAccountSizpaies { get; set; }
        public DbSet<BankAccountFactor> BankAccountFactors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProposalFilledForm> ProposalFilledForms { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccountFactor>().HasKey(t => new { t.BankAccountId, t.Type, t.ObjectId, t.CreateDate });

            base.OnModelCreating(modelBuilder);
        }
    }
}
