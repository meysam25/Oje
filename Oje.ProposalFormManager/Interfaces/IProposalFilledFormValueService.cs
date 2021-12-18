using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models.PageForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFilledFormValueService
    {
        void CreateByJsonConfig(PageForm ppfObj, long proposalFilledFormId, IFormCollection form, bool? isEdit = false);
        void UpdateBy(long id, IFormCollection form, PageForm jsonObj);
    }
}
