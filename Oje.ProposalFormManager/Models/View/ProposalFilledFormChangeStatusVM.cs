using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.View
{
    public class ProposalFilledFormChangeStatusVM
    {
        public long? id { get; set; }
        public ProposalFilledFormStatus? status { get; set; }
        public string description { get; set; }
    }
}
