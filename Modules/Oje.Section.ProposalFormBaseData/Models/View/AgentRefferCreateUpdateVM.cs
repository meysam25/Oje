﻿using Oje.Infrastructure.Models;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class AgentRefferCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public int? cid { get; set; }
        public string code { get; set; }
        public string fullname { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string tell { get; set; }
    }
}
