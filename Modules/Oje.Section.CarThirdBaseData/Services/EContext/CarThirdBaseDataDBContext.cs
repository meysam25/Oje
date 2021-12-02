using Oje.Section.CarThirdBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Services.EContext
{
    public class CarThirdBaseDataDBContext : DbContext
    {
        public CarThirdBaseDataDBContext(DbContextOptions<CarThirdBaseDataDBContext> options) : base(options)
        {

        }

        public DbSet<ThirdPartyRate> ThirdPartyRates { get; set; }
        public DbSet<CarSpecification> CarSpecifications { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ThirdPartyRateCompany> ThirdPartyRateCompanies { get; set; }
        public DbSet<ThirdPartyFinancialCommitment> ThirdPartyFinancialCommitments { get; set; }
        public DbSet<ThirdPartyLifeCommitment> ThirdPartyLifeCommitments { get; set; }
        public DbSet<ThirdPartyRequiredFinancialCommitment> ThirdPartyRequiredFinancialCommitments { get; set; }
        public DbSet<ThirdPartyExteraFinancialCommitment> ThirdPartyExteraFinancialCommitments { get; set; }
        public DbSet<ThirdPartyExteraFinancialCommitmentCom> ThirdPartyExteraFinancialCommitmentComs { get; set; }
        public DbSet<ThirdPartyDriverFinancialCommitment> ThirdPartyDriverFinancialCommitments { get; set; }
        public DbSet<ThirdPartyPassengerRate> ThirdPartyPassengerRates { get; set; }
        public DbSet<ThirdPartyPassengerRateCompany> ThirdPartyPassengerRateCompanies { get; set; }
        public DbSet<ThirdPartyCarCreateDatePercent> ThirdPartyCarCreateDatePercents { get; set; }
        public DbSet<ThirdPartyDriverHistoryDamagePenalty> ThirdPartyDriverHistoryDamagePenalties { get; set; }
        public DbSet<ThirdPartyFinancialAndBodyHistoryDamagePenalty> ThirdPartyFinancialAndBodyHistoryDamagePenalties { get; set; }
        public DbSet<ThirdPartyDriverNoDamageDiscountHistory> ThirdPartyDriverNoDamageDiscountHistories { get; set; }
        public DbSet<ThirdPartyBodyNoDamageDiscountHistory> ThirdPartyBodyNoDamageDiscountHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ThirdPartyRate>().Property(e => e.Rate).HasPrecision(17, 15);
            modelBuilder.Entity<ThirdPartyExteraFinancialCommitment>().Property(e => e.Rate).HasPrecision(17, 15);
            modelBuilder.Entity<ThirdPartyPassengerRate>().Property(e => e.Rate).HasPrecision(8, 6);

            base.OnModelCreating(modelBuilder);
        }
    }
}
