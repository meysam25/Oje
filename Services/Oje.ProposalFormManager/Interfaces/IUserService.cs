using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.PageForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IUserService
    {
        long CreateUserForProposalFormIfNeeded(IFormCollection form, int? siteSettingId, long? loginUserId);
        object GetSelect2List(Select2SearchVM searchInput, int? roleId, int? companyId, int? provinceId, int? cityId, int? siteSettingId);
        List<int> GetUserCompanies(long? userId);
    }
}
