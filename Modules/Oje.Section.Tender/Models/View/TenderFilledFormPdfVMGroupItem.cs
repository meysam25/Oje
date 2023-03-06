using Oje.Infrastructure.Enums;

namespace Oje.Section.Tender.Models.View
{
    public class TenderFilledFormPdfVMGroupItem
    {
        public string cssClass { get; set; }
        public string title { get; set; }
        public string value { get; set; }
        public bool? isBold { get; set; }
        public ctrlType? ctrlType { get; set; }
    }
}
