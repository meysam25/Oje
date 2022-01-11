using Oje.Section.Blog.Interfaces;
using Oje.Section.Blog.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Services
{
    public class BlogLastLikeAndViewService: IBlogLastLikeAndViewService
    {
        readonly BlogDBContext db = null;
        public BlogLastLikeAndViewService(BlogDBContext db)
        {
            this.db = db;
        }
    }
}
