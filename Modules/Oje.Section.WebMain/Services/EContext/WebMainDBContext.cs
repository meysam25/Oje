using Microsoft.EntityFrameworkCore;
using Oje.Section.WebMain.Models.DB;

namespace Oje.Section.WebMain.Services.EContext
{
    public class WebMainDBContext : DbContext
    {
        public DbSet<TopMenu> TopMenus { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageLeftRightDesign> PageLeftRightDesigns { get; set; }
        public DbSet<PageLeftRightDesignItem> PageLeftRightDesignItems { get; set; }
        public DbSet<FooterExteraLink> FooterExteraLinks { get; set; }
        public DbSet<FooterGroupExteraLink> FooterGroupExteraLinks { get; set; }
        public DbSet<ContactUs> ContactUses { get; set; }
        public DbSet<OurObject> OurObjects { get; set; }
        public DbSet<AutoAnswerOnlineChatMessage> AutoAnswerOnlineChatMessages { get; set; }
        public DbSet<AutoAnswerOnlineChatMessageLike> AutoAnswerOnlineChatMessageLikes { get; set; }
        public DbSet<SubscribeEmail> SubscribeEmails { get; set; }

        public WebMainDBContext(DbContextOptions<WebMainDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactUs>().HasKey(t => new { t.CreateDate, t.Ip1, t.Ip2, t.Ip3, t.Ip4 });
            modelBuilder.Entity<AutoAnswerOnlineChatMessageLike>().HasKey(t => new { t.AutoAnswerOnlineChatMessageId, t.SiteSettingId, t.Ip1, t.Ip2, t.Ip3, t.Ip4 });

            base.OnModelCreating(modelBuilder);
        }
    }
}
