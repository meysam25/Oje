using Oje.AccountService.Models.DB;
using System.Collections.Generic;

namespace Oje.AccountService.Interfaces
{
    public interface IBlogService
    {
        List<Blog> GetMainBlog(int count, int? siteSettingId);
    }
}
