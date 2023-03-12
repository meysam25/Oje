using Oje.Infrastructure.Models;

namespace Oje.ValidatedSignature.Models.View
{
    public class WalletTransactionMainGrid: GlobalGrid
    {
        public long? id { get; set; }
        public string user { get; set; }
        public string createDate { get; set; }
        public long? price { get; set; }
        public string traceNo { get; set; }
        public string website { get; set; }
    }
}
