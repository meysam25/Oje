using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class RequiredDocumentVM
    {
        [IgnoreStringEncode]
        public MyHtmlString desc { get; set; }
        public List<RequiredDocumentItemVM> items { get; set; }
        
    }
}
