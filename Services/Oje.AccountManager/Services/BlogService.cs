using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.AccountService.Services
{
    public class BlogService: IBlogService
    {
        readonly AccountDBContext db = null;

        public BlogService(AccountDBContext db)
        {
            this.db = db;
        }

        public List<Blog> GetMainBlog(int count, int? siteSettingId)
        {
            return db
                .Blogs
                .Where(t => t.IsActive == true && t.PublisheDate <= DateTime.Now && t.SiteSettingId == siteSettingId)
                .OrderByDescending(t => t.Id)
                .Take(count)
                .AsNoTracking()
                .ToList();
        }
    }
}
