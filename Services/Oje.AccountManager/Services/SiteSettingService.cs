using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oje.AccountService.Services.EContext;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;

namespace Oje.AccountService.Services
{
    public class SiteSettingService : ISiteSettingService
    {
        readonly AccountDBContext db = null;
        readonly IHttpContextAccessor httpContextAccessor = null;
        static List<SiteSetting> SS { get; set; }
        static object lockObject = new object();
        public SiteSettingService(
                AccountDBContext db,
                IHttpContextAccessor httpContextAccessor
            )
        {
            this.db = db;
            this.httpContextAccessor = httpContextAccessor;
        }

        public object GetightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.SiteSettings.Select(t => new { id = t.Id, title = t.Title + "(" + t.WebsiteUrl + ")" }).ToList());

            return result;
        }

        public SiteSetting GetSiteSetting()
        {
            if (lockObject == null)
                lockObject = new object();
            if (SS == null || SS.Count == 0)
            {
                lock (lockObject)
                {
                    if (SS == null || SS.Count == 0)
                        SS = db.SiteSettings.Include(t => t.User).AsNoTracking().ToList();
                }
            }
            string host = httpContextAccessor.HttpContext?.Request?.Host.Host;
            if (!string.IsNullOrEmpty(host))
                return SS.Where(t => t.WebsiteUrl == host || t.PanelUrl == host).FirstOrDefault();
            return null;
        }

        public void UpdateSiteSettings()
        {
            SS = db.SiteSettings.Include(t => t.User).AsNoTracking().ToList();
        }

        public object GetManifest()
        {
            var foundSetting = GetSiteSetting();
            if (foundSetting == null)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);

            return new
            {
                name = foundSetting.Title,
                short_name = foundSetting.Title,
                start_url = "/",
                display = "fullscreen",
                background_color = "#fdfdfd",
                theme_color = "#db4938",
                orientation = "portrait-primary",
                scope = "/",
                description = foundSetting.SeoMainPage,
                icons = new List<object>()
                {
                    new
                    {
                        src = GlobalConfig.FileAccessHandlerUrl + foundSetting.Image96,
                        type = "image/png",
                        sizes = "96x96"
                    },
                    new
                    {
                        src = GlobalConfig.FileAccessHandlerUrl + foundSetting.Image192,
                        type = "image/png",
                        sizes = "192x192"
                    },
                    new
                    {
                        src = GlobalConfig.FileAccessHandlerUrl + foundSetting.Image512,
                        type = "image/png",
                        sizes = "512x512",
                        purpose = "any maskable"
                    }
                }
            };
        }

        public string GetMainService()
        {
            string result = "const mainServiceName = 'mainServiceName';" + Environment.NewLine;

            result += "const assets = [";
            result += "'/',";
            result += "'/Modules/Core/css/master.min.css.gz',";
            result += "'/Modules/Core/css/websiteCore.min.css.gz',";
            result += "'/Modules/Core/css/mainPage.min.css.gz',";
            result += "'/Modules/Core/js/jquery.min.js.gz',";
            result += "'/Modules/Core/js/websiteCor.min.js.gz',";
            result += "'/Modules/Core/js/mainPage.min.js.gz',";
            result += "'/Modules/Core/js/registerServices.min.js.gz',";
            result += "'/Modules/Core/js/chart.min.js.gz',";
            result += "'/serviceWorker.js',";
            result += "'/Modules/Webfonts/Vazir/Vazir-Regular-FD.woff2',";
            result += "'/Modules/webfonts/fa-light-300.woff2',";
            result += "'/Modules/Assets/MainPage/electronDevelopment.png',";
            result += "'/Modules/Assets/MainPage/iranlogo.png',";
            result += "'/Modules/Assets/MainPage/bgp.png',";
            result += "'/Modules/Assets/MainPage/salecIcon.png',";
            result += "'/Modules/Assets/MainPage/badane.png',";
            result += "'/Modules/Assets/MainPage/atashsozi.png',";
            result += "'/Modules/Images/iran.png',";
            result += "'/Modules/Assets/MainPage/etehadiyeLogo.png',";
            result += "'/Modules/Assets/MainPage/telegram.svg',";
            result += "'/Modules/Assets/MainPage/twitter.svg',";
            result += "'/Modules/Assets/MainPage/instagram.svg',";
            result += "'/Modules/Assets/MainPage/linkedin.svg'";
            result += "];";

            result += @"
                self.addEventListener('install', installEvent => {
                  installEvent.waitUntil(
                    caches.open(mainServiceName).then(cache => {
                      cache.addAll(assets);
                    })
                  );
                });
            ";

            result += @"
                self.addEventListener('fetch', fetchEvent => {
                  fetchEvent.respondWith(
                    caches.match(fetchEvent.request).then(res => {
                      return res || fetch(fetchEvent.request);
                    })
                  );
                });
            ";


            return result;
        }
    }
}
