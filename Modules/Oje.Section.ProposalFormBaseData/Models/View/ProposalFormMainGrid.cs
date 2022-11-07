using Oje.Infrastructure.Models;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class ProposalFormMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public bool? isActive { get; set; }
        public int? setting { get; set; }
    }
}
