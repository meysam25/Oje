using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Interfaces;
using System.Collections.Generic;

namespace Oje.Section.WebMain.Models.View
{
    public class PageLeftRightDesignWebVM: IPageWebItemVM
    {
        public PageLeftRightDesignWebVM()
        {
            PageLeftRightDesignWebItemVMs = new();
        }

        public string title { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString description { get; set; }
        public int order { get; set; }
        public PageWebItemType type { get; set; }

        public List<PageLeftRightDesignWebItemVM> PageLeftRightDesignWebItemVMs { get; set; }
    }
}
