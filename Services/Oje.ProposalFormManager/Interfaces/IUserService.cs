using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IUserService
    {
        long CreateUserForProposalFormIfNeeded(IFormCollection form, int? siteSettingId, long? loginUserId);
        object GetSelect2List(Select2SearchVM searchInput, int? roleId, int? companyId, int? provinceId, int? cityId, int? siteSettingId);
        List<int> GetUserCompanies(long? userId);
        long GetUserWalletBalance(long? userId, int? siteSettingId);
    }
}
