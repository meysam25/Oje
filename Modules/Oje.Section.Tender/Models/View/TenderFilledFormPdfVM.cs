
using System.Collections.Generic;

namespace Oje.Section.Tender.Models.View
{
    public class TenderFilledFormPdfVM
    {
        public string createDate { get; set; }
        public string createUserFullname { get; set; }
        public string ppfTitle { get; set; }
        public long id { get; set; }
        public List<TenderFilledFormPdfVMGroupVM> TenderFilledFormPdfVMGroupVMs { get; set; }
    }
}
