using Oje.Infrastructure.Models;
using Oje.Section.Blog.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Interfaces
{
    public interface IBlogCategoryService
    {
        ApiResult Create(BlogCategoryCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(BlogCategoryCreateUpdateVM input, int? siteSettingId);
        GridResultVM<BlogCategoryMainGridResultVM> GetList(BlogCategoryMainGrid searchInput, int? siteSettingId);
        object GetLightList(int? siteSettingId);
        List<BlogCategoryWebVM> GetListForWeb(int? siteSettingId);
        object GetLightListForWeb(int? siteSettingId);
        BlogCategoryCreateUpdateVM GetBy(int id, int? siteSettingId);
    }
}
