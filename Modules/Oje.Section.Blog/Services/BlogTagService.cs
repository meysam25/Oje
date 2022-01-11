using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Blog.Interfaces;
using Oje.Section.Blog.Models.DB;
using Oje.Section.Blog.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Services
{
    public class BlogTagService : IBlogTagService
    {
        readonly BlogDBContext db = null;
        public BlogTagService(BlogDBContext db)
        {
            this.db = db;
        }

        public long CreateIfNotExist(string tag, int? siteSettingId)
        {
            long result = 0;

            if (!string.IsNullOrEmpty(tag))
            {
                result = db.BlogTags.Where(t => t.Title == tag && t.SiteSettingId == siteSettingId).Select(t => t.Id).FirstOrDefault();
                if (result <= 0)
                    result = Create(tag, siteSettingId);
            }

            return result;
        }

        private long Create(string tag, int? siteSettingId)
        {
            BlogTag newItem = new BlogTag() { Title = tag, SiteSettingId = siteSettingId.ToIntReturnZiro() };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            return newItem.Id;
        }

        public object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.BlogTags.OrderByDescending(t => t.Id).AsQueryable();
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            var resultList = qureResult.Select(t => new { id = t.Title, text = t.Title }).ToList();

            if (!resultList.Any(t => t.text == searchInput.search))
                result.Add(new { id = searchInput.search, text = searchInput.search });

            result.AddRange(resultList);

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}
