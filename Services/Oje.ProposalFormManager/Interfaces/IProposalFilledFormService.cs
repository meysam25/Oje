﻿using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFilledFormService
    {
        ApiResult Create(int? siteSettingId, IFormCollection form, long? loginUserId, string targetUrl, LoginUserVM loginUser);
        void JustValidation(int? siteSettingId, IFormCollection form, long? loginUserId, string targetUrl);
    }
}
