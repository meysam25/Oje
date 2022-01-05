using Oje.ProposalFormService.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface ICarSpecificationAmountService
    {
        object Inquiry(int? siteSettingId, CarBodyInquiryVM input, string targetArea);
    }
}
