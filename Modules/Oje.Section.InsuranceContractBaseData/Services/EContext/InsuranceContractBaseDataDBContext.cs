using Oje.Section.InsuranceContractBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace Oje.Section.InsuranceContractBaseData.Services.EContext
{
    public class InsuranceContractBaseDataDBContext : DbContext
    {
        public InsuranceContractBaseDataDBContext
            (
                DbContextOptions<InsuranceContractBaseDataDBContext> options
            ) : base(options)
        {

        }

        public DbSet<InsuranceContractType> InsuranceContractTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<InsuranceContractCompany> InsuranceContractCompanies { get; set; }
        public DbSet<InsuranceContract> InsuranceContracts { get; set; }
        public DbSet<InsuranceContractValidUserForFullDebit> InsuranceContractValidUserForFullDebits { get; set; }
        public DbSet<InsuranceContractUser> InsuranceContractUsers { get; set; }
        public DbSet<InsuranceContractInsuranceContractType> InsuranceContractInsuranceContractTypes { get; set; }
        public DbSet<InsuranceContractProposalForm> InsuranceContractProposalForms { get; set; }
        public DbSet<ProposalForm> ProposalForms { get; set; }
        public DbSet<InsuranceContractTypeRequiredDocument> InsuranceContractTypeRequiredDocuments { get; set; }
        public DbSet<InsuranceContractProposalFilledForm> InsuranceContractProposalFilledForms { get; set; }
        public DbSet<InsuranceContractProposalFilledFormStatusLog> InsuranceContractProposalFilledFormStatusLogs { get; set; }
        public DbSet<InsuranceContractProposalFilledFormKey> InsuranceContractProposalFilledFormKeys { get; set; }
        public DbSet<InsuranceContractProposalFilledFormValue> InsuranceContractProposalFilledFormValues { get; set; }
        public DbSet<InsuranceContractProposalFilledFormJson> InsuranceContractProposalFilledFormJsons { get; set; }
        public DbSet<InsuranceContractProposalFilledFormUser> InsuranceContractProposalFilledFormUsers { get; set; }
        public DbSet<InsuranceContractUserSubCategory> InsuranceContractUserSubCategories { get; set; }
        public DbSet<InsuranceContractUserBaseInsurance> InsuranceContractUserBaseInsurances { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InsuranceContractInsuranceContractType>().HasKey(t => new { t.InsuranceContractTypeId, t.InsuranceContractId });
            modelBuilder.Entity<InsuranceContractProposalFilledFormStatusLog>().HasKey(t => new { t.InsuranceContractProposalFilledFormId, t.Status, t.CreateDate });
            modelBuilder.Entity<InsuranceContractProposalFilledFormUser>().HasKey(t => new { t.InsuranceContractProposalFilledFormId, t.InsuranceContractUserId, t.InsuranceContractTypeId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
