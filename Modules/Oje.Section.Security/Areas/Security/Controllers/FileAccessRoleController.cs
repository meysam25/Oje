﻿using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "دسترسی فایل")]
    [CustomeAuthorizeFilter]
    public class FileAccessRoleController: Controller
    {
        readonly IFileAccessRoleService FileAccessRoleService = null;
        readonly IRoleService RoleService = null;
        public FileAccessRoleController(
            IFileAccessRoleService FileAccessRoleService,
            IRoleService RoleService
            )
        {
            this.FileAccessRoleService = FileAccessRoleService;
            this.RoleService = RoleService;
        }

        [AreaConfig(Title = "دسترسی فایل", Icon = "fa-photo-video", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "دسترسی فایل";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FileAccessRole", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست دسترسی فایل", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "FileAccessRole")));
        }

        [AreaConfig(Title = "افزودن دسترسی فایل جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateFileAccessRoleVM input)
        {
            return Json(FileAccessRoleService.Create(input));
        }

        [AreaConfig(Title = "حذف دسترسی فایل", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(FileAccessRoleService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک دسترسی فایل", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(FileAccessRoleService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  دسترسی فایل", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateFileAccessRoleVM input)
        {
            return Json(FileAccessRoleService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست دسترسی فایل", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] FileAccessRoleMainGrid searchInput)
        {
            return Json(FileAccessRoleService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] FileAccessRoleMainGrid searchInput)
        {
            var result = FileAccessRoleService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست نقش ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetRoleList()
        {
            return Json(RoleService.GetLightList());
        }
    }
}
