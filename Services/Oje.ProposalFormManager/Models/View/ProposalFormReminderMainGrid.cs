using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.View
{
    public class ProposalFormReminderMainGrid: GlobalGrid
    {
        public string ppfTitle { get; set; }
        public long? mobile { get; set; }
        public string td { get; set; }
    }
}
