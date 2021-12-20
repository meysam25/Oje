using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.View
{
    public class ProposalFilledFormLogMainGrid: GlobalGrid
    {
        public long? pKey { get; set; }
        public ProposalFilledFormStatus? status { get; set; }
        public string userFullname { get; set; }
        public string createDate { get; set; }
    }
}
