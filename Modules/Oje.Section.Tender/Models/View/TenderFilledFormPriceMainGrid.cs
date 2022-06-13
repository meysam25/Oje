using Oje.Infrastructure.Models;

namespace Oje.Section.Tender.Models.View
{
    public class TenderFilledFormPriceMainGrid: GlobalGridParentLong
    {
        public string insurance { get; set; }
        public int? company { get; set; }
        public string user { get; set; }
        public long? price { get; set; }
        public bool? isActive { get; set; }
        public string createDate { get; set; }
    }
}
