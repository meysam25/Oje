using Microsoft.EntityFrameworkCore;
using Oje.Section.Blog.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Services.EContext
{
    public class BlogDBContext : DbContext
    {
        public BlogDBContext(DbContextOptions<BlogDBContext> options) : base(options)
        {

        }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<Models.DB.Blog> Blogs { get; set; }
        public DbSet<BlogBlog> BlogBlogs { get; set; }
        public DbSet<BlogLastLikeAndView> BlogLastLikeAndViews { get; set; }
        public DbSet<BlogReview> BlogReviews { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<BlogTagBlog> BlogTagBlogs { get; set; }
        public DbSet<User> Users { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<BlogLastLikeAndView>().HasKey(t => new { t.Ip1, t.Ip2, t.Ip3, t.Ip4, t.BlogId, t.Type });
            modelBuilder.Entity<BlogReview>().HasKey(t => new { t.Ip1, t.Ip2, t.Ip3, t.Ip4, t.BlogId, t.CreateDate });
            modelBuilder.Entity<BlogBlog>().HasKey(t => new { t.BlogOwnId, t.BlogRelatedId });
            modelBuilder.Entity<BlogTagBlog>().HasKey(t => new { t.BlogId, t.BlogTagId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
