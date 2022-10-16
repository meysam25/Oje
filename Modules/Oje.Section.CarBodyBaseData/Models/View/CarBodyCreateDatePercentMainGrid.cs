using Oje.Infrastructure.Models;

namespace Oje.Section.CarBodyBaseData.Models.View
{
    public class CarBodyCreateDatePercentMainGrid: GlobalGrid
    {
        public int? carUsage { get; set; }
        public string title { get; set; }
        public int? fromYear { get; set; }
        public int? toYear { get; set; }
        public int? percent { get; set; }
        public bool? isActive { get; set; }
    }
}
