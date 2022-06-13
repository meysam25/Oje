using Oje.Infrastructure.Models;

namespace Oje.Section.Tender.Models.View
{
    public class TenderFilledFormMainGrid: GlobalGrid
    {
        public string userfullname { get; set; }
        public string insurances { get; set; }
        public string createDate { get; set; }
        public bool? isPub { get; set; }
    }
}
