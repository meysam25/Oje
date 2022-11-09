using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Models.DB;
using Oje.Section.GlobalForms.Models.View;
using Oje.Section.GlobalForms.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.GlobalForms.Services
{
    public class GeneralFilledFormStatusService : IGeneralFilledFormStatusService
    {
        readonly GeneralFormDBContext db = null;
        readonly IGeneralFormStatusService GeneralFormStatusService = null;

        public GeneralFilledFormStatusService
            (
                GeneralFormDBContext db,
                IGeneralFormStatusService GeneralFormStatusService
            )
        {
            this.db = db;
            this.GeneralFormStatusService = GeneralFormStatusService;
        }

        public void Create(long filledFormId, long statusId, string desc, long loginUserId)
        {
            if (filledFormId > 0 && statusId > 0 && loginUserId > 0)
            {
                db.Entry(new GeneralFilledFormStatus()
                {
                    CreateDate = DateTime.Now,
                    Description = desc,
                    GeneralFilledFormId = filledFormId,
                    GeneralFormStatusId = statusId,
                    UserId = loginUserId
                }).State = EntityState.Added;
                db.SaveChanges();
            }
        }

        public object GetList(GeneralFilledFormStatusMainGrid searchInput, int? siteSettingId, LoginUserVM loginUserVM)
        {
            searchInput = searchInput ?? new();
            var nullResult = new { total = 0, data = new List<object>() };

            var foundGFF = db.GeneralFilledForms.Where(t => t.Id == searchInput.pKey && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundGFF == null)
                return nullResult;

            var allValidForms = GeneralFormStatusService.GetLightList(loginUserVM?.roles);
            if (!allValidForms.Any(t => t.id == foundGFF.GeneralFormId + "_" + foundGFF.GeneralFormStatusId))
                return nullResult;


            var quiryResult = db.GeneralFilledFormStatuses.Where(t => t.GeneralFilledForm.IsDelete != true && t.GeneralFilledForm.SiteSettingId == siteSettingId && t.GeneralFilledFormId == searchInput.pKey);

            if (!string.IsNullOrEmpty(searchInput.status))
                quiryResult = quiryResult.Where(t => t.GeneralFormStatus.Title.Contains(searchInput.status));
            if (!string.IsNullOrEmpty(searchInput.userFullname))
                quiryResult = quiryResult.Where(t => (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.userFullname));
            if (!string.IsNullOrEmpty(searchInput.desc))
                quiryResult = quiryResult.Where(t => !string.IsNullOrEmpty(t.Description) && t.Description.Contains(searchInput.desc));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }

            int row = searchInput.skip;

            return new
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.CreateDate)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new 
                { 
                    id = t.Id,
                    status = t.GeneralFormStatus.Title,
                    userFullname = t.User.Firstname + " " + t.User.Lastname,
                    createDate = t.CreateDate,
                    desc = t.Description
                })
                .ToList()
                .Select(t => new 
                { 
                    row = ++row,
                    t.id,
                    t.status,
                    t.userFullname,
                    createDate = t.createDate.ToFaDate() + " " + t.createDate.ToString("HH:mm"),
                    t.desc
                })
                .ToList()
            };
        }
    }
}
