using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.ValidatedSignature.Models.View
{
    public class BankAccountFactorMainGrid: GlobalGrid
    {
        public string bcTitle { get; set; }
        public long? objId { get; set; }
        public BankAccountFactorType? type { get; set; }
        public string createDate { get; set; }
        public string user { get; set; }
        public long? price { get; set; }
        public bool? isPayed { get; set; }
        public string traceCode { get; set; }
        public string website { get; set; }
    }
}
