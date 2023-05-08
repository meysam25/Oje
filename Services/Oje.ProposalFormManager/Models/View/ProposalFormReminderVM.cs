using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.ProposalFormService.Models.View
{
    public class ProposalFormReminderVM
    {
        public long id { get;  set; }
        public int appfCatId { get;  set; }
        public string appfCatId_Title { get;  set; }
        public int fid { get; set; }
        public string fid_Title { get; set; }
        public string targetDate { get; set; }
        public string mobile { get; set; }
        public string summery { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString insuranceImage_address { get; set; }
        public string nationalCard_address { get; set; }
        public int cSOWSiteSettingId { get; set; }
        public string cSOWSiteSettingId_Title { get; set; }
        public string startDate { get; set; }
        public string fullname { get; set; }
        public string insuranceImage_address_download { get; internal set; }
    }
}
