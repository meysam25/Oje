using Oje.ProposalFormManager.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface ICarSpecificationAmountManager
    {
        object Inquiry(int? siteSettingId, CarBodyInquiryVM input);
    }
}
