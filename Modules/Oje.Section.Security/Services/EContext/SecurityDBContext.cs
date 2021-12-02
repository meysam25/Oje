using Microsoft.EntityFrameworkCore;
using Oje.Section.Security.Models.DB;

namespace Oje.Section.Security.Services.EContext
{
    public class SecurityDBContext: DbContext
    {
        public SecurityDBContext(DbContextOptions<SecurityDBContext> options) : base(options)
        {

        }
        public DbSet<FileAccessRole> FileAccessRoles { get; set; }
        public DbSet<IpLimitationWhiteList> IpLimitationWhiteLists { get; set; }
        public DbSet<IpLimitationBlackList> IpLimitationBlackLists { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
}
