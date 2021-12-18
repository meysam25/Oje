using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Services.EContext
{
    public class SmsDBContext : DbContext
    {
        public SmsDBContext(DbContextOptions<SmsDBContext> options) : base(options)
        {

        }

        public DbSet<Models.DB.SmsConfig> SmsConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
