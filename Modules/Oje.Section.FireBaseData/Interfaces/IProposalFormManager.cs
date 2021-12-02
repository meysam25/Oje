using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Interfaces
{
    public interface IProposalFormManager
    {
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
