using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IVehicleSystemManager
    {
        string GetTitleById(int? id);
        object GetLightList();
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
