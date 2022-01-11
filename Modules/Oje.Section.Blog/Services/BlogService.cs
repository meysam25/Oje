using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
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
    public class BlogService : IBlogService
    {
        readonly BlogDBContext db = null;
        readonly IBlogTagService BlogTagService = null;
        readonly IUploadedFileService UploadedFileService = null;
        public BlogService
            (
                BlogDBContext db,
                IBlogTagService BlogTagService,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.BlogTagService = BlogTagService;
            this.UploadedFileService = UploadedFileService;
        }

        public ApiResult Create(BlogCreateUpdateVM input, int? siteSettingId, long loginUserId)
        {
            createUpdateValidation(input, siteSettingId, loginUserId);

            Models.DB.Blog newItem = new Models.DB.Blog()
            {
                BlogCategoryId = input.catId.ToIntReturnZiro(),
                CreateDate = DateTime.Now,
                CreateUserId = loginUserId,
                Description = input.description,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Title = input.title,
                Summery = input.summery,
                SiteSettingId = siteSettingId.ToIntReturnZiro(),
                PublisheDate = input.publishDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value,
                VideoUrl = input.aparatUrl
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.tags != null && input.tags.Count > 0)
            {
                foreach (var tag in input.tags)
                {
                    long tagId = BlogTagService.CreateIfNotExist(tag, siteSettingId);
                    if (tagId > 0)
                        db.Entry(new BlogTagBlog() { BlogId = newItem.Id, BlogTagId = tagId }).State = EntityState.Added;
                }
            }

            if (input.rBlogs != null && input.rBlogs.Count > 0)
                foreach (var blogId in input.rBlogs)
                    db.Entry(new BlogBlog() { BlogOwnId = newItem.Id, BlogRelatedId = blogId }).State = EntityState.Added;

            if (input.mainImage != null && input.mainImage.Length > 0)
            {
                newItem.ImageUrl = UploadedFileService.UploadNewFile(FileType.BlogBigImage, input.mainImage, loginUserId, siteSettingId, newItem.Id, ".png,.jpg,.jpeg", false);
                newItem.ImageUrl600 = UploadedFileService.UploadNewFile(FileType.BlogBigImage600, input.mainImage, loginUserId, siteSettingId, newItem.Id, ".png,.jpg,.jpeg", false);
                newItem.ImageUrl200 = UploadedFileService.UploadNewFile(FileType.BlogMedImage, input.mainImage, loginUserId, siteSettingId, newItem.Id, ".png,.jpg,.jpeg", false);
                newItem.ImageUrl50 = UploadedFileService.UploadNewFile(FileType.BlogSmallImage, input.mainImage, loginUserId, siteSettingId, newItem.Id, ".png,.jpg,.jpeg", false);
            }

            if (input.mainSound != null && input.mainSound.Length > 0)
                newItem.SoundUrl = UploadedFileService.UploadNewFile(FileType.BlogSmallImage, input.mainSound, loginUserId, siteSettingId, newItem.Id, ".mp3", false);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(BlogCreateUpdateVM input, int? siteSettingId, long loginUserId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.catId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Category);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 300)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_300_chars);
            if (string.IsNullOrEmpty(input.publishDate))
                throw BException.GenerateNewException(BMessages.Please_Select_Published_Date);
            if (input.publishDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (string.IsNullOrEmpty(input.summery))
                throw BException.GenerateNewException(BMessages.Please_Enter_Summery);
            if (input.summery.Length > 1000)
                throw BException.GenerateNewException(BMessages.Summery_Can_Not_Be_More_then_1000_Chars);
            if (string.IsNullOrEmpty(input.description))
                throw BException.GenerateNewException(BMessages.Please_Enter_Description);
            if (input.tags == null || input.tags.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Tag);
            if (loginUserId <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (input.id.ToLongReturnZiro() <= 0 && (input.mainImage == null || input.mainImage.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_Image);
        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var foundItem =
               db.Blogs
               .Include(t => t.BlogTagBlogs)
               .Include(t => t.BlogOwnBlogs)
               .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
               .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.BlogTagBlogs != null && foundItem.BlogTagBlogs.Count > 0)
                foreach (var item in foundItem.BlogTagBlogs)
                    db.Entry(item).State = EntityState.Deleted;

            if (foundItem.BlogOwnBlogs != null && foundItem.BlogOwnBlogs.Count > 0)
                foreach (var item in foundItem.BlogOwnBlogs)
                    db.Entry(item).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public BlogVM GetById(long? id, int? siteSettingId)
        {
            return
                db.Blogs
                .OrderByDescending(t => t.Id)
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
               .Select(t => new
               {
                   id = t.Id,
                   catId = t.BlogCategoryId,
                   title = t.Title,
                   publishDate = t.PublisheDate,
                   summery = t.Summery,
                   description = t.Description,
                   aparatUrl = t.VideoUrl,
                   mainImage_address = t.ImageUrl,
                   mainSound_address = t.SoundUrl,
                   isActive = t.IsActive,
                   tags = t.BlogTagBlogs.Select(tt => tt.BlogTag.Title).ToList(),
                   rBlogs = t.BlogOwnBlogs.Select(tt => new
                   {
                       id = tt.BlogRelatedId,
                       title = tt.BlogRelated.Title
                   }).ToList()
               })
               .Take(1)
               .ToList()
               .Select(t => new BlogVM
               {
                   id = t.id,
                   catId = t.catId,
                   title = t.title,
                   publishDate = t.publishDate.ToFaDate(),
                   summery = t.summery,
                   description = t.description,
                   aparatUrl = t.aparatUrl,
                   mainImage_address = !string.IsNullOrEmpty(t.mainImage_address) ? (GlobalConfig.FileAccessHandlerUrl + t.mainImage_address) : "",
                   mainSound_address = !string.IsNullOrEmpty(t.mainSound_address) ? (GlobalConfig.FileAccessHandlerUrl + t.mainSound_address) : "",
                   isActive = t.isActive,
                   tags = t.tags.Select(tt => new { id = tt, title = tt }).ToList(),
                   rBlogs = t.rBlogs
               })
               .FirstOrDefault();
        }

        public GridResultVM<BlogMainGridResultVM> GetList(BlogMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new BlogMainGrid();

            var qureResult = db.Blogs.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.createBy))
                qureResult = qureResult.Where(t => (t.CreateUser.Firstname + " " + t.CreateUser.Lastname).Contains(searchInput.createBy));
            if (!string.IsNullOrEmpty(searchInput.publishDate) && searchInput.publishDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.publishDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<BlogMainGridResultVM>()
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
                    createBy = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    publishDate = t.PublisheDate,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new BlogMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    createBy = t.createBy,
                    title = t.title,
                    publishDate = t.publishDate.ToFaDate(),
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(BlogCreateUpdateVM input, int? siteSettingId, long loginUserId)
        {
            createUpdateValidation(input, siteSettingId, loginUserId);

            var foundItem =
                db.Blogs
                .Include(t => t.BlogTagBlogs)
                .Include(t => t.BlogOwnBlogs)
                .Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.BlogTagBlogs != null && foundItem.BlogTagBlogs.Count > 0)
                foreach (var item in foundItem.BlogTagBlogs)
                    db.Entry(item).State = EntityState.Deleted;

            if (foundItem.BlogOwnBlogs != null && foundItem.BlogOwnBlogs.Count > 0)
                foreach (var item in foundItem.BlogOwnBlogs)
                    db.Entry(item).State = EntityState.Deleted;


            foundItem.BlogCategoryId = input.catId.ToIntReturnZiro();
            foundItem.Description = input.description;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Title = input.title;
            foundItem.Summery = input.summery;
            foundItem.PublisheDate = input.publishDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
            foundItem.VideoUrl = input.aparatUrl;

            if (input.tags != null && input.tags.Count > 0)
            {
                foreach (var tag in input.tags)
                {
                    long tagId = BlogTagService.CreateIfNotExist(tag, siteSettingId);
                    if (tagId > 0)
                        db.Entry(new BlogTagBlog() { BlogId = foundItem.Id, BlogTagId = tagId }).State = EntityState.Added;
                }
            }

            if (input.rBlogs != null && input.rBlogs.Count > 0)
                foreach (var blogId in input.rBlogs)
                    db.Entry(new BlogBlog() { BlogOwnId = foundItem.Id, BlogRelatedId = blogId }).State = EntityState.Added;

            if (input.mainImage != null && input.mainImage.Length > 0)
            {
                foundItem.ImageUrl = UploadedFileService.UploadNewFile(FileType.BlogBigImage, input.mainImage, loginUserId, siteSettingId, foundItem.Id, ".png,.jpg,.jpeg", false);
                foundItem.ImageUrl600 = UploadedFileService.UploadNewFile(FileType.BlogBigImage600, input.mainImage, loginUserId, siteSettingId, foundItem.Id, ".png,.jpg,.jpeg", false);
                foundItem.ImageUrl200 = UploadedFileService.UploadNewFile(FileType.BlogMedImage, input.mainImage, loginUserId, siteSettingId, foundItem.Id, ".png,.jpg,.jpeg", false);
                foundItem.ImageUrl50 = UploadedFileService.UploadNewFile(FileType.BlogSmallImage, input.mainImage, loginUserId, siteSettingId, foundItem.Id, ".png,.jpg,.jpeg", false);
            }

            if (input.mainSound != null && input.mainSound.Length > 0)
                foundItem.SoundUrl = UploadedFileService.UploadNewFile(FileType.BlogSmallImage, input.mainSound, loginUserId, siteSettingId, foundItem.Id, ".mp3", false);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.Blogs.OrderByDescending(t => t.Id).AsQueryable();
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Title, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public object GetTopBlogs(int count, int? siteSettingId)
        {
            return db.Blogs
                .Where(t => t.IsActive == true && t.PublisheDate <= DateTime.Now)
                .OrderByDescending(t => t.Id)
                .Take(4)
                .Select(t => new
                {
                    img1 = GlobalConfig.FileAccessHandlerUrl + t.ImageUrl600,
                    img2 = GlobalConfig.FileAccessHandlerUrl + t.ImageUrl200,
                    title = t.Title,
                    desc = t.Summery
                }).ToList();
        }
    }
}
