using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Interfaces
{
    public interface IProposalFilledFormManager
    {
        ApiResult Create(int? siteSettingId, IFormCollection form, long? loginUserId);
    }
}
