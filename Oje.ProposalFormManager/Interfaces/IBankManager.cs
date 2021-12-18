using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IBankService
    {
        object GetLightList();
        bool IsValid(List<int> bankIds);
    }
}
