using Oje.Section.CarBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Services.EContext
{
    public class CarDBContext : DbContext
    {
        public CarDBContext(DbContextOptions<CarDBContext> options) : base(options)
        {

        }

        public DbSet<CarType> CarTypes { get; set; }
        public DbSet<CarSpecification> CarSpecifications { get; set; }
        public DbSet<VehicleSystem> VehicleSystems { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<VehicleUsage> VehicleUsages { get; set; }
        public DbSet<CarExteraDiscount> CarExteraDiscounts { get; set; }
        public DbSet<ProposalForm> ProposalForms { get; set; }
        public DbSet<CarExteraDiscountValue> CarExteraDiscountValues { get; set; }
        public DbSet<CarExteraDiscountRangeAmount> CarExteraDiscountRangeAmounts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CarExteraDiscountCategory> CarExteraDiscountCategories { get; set; }
        public DbSet<VehicleSpecCategory> VehicleSpecCategories { get; set; }
        public DbSet<VehicleSpec> VehicleSpecs { get; set; }
        public DbSet<VehicleTypeCarType> VehicleTypeCarTypes { get; set; }
        public DbSet<VehicleSystemVehicleType> VehicleSystemVehicleTypes { get; set; }
        public DbSet<CarSpecificationVehicleSpec> CarSpecificationVehicleSpecs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarSpecification>().Property(e => e.CarRoomRate).HasPrecision(10, 8);
            modelBuilder.Entity<VehicleUsage>().Property(e => e.BodyPercent).HasPrecision(5, 3);
            modelBuilder.Entity<VehicleUsage>().Property(e => e.ThirdPartyPercent).HasPrecision(5, 3);
            modelBuilder.Entity<CarExteraDiscountRangeAmount>().Property(e => e.Percent).HasPrecision(5, 2);
            modelBuilder.Entity<VehicleTypeCarType>().HasKey(t => new { t.CarTypeId, t.VehicleTypeId });
            modelBuilder.Entity<VehicleSystemVehicleType>().HasKey(t => new { t.VehicleSystemId, t.VehicleTypeId });
            modelBuilder.Entity<CarSpecificationVehicleSpec>().HasKey(t => new { t.CarSpecificationId, t.VehicleSpecId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
