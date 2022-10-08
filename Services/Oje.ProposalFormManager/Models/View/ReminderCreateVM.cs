using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;

namespace Oje.ProposalFormService.Models.View
{
    public class ReminderCreateVM: GlobalSiteSetting
    {
        public long? id { get; set; }
        public int? fid { get; set; }
        public string targetDate { get; set; }
        public string mobile { get; set; }
        public string summery { get; set; }
        public IFormFile insuranceImage { get; set; }
        public IFormFile nationalCard { get; set; }
    }
}
