using System.Collections.Generic;

namespace Oje.Section.Tender.Models.View
{
    public class TenderFilledFormPdfVMGroupVM
    {
        public TenderFilledFormPdfVMGroupVM()
        {
            ProposalFilledFormPdfGroupItems = new();
            TenderFilledFormPdfVMGroupVMs = new();
        }

        public long id { get; set; }
        public long configId { get; set; }
        public string title { get;  set; }
        public List<TenderFilledFormPdfVMGroupItem> ProposalFilledFormPdfGroupItems { get;  set; }
        public List<TenderFilledFormPdfVMGroupVM> TenderFilledFormPdfVMGroupVMs { get; set; }
    }
}
