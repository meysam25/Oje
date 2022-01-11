using Oje.Infrastructure.Models;
using Oje.Section.Blog.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Interfaces
{
    public interface IBlogService
    {
        ApiResult Create(BlogCreateUpdateVM input, int? siteSettingId, long loginUserId);
        ApiResult Delete(long? id, int? siteSettingId);
        BlogVM GetById(long? id, int? siteSettingId);
        ApiResult Update(BlogCreateUpdateVM input, int? siteSettingId, long loginUserId);
        GridResultVM<BlogMainGridResultVM> GetList(BlogMainGrid searchInput, int? siteSettingId);
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId);
        object GetTopBlogs(int count, int? siteSettingId);
    }
}
