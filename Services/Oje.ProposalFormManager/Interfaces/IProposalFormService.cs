using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.ProposalFormService.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFormService
    {
        ProposalForm GetByType(ProposalFormType type, int? siteSettingId);
        string GetJSonConfigFile(int proposalFormId, int? siteSettingId);
        bool Exist(int proposalFormId, int? siteSettingId);
        ProposalForm GetById(int id, int? siteSettingId);
        object GetSelect2List(Select2SearchVM searchInput, int? proposalFormCategoryId);
    }
}
