using Oje.Section.CarBodyBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBodyBaseData.Services.EContext
{
    public class CarBodyDBContext : DbContext
    {
        public CarBodyDBContext(DbContextOptions<CarBodyDBContext> options) : base(options)
        {

        }

        public DbSet<CarSpecification> CarSpecifications { get; set; }
        public DbSet<CarSpecificationAmount> CarSpecificationAmounts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CarSpecificationAmountCompany> CarSpecificationAmountCompanies { get; set; }
        public DbSet<CarBodyCreateDatePercent> CarBodyCreateDatePercents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarSpecificationAmount>().Property(e => e.Rate).HasPrecision(7, 5);

            base.OnModelCreating(modelBuilder);
        }

    }
}
