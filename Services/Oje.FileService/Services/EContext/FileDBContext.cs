using Microsoft.EntityFrameworkCore;
using Oje.FileService.Models.DB;
using Oje.Infrastructure.Services;

namespace Oje.FileService.Services.EContext
{
    public class FileDBContext : MyBaseDbContext
    {
        public FileDBContext(DbContextOptions<FileDBContext> options) : base(options)
        {

        }

        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<FileAccessRole> FileAccessRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
