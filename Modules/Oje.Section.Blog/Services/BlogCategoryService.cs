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

namespace Oje.Section.Blog.Services
{
    public class BlogCategoryService : IBlogCategoryService
    {
        readonly BlogDBContext db = null;
        public BlogCategoryService(BlogDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(BlogCategoryCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new BlogCategory()
            {
                Title = input.title,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                SiteSettingId = siteSettingId.ToIntReturnZiro()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public List<BlogCategoryWebVM> GetListForWeb(int? siteSettingId)
        {
            return db.BlogCategories
                .Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true)
                .Select(t => new
                {
                    title = t.Title,
                    count = t.Blogs.Count(tt => tt.SiteSettingId == siteSettingId && tt.IsActive == true && tt.PublisheDate <= DateTime.Now)
                })
                .Where(t => t.count > 0)
                .Select(t => new BlogCategoryWebVM
                {
                    title = t.title,
                    count = t.count
                })
                .ToList();
        }

        private void createUpdateValidation(BlogCategoryCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);

        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.BlogCategories.Where(t => t.SiteSettingId == siteSettingId).Select(t => new
            {
                id = t.Id,
                title = t.Title
            }).ToList());

            return result;
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.BlogCategories.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;

            try
            {
                db.SaveChanges();
            }
            catch
            {
                throw BException.GenerateNewException(BMessages.Unable_To_Delete);
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.BlogCategories
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive
                }).FirstOrDefault();
        }

        public GridResultVM<BlogCategoryMainGridResultVM> GetList(BlogCategoryMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new();

            var qureResult = db.BlogCategories.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<BlogCategoryMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new BlogCategoryMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(BlogCategoryCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.BlogCategories.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Title = input.title;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
