using Oje.Section.Secretariat.Models.DB;
using Oje.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Oje.Section.Secretariat.Services.EContext
{
    public class SecretariatDBContext : MyBaseDbContext
    {
        public SecretariatDBContext
            (
                DbContextOptions<SecretariatDBContext> options
            ) : base(options)
        {

        }

        public DbSet<SecretariatHeaderFooter> SecretariatHeaderFooters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SecretariatUserDigitalSignature> SecretariatUserDigitalSignatures { get; set; }
        public DbSet<SecretariatHeaderFooterDescription> SecretariatHeaderFooterDescriptions { get; set; }
        public DbSet<SecretariatLetterCategory> SecretariatLetterCategories { get; set; }
        public DbSet<SecretariatLetter> SecretariatLetters { get; set; }
        public DbSet<SecretariatLetterUser> SecretariatLetterUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<SecretariatLetterRecive> SecretariatLetterRecives { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
