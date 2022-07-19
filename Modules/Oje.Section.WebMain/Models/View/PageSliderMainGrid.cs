using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Models.View
{
    public class PageSliderMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public string page { get; set; }
        public bool? isActive { get; set; }
    }
}
