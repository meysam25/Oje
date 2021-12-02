using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountManager.Interfaces
{
    public interface IProposalFormManager
    {
        object GetightListForSelect2(Select2SearchVM searchInput, int? siteSettingId);
        bool Exist(int? sitesettingId, int formId);
    }
}
