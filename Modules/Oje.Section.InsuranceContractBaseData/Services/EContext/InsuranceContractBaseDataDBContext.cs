using Oje.Section.InsuranceContractBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Services.EContext
{
    public class InsuranceContractBaseDataDBContext: DbContext
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
        public DbSet<ProposalForm> ProposalForms { get; set; }
        public DbSet<InsuranceContract> InsuranceContracts { get; set; }
        public DbSet<InsuranceContractValidUserForFullDebit> InsuranceContractValidUserForFullDebits { get; set; }
        public DbSet<InsuranceContractUser> InsuranceContractUsers { get; set; }
    }
}
