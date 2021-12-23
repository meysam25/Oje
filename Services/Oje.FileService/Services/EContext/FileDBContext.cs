using Microsoft.EntityFrameworkCore;
using Oje.FileService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FileService.Services.EContext
{
    public class FileDBContext: DbContext
    {
        public FileDBContext(DbContextOptions<FileDBContext> options) : base(options)
        {

        }

        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<FileAccessRole> FileAccessRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
