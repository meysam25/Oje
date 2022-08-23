using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Section.Blog.Models.View;
using System.Collections.Generic;

namespace Oje.Section.Blog.Interfaces
{
    public interface IBlogService
    {
        ApiResult Create(BlogCreateUpdateVM input, int? siteSettingId, long loginUserId);
        ApiResult Delete(long? id, int? siteSettingId);
        BlogVM GetById(long? id, int? siteSettingId, IpSections ipSections);
        ApiResult Update(BlogCreateUpdateVM input, int? siteSettingId, long loginUserId);
        GridResultVM<BlogMainGridResultVM> GetList(BlogMainGrid searchInput, int? siteSettingId);
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId);
        object GetTopBlogs(int count, int? siteSettingId);
        object Search(BlogSearchVM input, int? siteSettingId, int itemPerPage = 10);
        void SetViewOrLike(long id, IpSections ipSections, BlogLastLikeAndViewType type);
        List<BlogVM> GetMostTypeBlogs(int? siteSettingId, int count, BlogLastLikeAndViewType type, long id);
        object BlogActions(long id, BlogWebAction input, int? siteSettingId, IpSections ipSections);
        BlogVM GetByIdForWeb(long? id, int? siteSettingId, IpSections ipSections);
        string GetBlogSiteMap(int? siteSettingId, string baseUrl);
    }
}
