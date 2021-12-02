using Oje.Infrastructure.Models;
using Oje.ProposalFormManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IGlobalInputInqueryManager
    {
        long Create(object input, List<InqeryExteraParameter> inqeryExteraParameters, int? siteSettingId);
    }
}
