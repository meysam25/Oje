using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Core.Areas.Core.Controllers
{
    public class NotFoundPagesController: Controller
    {
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public NotFoundPagesController
            (
                IBlockAutoIpService BlockAutoIpService,
                ISiteSettingService SiteSettingService
            )
        {
            this.BlockAutoIpService = BlockAutoIpService;
            this.SiteSettingService = SiteSettingService;
        }

        [Route("{*url}", Order = int.MaxValue - 50)]
        public IActionResult CatchAll(string url)
        {

            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.PageNotFound, Infrastructure.Enums.BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            throw BException.GenerateNewException(BMessages.Not_Found, Infrastructure.Enums.ApiResultErrorCode.NotFound);
        }
    }
}
