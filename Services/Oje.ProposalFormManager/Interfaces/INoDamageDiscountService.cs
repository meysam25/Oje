using Oje.ProposalFormService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface INoDamageDiscountService
    {
        NoDamageDiscount GetByFormId(int? ProposalFormId, int? Id);
        object GetLightList(int? ProposalFormId);
    }
}
