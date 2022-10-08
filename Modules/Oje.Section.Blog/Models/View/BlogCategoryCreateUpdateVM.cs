using Oje.Infrastructure.Models;

namespace Oje.Section.Blog.Models.View
{
    public class BlogCategoryCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
    }
}
