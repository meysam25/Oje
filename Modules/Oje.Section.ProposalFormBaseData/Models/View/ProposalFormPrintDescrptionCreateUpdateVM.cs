﻿using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class ProposalFormPrintDescrptionCreateUpdateVM: GlobalSiteSetting
    {
        public long? id { get; set; }
        public int? pfid { get; set; }
        public string pfid_Title { get; set; }
        public ProposalFormPrintDescrptionType? type { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString description { get; set; }
        public bool? isActive { get; set; }
    }
}
