using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class PageLeftRightDesignCreateUpdateVM
    {
        public long? id { get; set; }
        public long? pId { get; set; }
        public string pId_Title { get; set; }
        public string title { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString description { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }
    }
}
