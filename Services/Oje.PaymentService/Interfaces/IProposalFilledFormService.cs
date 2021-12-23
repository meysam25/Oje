using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Interfaces
{
    public interface IProposalFilledFormService
    {
        void UpdateTraceCode(long objectId, string traceNo);
    }
}
