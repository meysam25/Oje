using Oje.Infrastructure.Models;

namespace Oje.Section.GlobalForms.Models.View
{
    public class GeneralFormMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public string name { get; set; }
        public bool? isActive { get; set; }
        public int? setting { get; set; }
    }
}
