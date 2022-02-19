using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.DB;
using Oje.Section.WebMain.Models.View;
using Oje.Section.WebMain.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Services
{
    public class ContactUsService : IContactUsService
    {
        readonly WebMainDBContext db = null;
        public ContactUsService(WebMainDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(int? siteSettingId, IpSections ipSections, ContactUsWebVM input)
        {
            CreateValidation(siteSettingId, ipSections, input);

            db.ContactUses.Add(new ContactUs()
            {
                CreateDate = DateTime.Now,
                SiteSettingId = siteSettingId.Value,
                Description = input.description,
                Email = input.email,
                Fullname = input.fullname,
                Ip1 = ipSections.Ip1,
                Ip2 = ipSections.Ip2,
                Ip3 = ipSections.Ip3,
                Ip4 = ipSections.Ip4,
                Tell = input.tellNumber
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetBy(string id, int? siteSettingId)
        {
            if (!string.IsNullOrEmpty(id) && id.IndexOf(",") > 0)
            {
                var allParts = id.Split(',');
                if (allParts.Length == 5)
                {
                    byte ip1 = allParts[0].ToByteReturnZiro(), ip2 = allParts[1].ToByteReturnZiro(), ip3 = allParts[2].ToByteReturnZiro(), ip4 = allParts[3].ToByteReturnZiro();
                    DateTime? cd = allParts[4].ToDateTimeFromTick();
                    if (cd != null)
                    {
                        return new { description = db.ContactUses.Where(t => t.Ip1 == ip1 && t.Ip2 == ip2 && t.Ip3 == ip3 && t.Ip4 == ip4 && t.CreateDate == cd && t.SiteSettingId == siteSettingId).Select(t => t.Description).FirstOrDefault() };
                    }
                }
            }

            return null;
        }

        public GridResultVM<UserContactUsMainGridResultVM> GetList(UserContactUsMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new UserContactUsMainGrid();

            var qureResult = db.ContactUses.Where(t => t.SiteSettingId == siteSettingId);

            int row = searchInput.skip;

            return new GridResultVM<UserContactUsMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                .OrderByDescending(t => t.CreateDate)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    t.Ip1,
                    t.Ip2,
                    t.Ip3,
                    t.Ip4,
                    t.CreateDate,
                    fullname = t.Fullname,
                    tell = t.Tell,
                    email = t.Email
                })
                .ToList()
                .Select(t => new UserContactUsMainGridResultVM
                {
                    id = t.Ip1 + "," + t.Ip2 + "," + t.Ip3 + "," + t.Ip4 + "," + t.CreateDate.Ticks,
                    createDate = t.CreateDate.ToFaDate() + " " + t.CreateDate.ToString("hh:MM:ss"),
                    email = t.email,
                    fullname = t.fullname,
                    row = ++row,
                    tell = t.tell
                })
                .ToList()
            };
        }

        private void CreateValidation(int? siteSettingId, IpSections ipSections, ContactUsWebVM input)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (ipSections == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.fullname))
                throw BException.GenerateNewException(BMessages.Please_Enter_Firstname);
            if (input.fullname.Length > 100)
                throw BException.GenerateNewException(BMessages.FirstName_CanNot_Be_MoreThen_100_Char);
            if (string.IsNullOrEmpty(input.tellNumber))
                throw BException.GenerateNewException(BMessages.Please_Enter_Tell);
            if (input.tellNumber.Length > 50)
                throw BException.GenerateNewException(BMessages.Tell_CanNot_Be_MoreThen_50_Chars);
            if (string.IsNullOrEmpty(input.email))
                throw BException.GenerateNewException(BMessages.Please_Enter_Email);
            if (input.email.Length > 50)
                throw BException.GenerateNewException(BMessages.Email_CanNot_Be_More_Then_100_Chars);
            if (!input.email.IsValidEmail())
                throw BException.GenerateNewException(BMessages.Invalid_Email);
            if (string.IsNullOrEmpty(input.description))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (input.description.Length > 100)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_1000);

        }
    }
}
