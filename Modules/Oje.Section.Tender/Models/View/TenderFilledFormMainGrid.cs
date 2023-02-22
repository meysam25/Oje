using Oje.Infrastructure.Models;

namespace Oje.Section.Tender.Models.View
{
    public class TenderFilledFormMainGrid: GlobalGrid
    {
        public string userfullname { get; set; }
        public string insurances { get; set; }
        public string provinceTitle { get; set; }
        public string cityTitle { get; set; }
        public string createDate { get; set; }
        public string endDate { get; set; }
        public bool? isPub { get; set; }
    }
}
