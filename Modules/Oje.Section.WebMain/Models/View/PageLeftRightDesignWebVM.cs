using Oje.Infrastructure.Enums;
using Oje.Section.WebMain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class PageLeftRightDesignWebVM: IPageWebItemVM
    {
        public PageLeftRightDesignWebVM()
        {
            PageLeftRightDesignWebItemVMs = new();
        }

        public string title { get; set; }
        public string description { get; set; }
        public int order { get; set; }
        public PageWebItemType type { get; set; }

        public List<PageLeftRightDesignWebItemVM> PageLeftRightDesignWebItemVMs { get; set; }
    }
}
