﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models.PageForms
{
    public class panel
    {
        public panel()
        {
        }

        public bool? hasInquiry { get; set; }
        public bool? isAgentRequired { get; set; }
        public bool? isCompanyListRequired { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string loadUrl { get; set; }
        public List<stepWizard> stepWizards { get; set; }
        public List<ctrl> ctrls { get; set; }
    }
}
