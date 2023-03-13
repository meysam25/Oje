using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using Oje.FileService.Interfaces;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Exceptions;
using Microsoft.AspNetCore.StaticFiles;
using System.Linq;

namespace Oje.Section.Core.Areas.Controllers
{
    [Area("Core")]
    [Route("[Area]/[Controller]/[Action]")]
    [Route("[Area]/[Controller]/[Action]/{id}")]
    public class BaseDataController : Controller
    {
        readonly IUploadedFileService UploadedFileService = null;
        readonly IProvinceService ProvinceService = null;
        readonly ICityService CityService = null;
        readonly ICompanyService CompanyService = null;
        readonly ISiteSettingService SiteSettingService = null;
        //readonly IUserService UserService = null;
        public BaseDataController(
                IUploadedFileService UploadedFileService,
                IProvinceService ProvinceService,
                ICityService CityService,
                ICompanyService CompanyService,
                ISiteSettingService SiteSettingService//,
                                                      //IUserService UserService
            )
        {
            this.UploadedFileService = UploadedFileService;
            this.ProvinceService = ProvinceService;
            this.CityService = CityService;
            this.CompanyService = CompanyService;
            this.SiteSettingService = SiteSettingService;
            //this.UserService = UserService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //var m = "test".Encrypt();
            ManageModalResource.Copy();
            return Json(true);
        }

        [HttpGet]
        public IActionResult GetSettingList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(SiteSettingService.GetightList(HttpContext.GetLoginUser()?.canSeeOtherWebsites, searchInput));
        }

        //[HttpGet]
        //public IActionResult HashPassword()
        //{
        //    UserService.UpdateHashPassword();
        //    return Json(true);
        //}

        [HttpGet]
        public IActionResult GetFile(string fn)
        {
            if (string.IsNullOrEmpty(fn))
                return Content("File Not Found");

            var loginUserId = HttpContext.GetLoginUser()?.UserId;

            var foundFile = UploadedFileService.GetFile(fn, loginUserId.ToLongReturnZiro());
            if (foundFile == null || string.IsNullOrEmpty(foundFile.FileNameOnServer) || System.IO.File.Exists(foundFile.FileNameOnServer) == false || !foundFile.IsSignature())
                return Content("File Not Found");

            var fi = new FileInfo(foundFile.FileName);
            var rfi = new FileInfo(foundFile.FileNameOnServer);

            Response.Headers["Cache-Control"] = "max-age=" + new TimeSpan(365, 0, 0, 0).TotalSeconds.ToString("0");

            if (foundFile.FileSize != null && rfi.Length != foundFile.FileSize)
                return Content("File Not Found");

            string rFileContentType = " ";
            if (!string.IsNullOrEmpty(fi.Name))
                new FileExtensionContentTypeProvider().TryGetContentType(new FileInfo(fi.Name).Name, out rFileContentType);

            if (foundFile.FileContentType != " " && foundFile.FileContentType != null && rFileContentType != foundFile.FileContentType)
                return Content("File Not Found");

            return File(System.IO.File.ReadAllBytes(foundFile.FileNameOnServer), rFileContentType, fi.Name);
        }

        [HttpPost]
        public IActionResult Get(string id)
        {
            return Json(EnumService.GetEnum(id, false));
        }

        [HttpPost]
        public IActionResult GetNoEmpty(string id)
        {
            return Json(EnumService.GetEnum(id, true));
        }

        [HttpPost]
        public IActionResult GenerateCaptch()
        {
            return Json(Captcha.Generate().Uid);
        }

        [HttpPost]
        public IActionResult GetProvinceList()
        {
            return Json(ProvinceService.GetLightList());
        }

        [HttpPost]
        public IActionResult GetCityList([FromQuery] int? id)
        {
            return Json(CityService.GetLightList(id));
        }

        [HttpPost]
        public IActionResult GetCompanyList()
        {
            return Json(CompanyService.GetightList());
        }

        [HttpGet]
        public IActionResult GetCityList2([FromQuery] int? provinceId, [FromQuery] Select2SearchVM searchInput)
        {
            if (provinceId.ToIntReturnZiro() <= 0)
            {
                if (Request.Query.Keys.Any(t => !string.IsNullOrEmpty(t) && t.ToLower().EndsWith("provinceid")))
                {
                    var provinceKey = Request.Query.Keys.Where(t => !string.IsNullOrEmpty(t) && t.ToLower().EndsWith("provinceid")).FirstOrDefault();
                    if (!string.IsNullOrEmpty(provinceKey))
                        provinceId = Request.Query[provinceKey][0].ToIntReturnZiro();
                }
            }
            return Json(CityService.GetSelect2List(provinceId, searchInput));
        }

        [HttpGet]
        public IActionResult GetCaptchaImage(Guid? id)
        {
            if (id == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var imageBytes = Captcha.GetImageCaptch(id.Value);
            if (imageBytes != null && imageBytes.Length > 0)
                return File(imageBytes, "image/jpeg");

            throw BException.GenerateNewException(BMessages.Not_Found);
        }

        [HttpGet]
        public string GenerateToken()
        {
            return RandomService.GeneratePassword(5) + "." + RandomService.GeneratePassword(5) + "." + RandomService.GeneratePassword(5);
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            object result = new object();
            if (Request.Form.Files.Count > 0)
            {
                var pFile = Request.Form.Files[0];
                if (pFile != null && pFile.Length > 0)
                {
                    var tempResult = UploadedFileService.UploadNewFile(FileType.CKEditor, pFile, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id, null, ".png,.jpg,.jpeg", false);
                    result = new { fileName = pFile.FileName, uploaded = 1, url = GlobalConfig.FileAccessHandlerUrl + tempResult };
                }
            }

            return Json(result);
        }
    }
}
