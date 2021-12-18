using Oje.Infrastructure.Models;
using Oje.ProposalFormService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IGlobalInputInqueryService
    {
        long Create(object input, List<InqeryExteraParameter> inqeryExteraParameters, int? siteSettingId);
    }
}
