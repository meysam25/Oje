using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models
{
    public class PPFUserTypes
    {
        public long userId { get; set; }
        public ProposalFilledFormUserType ProposalFilledFormUserType { get; set; }
        public string fullUserName { get; set; }
        public string mobile { get; set; }
        public string emaile { get; set; }
    }
}
