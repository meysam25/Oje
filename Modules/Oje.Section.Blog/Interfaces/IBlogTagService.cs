using Oje.Infrastructure.Models;
using Oje.Section.Blog.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Interfaces
{
    public interface IBlogTagService
    {
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId);
        long CreateIfNotExist(string tag, int? siteSettingId);
        BlogTag GetBy(long id, int? siteSettingId);
    }
}
