using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.Section.SalesNetworkBaseData.Models.View
{
    public class SalesNetworkCommissionLevelCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public int? snId { get; set; }
        public int? step { get; set; }
        public decimal? rate { get; set; }
        public PersonType? calceType { get; set; }
    }
}
