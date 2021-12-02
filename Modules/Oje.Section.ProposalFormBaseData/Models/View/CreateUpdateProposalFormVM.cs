using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Enums;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class CreateUpdateProposalFormVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public int? ppfCatId { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString jsonStr { get; set; }
        public string description { get; set; }
        public bool? isActive { get; set; }
        public int? siteSettingId { get; set; }
        public string ppfCatId_Title { get; set; }
        public string rules_address { get; set; }
        public IFormFile rules { get; set; }
        public ProposalFormType? type { get; set; }

    }
}
