using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Models.DB;

namespace Oje.Section.GlobalForms.Services.EContext
{
    public class GeneralFormDBContext : MyBaseDbContext
    {
        public GeneralFormDBContext(DbContextOptions<GeneralFormDBContext> options) : base(options)
        {

        }

        public DbSet<GeneralForm> GeneralForms { get; set; }
        public DbSet<SiteSetting> SiteSettings { get; set; }
        public DbSet<GeneralFormStatus> GeneralFormStatuses { get; set; }
        public DbSet<GeneralFormRequiredDocument> GeneralFormRequiredDocuments { get; set; }
        public DbSet<GeneralFilledForm> GeneralFilledForms { get; set; }
        public DbSet<GeneralFormCacheJson> GeneralFormCacheJsons { get; set; }
        public DbSet<GeneralFormJson> GeneralFormJsons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GeneralFilledFormKey> GeneralFilledFormKeys { get; set; }
        public DbSet<GeneralFilledFormValue> GeneralFilledFormValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
