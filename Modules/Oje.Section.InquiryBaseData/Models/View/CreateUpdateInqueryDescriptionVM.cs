using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class CreateUpdateInqueryDescriptionVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int? ppfId { get; set; }
        public string ppfId_Title { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString desc { get; set; }
        public bool? isActive { get; set; }
        public int? settId { get; set; }

        public List<int> cIds { get; set; }

    }
}
