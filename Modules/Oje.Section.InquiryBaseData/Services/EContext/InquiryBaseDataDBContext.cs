using Oje.Section.InquiryBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Services.EContext
{
    public class InquiryBaseDataDBContext : DbContext
    {
        public InquiryBaseDataDBContext(DbContextOptions<InquiryBaseDataDBContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<InquiryDuration> InquiryDurations { get; set; }
        public DbSet<ProposalForm> ProposalForms { get; set; }
        public DbSet<CashPayDiscount> CashPayDiscounts { get; set; }
        public DbSet<InquiryMaxDiscount> InquiryMaxDiscounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GlobalDiscount> GlobalDiscounts { get; set; }
        public DbSet<InsuranceContractDiscount> InsuranceContractDiscounts { get; set; }
        public DbSet<InsuranceContract> InsuranceContracts { get; set; }
        public DbSet<InquiryCompanyLimit> InquiryCompanyLimits { get; set; }
        public DbSet<InquiryCompanyLimitCompany> InquiryCompanyLimitCompanies { get; set; }
        public DbSet<RoundInquery> RoundInqueries { get; set; }
        public DbSet<NoDamageDiscount> NoDamageDiscounts { get; set; }
        public DbSet<InqueryDescription> InqueryDescriptions { get; set; }
        public DbSet<SiteSetting> SiteSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InquiryCompanyLimitCompany>().HasKey(t => new { t.CompanyId, t.InquiryCompanyLimitId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
