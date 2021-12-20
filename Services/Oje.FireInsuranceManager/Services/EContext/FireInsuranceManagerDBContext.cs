using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Services.EContext
{
    public class FireInsuranceServiceDBContext : DbContext
    {
        public FireInsuranceServiceDBContext(DbContextOptions<FireInsuranceServiceDBContext> options) : base(options)
        {

        }

        public DbSet<FireInsuranceCoverageTitle> FireInsuranceCoverageTitles { get; set; }
        public DbSet<ProposalForm> ProposalForms { get; set; }
        public DbSet<FireInsuranceCoverage> FireInsuranceCoverages { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<FireInsuranceBuildingBody> FireInsuranceBuildingBodies { get; set; }
        public DbSet<FireInsuranceBuildingType> FireInsuranceBuildingTypes { get; set; }
        public DbSet<FireInsuranceRate> FireInsuranceRates { get; set; }
        public DbSet<FireInsuranceBuildingUnitValue> FireInsuranceBuildingUnitValues { get; set; }
        public DbSet<FireInsuranceTypeOfActivity> FireInsuranceTypeOfActivities { get; set; }
        public DbSet<FireInsuranceCoverageActivityDangerLevel> FireInsuranceCoverageActivityDangerLevels { get; set; }
        public DbSet<FireInsuranceCoverageCityDangerLevel> FireInsuranceCoverageCityDangerLevels { get; set; }
        public DbSet<FireInsuranceBuildingAge> FireInsuranceBuildingAges { get; set; }
        public DbSet<InquiryDuration> InquiryDurations { get; set; }
        public DbSet<InsuranceContractDiscount> InsuranceContractDiscounts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Tax> Taxs { get; set; }
        public DbSet<Duty> Duties { get; set; }
        public DbSet<InquiryMaxDiscount> InquiryMaxDiscounts { get; set; }
        public DbSet<InquiryCompanyLimit> InquiryCompanyLimits { get; set; }
        public DbSet<InquiryCompanyLimitCompany> InquiryCompanyLimitCompanies { get; set; }
        public DbSet<GlobalDiscount> GlobalDiscounts { get; set; }
        public DbSet<GlobalInputInquery> GlobalInputInqueries { get; set; }
        public DbSet<GlobalInquery> GlobalInqueries { get; set; }
        public DbSet<CashPayDiscount> CashPayDiscounts { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<RoundInquery> RoundInqueriess { get; set; }
        public DbSet<InqueryDescription> InqueryDescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FireInsuranceCoverage>().Property(e => e.Rate).HasPrecision(9, 8);
            modelBuilder.Entity<FireInsuranceRate>().Property(e => e.Rate).HasPrecision(9, 8);
            modelBuilder.Entity<FireInsuranceCoverageActivityDangerLevel>().Property(e => e.Rate).HasPrecision(9, 8);
            modelBuilder.Entity<FireInsuranceCoverageCityDangerLevel>().Property(e => e.Rate).HasPrecision(9, 8);

            modelBuilder.Entity<InquiryCompanyLimitCompany>().HasKey(t => new { t.CompanyId, t.InquiryCompanyLimitId });
            modelBuilder.Entity<PaymentMethodCompany>().HasKey(t => new { t.CompanyId, t.PaymentMethodId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
