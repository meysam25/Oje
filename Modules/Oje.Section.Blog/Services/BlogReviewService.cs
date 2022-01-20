using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Blog.Interfaces;
using Oje.Section.Blog.Models.DB;
using Oje.Section.Blog.Models.View;
using Oje.Section.Blog.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Services
{
    public class BlogReviewService : IBlogReviewService
    {
        readonly BlogDBContext db = null;
        public BlogReviewService(BlogDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(BlogWebAction input, int? siteSettingId, IpSections ipSections, long blogId)
        {
            CreateValidation(input, siteSettingId, ipSections, blogId);

            db.Entry(new BlogReview
            {
                BlogId = blogId,
                SiteSettingId = siteSettingId.ToIntReturnZiro(),
                CreateDate = DateTime.Now,
                Description = input.description,
                Email = input.email,
                Ip1 = ipSections.Ip1,
                Ip2 = ipSections.Ip2,
                Ip3 = ipSections.Ip3,
                Ip4 = ipSections.Ip4,
                Mobile = input.mobile,
                Title = input.fullname,
                Website = " "
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetConfirmList(int? siteSettingId, IpSections ipSections, long blogId)
        {
            return db.BlogReviews
                .OrderByDescending(t => t.CreateDate)
                .Where(t => t.SiteSettingId == siteSettingId && t.BlogId == blogId)
                .Where(t => t.IsConfirm == true || (t.Ip1 == ipSections.Ip1 && t.Ip2 == ipSections.Ip2 && t.Ip3 == ipSections.Ip3 && t.Ip4 == ipSections.Ip4))
                .Select(t => new
                {
                    fn = t.Title,
                    cd = t.CreateDate.ToFaDate() + " " + t.CreateDate.ToString("hh:MM"),
                    des = t.Description,
                    ic = t.IsConfirm
                })
                .ToList();
               

        }

        private void CreateValidation(BlogWebAction input, int? siteSettingId, IpSections ipSections, long blogId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.mobile))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile);
            if (!input.mobile.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (string.IsNullOrEmpty(input.email))
                throw BException.GenerateNewException(BMessages.Please_Enter_Email);
            if (!input.email.IsValidEmail())
                throw BException.GenerateNewException(BMessages.Invalid_Email);
            if (string.IsNullOrEmpty(input.description))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (input.email.Length > 100)
                throw BException.GenerateNewException(BMessages.Email_CanNot_Be_More_Then_100_Chars);
            if (ipSections == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (blogId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (string.IsNullOrEmpty(input.fullname))
                throw BException.GenerateNewException(BMessages.Please_Enter_Name);
            if (input.fullname.Length > 50)
                throw BException.GenerateNewException(BMessages.FirstName_CanNot_Be_MoreThen_50_Char);
        }
    }
}
