using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Oje.AccountService.Interfaces;
using Oje.FileService.Interfaces;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using System.IO;
using System;

namespace Oje.Section.Core.Areas.Core.Controllers
{
    public class NotFoundPagesController: Controller
    {
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IUploadedFileService UploadedFileService = null;
        public NotFoundPagesController
            (
                IBlockAutoIpService BlockAutoIpService,
                ISiteSettingService SiteSettingService,
                IUploadedFileService UploadedFileService
            )
        {
            this.BlockAutoIpService = BlockAutoIpService;
            this.SiteSettingService = SiteSettingService;
            this.UploadedFileService = UploadedFileService;
        }

        [HttpGet]
        [Route("favicon.ico", Order = int.MaxValue - 1000)]
        [Route("apple-touch-icon-precomposed.png", Order = int.MaxValue - 1000)]
        [Route("apple-touch-icon.png", Order = int.MaxValue - 1000)]
        [Route("apple-touch-icon-120x120.png", Order = int.MaxValue - 1000)]
        public IActionResult GetFile()
        {
            var curSetting = SiteSettingService.GetSiteSetting();

            if (curSetting == null)
                return Content("File Not Found");

            var foundFile = UploadedFileService.GetFile(curSetting.Image512.Replace("?fn=",""));
            if (foundFile == null || string.IsNullOrEmpty(foundFile.FileNameOnServer) || System.IO.File.Exists(foundFile.FileNameOnServer) == false)
                return Content("File Not Found");

            var fi = new FileInfo(foundFile.FileName);
            var rfi = new FileInfo(foundFile.FileNameOnServer);

            Response.Headers["Cache-Control"] = "max-age=" + new TimeSpan(365, 0, 0, 0).TotalSeconds.ToString("0");

            if (foundFile.FileSize != null && rfi.Length != foundFile.FileSize)
                return Content("File Not Found");

            string rFileContentType = " ";
            if (!string.IsNullOrEmpty(fi.Name))
                new FileExtensionContentTypeProvider().TryGetContentType(new FileInfo(fi.Name).Name, out rFileContentType);

            if (foundFile.FileContentType != null && rFileContentType != foundFile.FileContentType)
                return Content("File Not Found");

            return File(System.IO.File.ReadAllBytes(foundFile.FileNameOnServer), rFileContentType, fi.Name);
        }

        [Route("{*url}", Order = int.MaxValue - 50)]
        public IActionResult CatchAll(string url)
        {

            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.PageNotFound, Infrastructure.Enums.BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            throw BException.GenerateNewException(BMessages.Not_Found, Infrastructure.Enums.ApiResultErrorCode.NotFound);
        }
    }
}
