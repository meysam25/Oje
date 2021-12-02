using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IBankManager
    {
        object GetLightList();
        bool IsValid(List<int> bankIds);
    }
}
