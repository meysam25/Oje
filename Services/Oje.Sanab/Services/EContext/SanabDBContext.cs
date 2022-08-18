using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;
using Oje.Sanab.Models.DB;

namespace Oje.Sms.Services.EContext
{
    public class SanabDBContext : MyBaseDbContext
    {
        public SanabDBContext
            (
                DbContextOptions<SanabDBContext> options
            ) : base(options)
        {
        }

        public DbSet<SanabUser> SanabUsers { get; set; }
        public DbSet<SanabSystemFieldVehicleSystem> SanabSystemFieldVehicleSystems { get; set; }
        public DbSet<VehicleSystem> VehicleSystems { get; set; }
        public DbSet<VehicleSpec> VehicleSpecs { get; set; }
        public DbSet<SanabTypeFieldVehicleSpec> SanabTypeFieldVehicleSpecs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<SanabCompany> SanabCompanies { get; set; }
        public DbSet<SanabCarThirdPartyPlaqueInquiry> SanabCarThirdPartyPlaqueInquiries { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<SanabVehicleType> SanabVehicleTypes { get; set; }
        public DbSet<CarType> CarTypes { get; set; }
        public DbSet<SanabCarType> SanabCarTypes { get; set; }
        public DbSet<ThirdPartyFinancialAndBodyHistoryDamagePenalty> ThirdPartyFinancialAndBodyHistoryDamagePenalties { get; set; }
        public DbSet<ThirdPartyDriverHistoryDamagePenalty> ThirdPartyDriverHistoryDamagePenalties { get; set; }
        public DbSet<ThirdPartyDriverNoDamageDiscountHistory> ThirdPartyDriverNoDamageDiscountHistories { get; set; }
        public DbSet<ThirdPartyBodyNoDamageDiscountHistory> ThirdPartyBodyNoDamageDiscountHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SanabCarThirdPartyPlaqueInquiry>().Property(e => e.Tonage).HasPrecision(8, 2);
            base.OnModelCreating(modelBuilder);
        }
    }
}
