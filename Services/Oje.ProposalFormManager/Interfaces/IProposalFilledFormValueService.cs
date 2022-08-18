using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models.PageForms;
using System.Collections.Generic;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFilledFormValueService
    {
        void CreateByJsonConfig(PageForm ppfObj, long proposalFilledFormId, IFormCollection form, List<ctrl> allCtrls, bool? isEdit = false);
        void UpdateBy(long id, IFormCollection form, PageForm jsonObj);
    }
}
