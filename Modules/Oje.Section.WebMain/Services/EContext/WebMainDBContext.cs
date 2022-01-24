using Microsoft.EntityFrameworkCore;
using Oje.Section.WebMain.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Services.EContext
{
    public class WebMainDBContext: DbContext
    {
        public DbSet<TopMenu> TopMenus { get; set; }

        public WebMainDBContext(DbContextOptions<WebMainDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
