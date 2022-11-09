using Oje.Infrastructure.Models;

namespace Oje.Section.GlobalForms.Models.View
{
    public class GeneralFilledFormStatusMainGrid: GlobalGridParentLong
    {
        public string status { get; set; }
        public string userFullname { get; set; }
        public string createDate { get; set; }
        public string desc { get; set; }
    }
}
