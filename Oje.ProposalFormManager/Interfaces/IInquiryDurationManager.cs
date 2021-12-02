using Oje.ProposalFormManager.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IInquiryDurationManager
    {
        InquiryDuration GetById(int? siteSettingId, int? proposalFormId, int? id);
        object GetLightList(int? siteSettingId, int? proposalFormId);
    }
}
