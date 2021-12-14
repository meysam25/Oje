using Microsoft.EntityFrameworkCore;
using Oje.ProposalFormManager.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Services.EContext
{
    public class ProposalFormDBContext : DbContext
    {
        public ProposalFormDBContext(DbContextOptions<ProposalFormDBContext> options) : base(options)
        {

        }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<ProposalFormRequiredDocument> ProposalFormRequiredDocuments { get; set; }
        public DbSet<Duty> Duties { get; set; }
        public DbSet<Tax> Taxs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ProposalFilledFormCompany> ProposalFilledFormCompanies { get; set; }
        public DbSet<ProposalFilledFormUser> ProposalFilledFormUsers { get; set; }
        public DbSet<GlobalDiscount> GlobalDiscounts { get; set; }
        public DbSet<GlobalInputInquery> GlobalInputInqueries { get; set; }
        public DbSet<GlobalInquery> GlobalInqueries { get; set; }
        public DbSet<GlobalInqueryInputParameter> GlobalInqueryInputParameters { get; set; }
        public DbSet<GlobalInquiryItem> GlobalInquiryItems { get; set; }
        public DbSet<ProposalFilledForm> ProposalFilledForms { get; set; }
        public DbSet<ProposalFilledFormKey> ProposalFilledFormKeys { get; set; }
        public DbSet<ProposalFilledFormValue> ProposalFilledFormValues { get; set; }
        public DbSet<ProposalForm> ProposalForms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ThirdPartyRate> ThirdPartyRates { get; set; }
        public DbSet<ThirdPartyRateCompany> ThirdPartyRateCompanies { get; set; }
        public DbSet<InquiryCompanyLimit> InquiryCompanyLimits { get; set; }
        public DbSet<CarExteraDiscount> CarExteraDiscounts { get; set; }
        public DbSet<NoDamageDiscount> NoDamageDiscounts { get; set; }
        public DbSet<ThirdPartyDriverNoDamageDiscountHistory> ThirdPartyDriverNoDamageDiscountHistories { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<VehicleSystem> VehicleSystems { get; set; }
        public DbSet<VehicleUsage> VehicleUsages { get; set; }
        public DbSet<ThirdPartyFinancialAndBodyHistoryDamagePenalty> ThirdPartyFinancialAndBodyHistoryDamagePenalties { get; set; }
        public DbSet<ThirdPartyDriverHistoryDamagePenalty> ThirdPartyDriverHistoryDamagePenalties { get; set; }
        public DbSet<CashPayDiscount> CashPayDiscounts { get; set; }
        public DbSet<ThirdPartyFinancialCommitment> ThirdPartyFinancialCommitments { get; set; }
        public DbSet<ThirdPartyLifeCommitment> ThirdPartyLifeCommitments { get; set; }
        public DbSet<ThirdPartyDriverFinancialCommitment> ThirdPartyDriverFinancialCommitments { get; set; }
        public DbSet<ThirdPartyPassengerRate> ThirdPartyPassengerRates { get; set; }
        public DbSet<RoundInquery> RoundInqueries { get; set; }
        public DbSet<CarSpecification> CarSpecifications { get; set; }
        public DbSet<ThirdPartyCarCreateDatePercent> ThirdPartyCarCreateDatePercents { get; set; }
        public DbSet<InquiryMaxDiscount> InquiryMaxDiscounts { get; set; }
        public DbSet<ThirdPartyRequiredFinancialCommitment> ThirdPartyRequiredFinancialCommitments { get; set; }
        public DbSet<InsuranceContractDiscount> InsuranceContractDiscounts { get; set; }
        public DbSet<InquiryDuration> InquiryDurations { get; set; }
        public DbSet<InqueryDescription> InqueryDescriptions { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<ThirdPartyBodyNoDamageDiscountHistory> ThirdPartyBodyNoDamageDiscountHistories { get; set; }
        public DbSet<ProposalFilledFormJson> ProposalFilledFormJsons { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ProposalFilledFormDocument> ProposalFilledFormDocuments { get; set; }
        public DbSet<CarBodyCreateDatePercent> CarBodyCreateDatePercents { get; set; }
        public DbSet<CarSpecificationAmount> CarSpecificationAmounts { get; set; }
        public DbSet<ProposalFormCategory> ProposalFormCategories { get; set; }
        public DbSet<ProposalFilledFormStatusLog> ProposalFilledFormStatusLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ThirdPartyRate>().Property(e => e.Rate).HasPrecision(17, 15);
            modelBuilder.Entity<ThirdPartyExteraFinancialCommitment>().Property(e => e.Rate).HasPrecision(17, 15);
            modelBuilder.Entity<ThirdPartyPassengerRate>().Property(e => e.Rate).HasPrecision(8, 6);
            modelBuilder.Entity<CarSpecification>().Property(e => e.CarRoomRate).HasPrecision(10, 8);
            modelBuilder.Entity<VehicleUsage>().Property(e => e.BodyPercent).HasPrecision(5, 3);
            modelBuilder.Entity<VehicleUsage>().Property(e => e.ThirdPartyPercent).HasPrecision(5, 3);
            modelBuilder.Entity<CarExteraDiscountRangeAmount>().Property(e => e.Percent).HasPrecision(5, 2);
            modelBuilder.Entity<CarSpecificationAmount>().Property(e => e.Rate).HasPrecision(7, 5);

            modelBuilder.Entity<InquiryCompanyLimitCompany>().HasKey(t => new { t.CompanyId, t.InquiryCompanyLimitId });
            modelBuilder.Entity<ProposalFilledFormStatusLog>().HasKey(t => new { t.ProposalFilledFormId, t.Type, t.CreateDate });
            modelBuilder.Entity<UserCompany>().HasKey(t => new { t.CompanyId, t.UserId });
            modelBuilder.Entity<ProposalFilledFormCompany>().HasKey(t => new { t.CompanyId, t.ProposalFilledFormId });
            modelBuilder.Entity<ProposalFilledFormUser>().HasKey(t => new { t.ProposalFilledFormId, t.UserId, t.Type });
            modelBuilder.Entity<PaymentMethodCompany>().HasKey(t => new { t.CompanyId, t.PaymentMethodId });

            base.OnModelCreating(modelBuilder);
        }

    }
}
