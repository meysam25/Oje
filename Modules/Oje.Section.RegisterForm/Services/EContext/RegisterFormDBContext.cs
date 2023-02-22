using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Models.DB;

namespace Oje.Section.RegisterForm.Services.EContext
{
    public class RegisterFormDBContext : MyBaseDbContext
    {
        public RegisterFormDBContext(DbContextOptions<RegisterFormDBContext> options) : base(options)
        {

        }

        public DbSet<UserRegisterForm> UserRegisterForms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRegisterFormRequiredDocumentType> UserRegisterFormRequiredDocumentTypes { get; set; }
        public DbSet<UserRegisterFormRequiredDocument> UserRegisterFormRequiredDocuments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<UserFilledRegisterForm> UserFilledRegisterForms { get; set; }
        public DbSet<Provinc> Provincs { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<UserFilledRegisterFormJson> UserFilledRegisterFormJsons { get; set; }
        public DbSet<UserFilledRegisterFormKey> UserFilledRegisterFormKeys { get; set; }
        public DbSet<UserFilledRegisterFormValue> UserFilledRegisterFormValues { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<UserFilledRegisterFormCompany> UserFilledRegisterFormCompanies { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<AgentReffer> AgentReffers { get; set; }
        public DbSet<UserRegisterFormPrice> UserRegisterFormPrices { get; set; }
        public DbSet<UserFilledRegisterFormCardPayment> UserFilledRegisterFormCardPayments { get; set; }
        public DbSet<UserRegisterFormDiscountCode> UserRegisterFormDiscountCodes { get; set; }
        public DbSet<UserRegisterFormDiscountCodeUse> UserRegisterFormDiscountCodeUses { get; set; }
        public DbSet<UserRegisterFormCompany> UserRegisterFormCompanies { get; set; }
        public DbSet<UserRegisterFormPrintDescrption> UserRegisterFormPrintDescrptions { get; set; }
        public DbSet<UserRegisterFormRole> UserRegisterFormRoles { get; set; }
        public DbSet<UserRegisterFormCategory> UserRegisterFormCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCompany>().HasKey(t => new { t.UserId, t.CompanyId });
            modelBuilder.Entity<UserFilledRegisterFormCompany>().HasKey(t => new { t.CompanyId, t.UserFilledRegisterFormId });
            modelBuilder.Entity<UserRegisterFormRole>().HasKey(t => new { t.RoleId, t.UserRegisterFormId });

            modelBuilder.Entity<User>().Property(e => e.MapLat).HasPrecision(18, 15);
            modelBuilder.Entity<User>().Property(e => e.MapLon).HasPrecision(18, 15);

            base.OnModelCreating(modelBuilder);
        }
    }
}
