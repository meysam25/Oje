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
    }
}
