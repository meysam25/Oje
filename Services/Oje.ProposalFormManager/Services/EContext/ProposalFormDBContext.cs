﻿using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Models.DB;

namespace Oje.ProposalFormService.Services.EContext
{
    public class ProposalFormDBContext : MyBaseDbContext
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
        public DbSet<VehicleTypeCarType> VehicleTypeCarTypes { get; set; }
        public DbSet<VehicleSpec> VehicleSpecs { get; set; }
        public DbSet<CarType> CarTypes { get; set; }
        public DbSet<ProposalFormReminder> ProposalFormReminders { get; set; }
        public DbSet<ProposalFormPrintDescrption> ProposalFormPrintDescrptions { get; set; }
        public DbSet<AgentReffer> AgentReffers { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProposalFilledFormCacheJson> ProposalFilledFormCacheJsons { get; set; }
        public DbSet<ProposalFilledFormStatusLogFile> ProposalFilledFormStatusLogFiles { get; set; }
        public DbSet<ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscount> ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscounts { get; set; }
        public DbSet<ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompany> ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies { get; set; }
        public DbSet<ProposalFilledFormSiteSetting> ProposalFilledFormSiteSettings { get; set; }

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
            modelBuilder.Entity<CarExteraDiscountRangeAmount>().Property(e => e.CreateDateSelfPercent).HasPrecision(5, 2);

            modelBuilder.Entity<InquiryCompanyLimitCompany>().HasKey(t => new { t.CompanyId, t.InquiryCompanyLimitId });
            modelBuilder.Entity<ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompany>().HasKey(t => new { t.CompanyId, t.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountId });
            modelBuilder.Entity<UserCompany>().HasKey(t => new { t.CompanyId, t.UserId });
            modelBuilder.Entity<ProposalFilledFormCompany>().HasKey(t => new { t.CompanyId, t.ProposalFilledFormId });
            modelBuilder.Entity<ProposalFilledFormUser>().HasKey(t => new { t.ProposalFilledFormId, t.UserId, t.Type });
            modelBuilder.Entity<PaymentMethodCompany>().HasKey(t => new { t.CompanyId, t.PaymentMethodId });
            modelBuilder.Entity<VehicleTypeCarType>().HasKey(t => new { t.CarTypeId, t.VehicleTypeId });
            modelBuilder.Entity<VehicleSystemVehicleType>().HasKey(t => new { t.VehicleSystemId, t.VehicleTypeId });
            modelBuilder.Entity<CarSpecificationVehicleSpec>().HasKey(t => new { t.CarSpecificationId, t.VehicleSpecId });
            modelBuilder.Entity<ProposalFilledFormSiteSetting>().HasKey(t => new { t.SiteSettingId, t.ProposalFilledFormId });

            base.OnModelCreating(modelBuilder);
        }

    }
}
