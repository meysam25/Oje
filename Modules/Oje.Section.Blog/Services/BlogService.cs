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
using Oje.Security.Interfaces;
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
        readonly IBlogReviewService BlogReviewService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        public BlogService
            (
                BlogDBContext db,
                IBlogTagService BlogTagService,
                IUploadedFileService UploadedFileService,
                IBlogReviewService BlogReviewService,
                IBlockAutoIpService BlockAutoIpService
            )
        {
            this.db = db;
            this.BlogTagService = BlogTagService;
            this.UploadedFileService = UploadedFileService;
            this.BlogReviewService = BlogReviewService;
            this.BlockAutoIpService = BlockAutoIpService;
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

        public BlogVM GetById(long? id, int? siteSettingId, IpSections ipSections)
        {
            return
                db.Blogs
                .OrderByDescending(t => t.Id)
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
               .Select(t => new
               {
                   id = t.Id,
                   catId = t.BlogCategoryId,
                   catTitle = t.BlogCategory.Title,
                   title = t.Title,
                   publishDate = t.PublisheDate,
                   summery = t.Summery,
                   description = t.Description,
                   aparatUrl = t.VideoUrl,
                   mainImage_address = t.ImageUrl,
                   mainSound_address = t.SoundUrl,
                   isActive = t.IsActive,
                   commCount = t.BlogReviews.Count(tt => tt.IsConfirm == true),
                   likeCount = t.BlogLastLikeAndViews.Count(tt => tt.Type == BlogLastLikeAndViewType.Like),
                   didILikeIt = t.BlogLastLikeAndViews.Any(tt => tt.Type == BlogLastLikeAndViewType.Like && tt.Ip1 == ipSections.Ip1 && tt.Ip2 == ipSections.Ip2 && tt.Ip3 == ipSections.Ip3 && tt.Ip4 == ipSections.Ip4),
                   tags = t.BlogTagBlogs.Select(tt => tt.BlogTag.Title).ToList(),
                   rBlogs = t.BlogOwnBlogs.Select(tt => new
                   {
                       id = tt.BlogRelatedId,
                       title = tt.BlogRelated.Title,
                       catTitle = tt.BlogRelated.BlogCategory.Title,
                       src = tt.BlogRelated.ImageUrl200,
                       pDate = tt.BlogRelated.PublisheDate
                   }).ToList()
               })
               .Take(1)
               .ToList()
               .Select(t => new BlogVM
               {
                   id = t.id,
                   commCount = t.commCount,
                   likeCount = t.likeCount,
                   catId = t.catId,
                   catTitle = t.catTitle,
                   title = t.title,
                   publishDate = t.publishDate.ToFaDate(),
                   summery = t.summery,
                   url = GenerateUrlForBlog(t.catTitle, t.title, t.id),
                   description = t.description,
                   didILikeIt = t.didILikeIt,
                   aparatUrl = t.aparatUrl,
                   mainImage_address = !string.IsNullOrEmpty(t.mainImage_address) ? (GlobalConfig.FileAccessHandlerUrl + t.mainImage_address) : "",
                   mainSound_address = !string.IsNullOrEmpty(t.mainSound_address) ? (GlobalConfig.FileAccessHandlerUrl + t.mainSound_address) : "",
                   isActive = t.isActive,
                   tags = t.tags.Select(tt => new BlogTagVM { id = tt, title = tt }).ToList(),
                   rBlogs = t.rBlogs.Select(tt => new BlogVM
                   {
                       id = tt.id,
                       title = tt.title,
                       mainImage_address = !string.IsNullOrEmpty(tt.src) ? (GlobalConfig.FileAccessHandlerUrl + tt.src) : "",
                       url = GenerateUrlForBlog(tt.catTitle, tt.title, tt.id),
                       publishDate = tt.pDate.ToFaDate()
                   }).ToList()
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

            var qureResult = db.Blogs.OrderByDescending(t => t.Id).Where(t => t.SiteSettingId == siteSettingId);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public object GetTopBlogs(int count, int? siteSettingId)
        {
            return db.Blogs
                .Where(t => t.IsActive == true && t.PublisheDate <= DateTime.Now && t.SiteSettingId == siteSettingId)
                .OrderByDescending(t => t.Id)
                .Take(count)
                .Select(t => new
                {
                    img1 = GlobalConfig.FileAccessHandlerUrl + t.ImageUrl600,
                    img2 = GlobalConfig.FileAccessHandlerUrl + t.ImageUrl200,
                    title = t.Title,
                    desc = t.Summery,
                    cTitle = t.BlogCategory.Title,
                    id = t.Id
                })
                .ToList()
                .Select(t => new
                {
                    t.img1,
                    t.img2,
                    t.desc,
                    t.title,
                    url = GenerateUrlForBlog(t.cTitle, t.title, t.id)
                })
                .ToList()
                ;
        }



        public string GenerateUrlForBlog(string catTitle, string blogTitle, long? blogId)
        {
            return "/Blog/" + blogId + "/" + GlobalConfig.replaceInvalidChars(blogTitle) + "/" + GlobalConfig.replaceInvalidChars(catTitle);
        }

        public object Search(BlogSearchVM input, int? siteSettingId, int itemPerPage = 10)
        {
            var queryResult = db.Blogs.Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true && t.PublisheDate <= DateTime.Now);

            if (input.page == null || input.page <= 0)
                input.page = 1;

            if (input.sortID != null)
            {
                if (input.sortID == BlogSortTypes.Commented)
                    queryResult = queryResult.OrderByDescending(t => t.BlogReviews.Count(tt => tt.IsConfirm == true));
                else if (input.sortID == BlogSortTypes.Viewed)
                    queryResult = queryResult.OrderByDescending(t => t.BlogLastLikeAndViews.Count(tt => tt.Type == BlogLastLikeAndViewType.View));
            }
            else
                queryResult = queryResult.OrderBy(t => t.PublisheDate);

            if (input.typeId != null)
            {
                if (input.typeId == BlogTypes.Video)
                    queryResult = queryResult.Where(t => !string.IsNullOrEmpty(t.VideoUrl));
                else if (input.typeId == BlogTypes.Sound)
                    queryResult = queryResult.Where(t => !string.IsNullOrEmpty(t.SoundUrl));
                else
                    queryResult = queryResult.Where(t => string.IsNullOrEmpty(t.SoundUrl) && string.IsNullOrEmpty(t.VideoUrl));
            }

            if (input.catIds != null && input.catIds.Count > 0)
                queryResult = queryResult.Where(t => input.catIds.Contains(t.BlogCategoryId));

            if (input.keyWordId.ToLongReturnZiro() > 0)
                queryResult = queryResult.Where(t => t.BlogTagBlogs.Any(tt => tt.BlogTagId == input.keyWordId));

            int curPage = input.page.Value;
            var totalItem = queryResult.Count();

            return new
            {
                total = totalItem > 0 ? totalItem.ToString("###,###") : "0",
                pageCount = Math.Ceiling(Convert.ToDecimal(totalItem) / Convert.ToDecimal(itemPerPage)).ToIntReturnZiro(),
                data = queryResult
                .Skip(itemPerPage * (curPage - 1))
                .Take(itemPerPage)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    src = t.ImageUrl600,
                    summery = t.Summery,
                    createUserFullname = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                    createDate = t.PublisheDate,
                    catTitle = t.BlogCategory.Title,
                    fCount = t.BlogLastLikeAndViews.Count(tt => tt.Type == BlogLastLikeAndViewType.Like),
                    mCount = t.BlogReviews.Count(tt => tt.IsConfirm == true)
                })
                .ToList()
                .Select(t => new
                {
                    t.id,
                    t.title,
                    src = !string.IsNullOrEmpty(t.src) ? GlobalConfig.FileAccessHandlerUrl + t.src : "",
                    t.summery,
                    t.createUserFullname,
                    createDate = t.createDate.GetUserFrindlyDate(),
                    t.catTitle,
                    t.fCount,
                    t.mCount,
                    url = GenerateUrlForBlog(t.catTitle, t.title, t.id)
                })
                .ToList()
            };
        }

        public void SetViewOrLike(long id, IpSections ipSections, BlogLastLikeAndViewType type)
        {
            if (type == BlogLastLikeAndViewType.Like)
            {
                var foundItem = db.BlogLastLikeAndViews.Where(t => t.BlogId == id && t.Type == type && t.Ip1 == ipSections.Ip1 && t.Ip2 == ipSections.Ip2 && t.Ip3 == ipSections.Ip3 && t.Ip4 == ipSections.Ip4).FirstOrDefault();
                if (foundItem == null)
                {
                    db.Entry(new BlogLastLikeAndView()
                    {
                        BlogId = id,
                        Ip1 = ipSections.Ip1,
                        Ip2 = ipSections.Ip2,
                        Ip3 = ipSections.Ip3,
                        Ip4 = ipSections.Ip4,
                        Type = type,
                        CreateDate = DateTime.Now,
                    }).State = EntityState.Added;
                }
                else
                    db.Entry(foundItem).State = EntityState.Deleted;

                db.SaveChanges();
            }
            else
            {
                db.Entry(new BlogLastLikeAndView()
                {
                    BlogId = id,
                    Ip1 = ipSections.Ip1,
                    Ip2 = ipSections.Ip2,
                    Ip3 = ipSections.Ip3,
                    Ip4 = ipSections.Ip4,
                    Type = type,
                    CreateDate = DateTime.Now,
                }).State = EntityState.Added;
                try
                {
                    db.SaveChanges();
                }
                catch { }
            }

        }

        public List<BlogVM> GetMostTypeBlogs(int? siteSettingId, int count, BlogLastLikeAndViewType type, long id)
        {
            if (count <= 0)
                count = 10;

            return
                db.Blogs
                .Where(t => t.SiteSettingId == siteSettingId && t.Id != id)
                .OrderByDescending(t => t.BlogLastLikeAndViews.Count(tt => tt.Type == BlogLastLikeAndViewType.View))
                .Take(count)
                .Select(t => new
                {
                    t.Id,
                    t.Title,
                    t.ImageUrl50,
                    catTitle = t.BlogCategory.Title,
                    t.PublisheDate
                })
                .ToList()
                .Select(t => new BlogVM
                {
                    id = t.Id,
                    title = t.Title,
                    mainImage_address = !string.IsNullOrEmpty(t.ImageUrl50) ? (GlobalConfig.FileAccessHandlerUrl + t.ImageUrl50) : "",
                    url = GenerateUrlForBlog(t.catTitle, t.Title, t.Id),
                    publishDate = t.PublisheDate.ToFaDate()
                })
                .ToList();
        }

        void validateBlog(long id, int? siteSettingId)
        {
            if (!db.Blogs.Any(t => t.IsActive == true && t.PublisheDate <= DateTime.Now && t.Id == id && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Not_Found);
        }

        public object BlogActions(long id, BlogWebAction input, int? siteSettingId, IpSections ipSections)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.action))
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (ipSections == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);

            if (input.action == "likeOrDisLike")
            {
                BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.LikeBlog, BlockAutoIpAction.BeforeExecute, ipSections, siteSettingId);
                validateBlog(id, siteSettingId);
                SetViewOrLike(id, ipSections, BlogLastLikeAndViewType.Like);
                BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.LikeBlog, BlockAutoIpAction.AfterExecute, ipSections, siteSettingId);
                return getLikeCount(id);
            }
            else if (input.action == "newReview")
            {
                BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.BlogReview, BlockAutoIpAction.BeforeExecute, ipSections, siteSettingId);
                validateBlog(id, siteSettingId);
                var tempResult = BlogReviewService.Create(input, siteSettingId, ipSections, id);
                BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.BlogReview, BlockAutoIpAction.AfterExecute, ipSections, siteSettingId);
                return tempResult;
            }
            else if (input.action == "commentList")
                return BlogReviewService.GetConfirmList(siteSettingId, ipSections, id);
            else
                throw BException.GenerateNewException(BMessages.Validation_Error);

        }

        private int getLikeCount(long id)
        {
            return db.BlogLastLikeAndViews.Count(t => t.BlogId == id && t.Type == BlogLastLikeAndViewType.Like);
        }

        public BlogVM GetByIdForWeb(long? id, int? siteSettingId, IpSections ipSections)
        {
            return
                db.Blogs
                .OrderByDescending(t => t.Id)
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
               .Select(t => new
               {
                   id = t.Id,
                   catId = t.BlogCategoryId,
                   catTitle = t.BlogCategory.Title,
                   title = t.Title,
                   publishDate = t.PublisheDate,
                   summery = t.Summery,
                   description = t.Description,
                   aparatUrl = t.VideoUrl,
                   mainImage_address = t.ImageUrl600,
                   mainSound_address = t.SoundUrl,
                   isActive = t.IsActive,
                   commCount = t.BlogReviews.Count(tt => tt.IsConfirm == true),
                   likeCount = t.BlogLastLikeAndViews.Count(tt => tt.Type == BlogLastLikeAndViewType.Like),
                   didILikeIt = t.BlogLastLikeAndViews.Any(tt => tt.Type == BlogLastLikeAndViewType.Like && tt.Ip1 == ipSections.Ip1 && tt.Ip2 == ipSections.Ip2 && tt.Ip3 == ipSections.Ip3 && tt.Ip4 == ipSections.Ip4),
                   tags = t.BlogTagBlogs.Select(tt => new { title = tt.BlogTag.Title, id = tt.BlogTagId }).ToList(),
                   rBlogs = t.BlogOwnBlogs.Select(tt => new
                   {
                       id = tt.BlogRelatedId,
                       title = tt.BlogRelated.Title,
                       catTitle = tt.BlogRelated.BlogCategory.Title,
                       src = tt.BlogRelated.ImageUrl200,
                       pDate = tt.BlogRelated.PublisheDate
                   }).ToList()
               })
               .Take(1)
               .ToList()
               .Select(t => new BlogVM
               {
                   id = t.id,
                   commCount = t.commCount,
                   likeCount = t.likeCount,
                   catId = t.catId,
                   catTitle = t.catTitle,
                   title = t.title,
                   publishDate = t.publishDate.ToFaDate(),
                   summery = t.summery,
                   url = GenerateUrlForBlog(t.catTitle, t.title, t.id),
                   description = t.description,
                   didILikeIt = t.didILikeIt,
                   aparatUrl = t.aparatUrl,
                   mainImage_address = !string.IsNullOrEmpty(t.mainImage_address) ? (GlobalConfig.FileAccessHandlerUrl + t.mainImage_address) : "",
                   mainSound_address = !string.IsNullOrEmpty(t.mainSound_address) ? (GlobalConfig.FileAccessHandlerUrl + t.mainSound_address) : "",
                   isActive = t.isActive,
                   tags = t.tags.Select(tt => new BlogTagVM { id = tt.id + "", title = tt.title }).ToList(),
                   rBlogs = t.rBlogs.Select(tt => new BlogVM
                   {
                       id = tt.id,
                       title = tt.title,
                       mainImage_address = !string.IsNullOrEmpty(tt.src) ? (GlobalConfig.FileAccessHandlerUrl + tt.src) : "",
                       url = GenerateUrlForBlog(tt.catTitle, tt.title, tt.id),
                       publishDate = tt.pDate.ToFaDate()
                   }).ToList()
               })
               .FirstOrDefault();
        }
    }
}
