using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Oje.FileService.Interfaces;

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
        readonly IUserService UserService = null;
        public BaseDataController(
                IUploadedFileService UploadedFileService,
                IProvinceService ProvinceService,
                ICityService CityService,
                IUserService UserService
            )
        {
            this.UploadedFileService = UploadedFileService;
            this.ProvinceService = ProvinceService;
            this.CityService = CityService;
            this.UserService = UserService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ManageModalResource.Copy();
            return Json(true);
        }

        [HttpGet]
        public IActionResult GetFile(string fn)
        {
            if (string.IsNullOrEmpty(fn))
                return Content("File Not Found");

            var loginUserId = HttpContext.GetLoginUser()?.UserId;

            var allChildUserId = UserService.GetChildsUserId(loginUserId.ToLongReturnZiro());
            var foundFile = UploadedFileService.GetFile(fn, loginUserId.ToLongReturnZiro(), allChildUserId);
            if (foundFile == null || string.IsNullOrEmpty(foundFile.FileNameOnServer) || System.IO.File.Exists(foundFile.FileNameOnServer) == false || string.IsNullOrEmpty(foundFile.FileContentType))
                return Content("File Not Found");

            Response.Headers["Cache-Control"] = "max-age=" + new TimeSpan(365, 0, 0, 0).TotalSeconds.ToString("0");

            return File(System.IO.File.ReadAllBytes(foundFile.FileNameOnServer), foundFile.FileContentType, new FileInfo(foundFile.FileName).Name);
        }

        [HttpPost]
        public IActionResult Get(string id)
        {
            return Json(EnumService.GetEnum(id));
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

        [HttpGet]
        public IActionResult GetCaptchaImage(Guid? id)
        {
            if (id == null)
                return NotFound();
            var imageBytes = Captcha.GetImageCaptch(id.Value);
            if (imageBytes != null && imageBytes.Length > 0)
                return File(imageBytes, "image/jpeg");

            return NotFound();
        }

        [HttpGet]
        public string GenerateToken()
        {
            return Guid.NewGuid().ToString();
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
                    var tempResult = UploadedFileService.UploadNewFile(FileType.CKEditor, pFile, null, null, null, ".png,.jpg,.jpeg", false);
                    result = new { @default = GlobalConfig.FileAccessHandlerUrl + tempResult };
                }
            }

            return Json(result);
        }
    }
}
