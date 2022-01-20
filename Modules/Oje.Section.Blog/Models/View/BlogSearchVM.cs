using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Models.View
{
    public class BlogSearchVM
    {
        public List<int> catIds { get; set; }
        public BlogSortTypes? sortID { get; set; }
        public BlogTypes? typeId { get; set; }
        public int? page { get; set; }
        public long? keyWordId { get; set; }
    }
}
