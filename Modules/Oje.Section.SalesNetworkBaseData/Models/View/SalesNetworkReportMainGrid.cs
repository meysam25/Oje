using Oje.Infrastructure.Models;

namespace Oje.Section.SalesNetworkBaseData.Models.View
{
    public class SalesNetworkReportMainGrid: GlobalGrid
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public long? userId { get; set; }
        public int? snId { get; set; }
    }
}
