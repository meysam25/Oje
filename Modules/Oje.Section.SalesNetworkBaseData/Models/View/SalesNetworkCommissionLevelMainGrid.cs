using Oje.Infrastructure.Models;

namespace Oje.Section.SalesNetworkBaseData.Models.View
{
    public class SalesNetworkCommissionLevelMainGrid: GlobalGrid
    {
        public string snId { get; set; }
        public int? step { get; set; }
        public decimal? rate { get; set; }
    }
}
