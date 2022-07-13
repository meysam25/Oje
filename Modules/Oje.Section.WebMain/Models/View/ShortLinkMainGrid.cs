using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class ShortLinkMainGrid: GlobalGrid
    {
        public string code { get; set; }
        public string link { get; set; }
        public bool? isActive { get; set; }
    }
}
