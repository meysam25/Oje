using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Blog.Interfaces;
using Oje.Section.Blog.Models.DB;
using Oje.Section.Blog.Models.View;
using Oje.Section.Blog.Services.EContext;
using System;
using System.Linq;

namespace Oje.Section.Blog.Services
{
    public class BlogReviewService : IBlogReviewService
    {
        readonly BlogDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public BlogReviewService
            (
                BlogDBContext db,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
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

        public object Delete(string id, int? siteSettingId)
        {
            byte? ip1 = null;
            byte? ip2 = null;
            byte? ip3 = null;
            byte? ip4 = null;
            DateTime? cd = null;
            long? blogId = null;
            try
            {
                var arrIdItems = id.Split('_');
                ip1 = arrIdItems[0].ToByteReturnZiro();
                ip2 = arrIdItems[1].ToByteReturnZiro();
                ip3 = arrIdItems[2].ToByteReturnZiro();
                ip4 = arrIdItems[3].ToByteReturnZiro();
                cd = new DateTime(arrIdItems[4].ToLongReturnZiro());
                blogId = arrIdItems[5].ToLongReturnZiro();
            }
            catch { }
            var foundItem = db.BlogReviews
                .Where(t => t.Ip1 == ip1 && t.Ip2 == ip2 && t.Ip3 == ip3 && t.Ip4 == ip4 && t.BlogId == blogId && t.CreateDate == cd)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(string id, int? siteSettingId)
        {
            byte? ip1 = null;
            byte? ip2 = null;
            byte? ip3 = null;
            byte? ip4 = null;
            DateTime? cd = null;
            long? blogId = null;
            try
            {
                var arrIdItems = id.Split('_');
                ip1 = arrIdItems[0].ToByteReturnZiro();
                ip2 = arrIdItems[1].ToByteReturnZiro();
                ip3 = arrIdItems[2].ToByteReturnZiro();
                ip4 = arrIdItems[3].ToByteReturnZiro();
                cd = new DateTime(arrIdItems[4].ToLongReturnZiro());
                blogId = arrIdItems[5].ToLongReturnZiro();
            }
            catch { }

            return db.BlogReviews
                .Where(t => t.Ip1 == ip1 && t.Ip2 == ip2 && t.Ip3 == ip3 && t.Ip4 == ip4 && t.BlogId == blogId && t.CreateDate == cd)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Select(t => new
                {
                    descrption = t.Description
                })
                .FirstOrDefault();
        }


        public object Confirm(string id, int? siteSettingId, long? userId)
        {
            if (userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            byte? ip1 = null;
            byte? ip2 = null;
            byte? ip3 = null;
            byte? ip4 = null;
            DateTime? cd = null;
            long? blogId = null;
            try
            {
                var arrIdItems = id.Split('_');
                ip1 = arrIdItems[0].ToByteReturnZiro();
                ip2 = arrIdItems[1].ToByteReturnZiro();
                ip3 = arrIdItems[2].ToByteReturnZiro();
                ip4 = arrIdItems[3].ToByteReturnZiro();
                cd = new DateTime(arrIdItems[4].ToLongReturnZiro());
                blogId = arrIdItems[5].ToLongReturnZiro();
            }
            catch { }
            var foundItem = db.BlogReviews
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Ip1 == ip1 && t.Ip2 == ip2 && t.Ip3 == ip3 && t.Ip4 == ip4 && t.BlogId == blogId && t.CreateDate == cd)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsConfirm = !foundItem.IsConfirm;
            foundItem.ConfirmUserId = userId;
            if (foundItem.IsConfirm == true)
                foundItem.ConfirmDate = DateTime.Now;
            else
                foundItem.ConfirmDate = null;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public GridResultVM<BlogReviewMainGridResultVM> GetList(BlogReviewMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new BlogReviewMainGrid();

            var qureResult = db.BlogReviews
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.blogTitle))
                qureResult = qureResult.Where(t => t.Blog.Title.Contains(searchInput.blogTitle));
            if (!string.IsNullOrEmpty(searchInput.userFullname))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.userFullname));
            if (!string.IsNullOrEmpty(searchInput.userMobile))
                qureResult = qureResult.Where(t => t.Mobile.Contains(searchInput.userMobile));
            if (!string.IsNullOrEmpty(searchInput.userEmail))
                qureResult = qureResult.Where(t => t.Email.Contains(searchInput.userEmail));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsConfirm == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<BlogReviewMainGridResultVM>()
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
                    t.BlogId,
                    t.Title,
                    t.Email,
                    t.Mobile,
                    t.IsConfirm,
                    t.ConfirmDate,
                    blogTitle = t.Blog.Title,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new BlogReviewMainGridResultVM
                {
                    row = ++row,
                    id = t.Ip1 + "_" + t.Ip2 + "_" + t.Ip3 + "_" + t.Ip4 + "_" + t.CreateDate.Ticks + "_" + t.BlogId,
                    blogTitle = t.blogTitle,
                    iA = t.IsConfirm == true ? true : false,
                    isActive = t.IsConfirm == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    userFullname = t.Title,
                    userMobile = t.Mobile,
                    userEmail = t.Email,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()

            };
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
