using Oje.Section.BaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;

namespace Oje.Section.BaseData.Services.EContext
{
    public class BaseDataDBContext : MyBaseDbContext
    {
        public BaseDataDBContext(DbContextOptions<BaseDataDBContext> options) : base(options)
        {

        }

        public DbSet<SiteSetting> SiteSettings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<JobDangerLevel> JobDangerLevels { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Tax> Taxs { get; set; }
        public DbSet<Duty> Duties { get; set; }
        public DbSet<ProposalForm> ProposalForms { get; set; }
    }
}
