using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class LoginDescrptionCreateUpdateVM
    {
        public int? id { get; set; }
        public string url { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString desc { get; set; }
        public bool? isActive { get; set; }
    }
}
