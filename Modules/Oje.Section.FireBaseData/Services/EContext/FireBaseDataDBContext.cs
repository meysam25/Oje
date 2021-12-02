using Oje.Section.FireBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Services.EContext
{
    public class FireBaseDataDBContext : DbContext
    {
        public FireBaseDataDBContext(DbContextOptions<FireBaseDataDBContext> options) : base(options)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FireInsuranceCoverage>().Property(e => e.Rate).HasPrecision(9, 8);
            modelBuilder.Entity<FireInsuranceRate>().Property(e => e.Rate).HasPrecision(9, 8);
            modelBuilder.Entity<FireInsuranceCoverageActivityDangerLevel>().Property(e => e.Rate).HasPrecision(9, 8);
            modelBuilder.Entity<FireInsuranceCoverageCityDangerLevel>().Property(e => e.Rate).HasPrecision(9, 8);

            base.OnModelCreating(modelBuilder);
        }
    }
}
