using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models.PageForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IProposalFilledFormValueManager
    {
        void CreateByJsonConfig(PageForm ppfObj, long proposalFilledFormId, IFormCollection form);
    }
}
