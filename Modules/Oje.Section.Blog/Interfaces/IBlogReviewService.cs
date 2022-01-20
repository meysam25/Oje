using Oje.Infrastructure.Models;
using Oje.Section.Blog.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Interfaces
{
    public interface IBlogReviewService
    {
        ApiResult Create(BlogWebAction input, int? siteSettingId, IpSections ipSections, long blogId);
        object GetConfirmList(int? siteSettingId, IpSections ipSections, long blogId);
    }
}
