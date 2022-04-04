using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class RequiredDocumentVM
    {
        [IgnoreStringEncode]
        public MyHtmlString desc { get; set; }
        public object items { get; set; }
    }
}
