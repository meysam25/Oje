using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.DB;
using Oje.Section.WebMain.Models.View;
using Oje.Section.WebMain.Services.EContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Oje.Section.WebMain.Services
{
    public class PageService : IPageService
    {
        readonly WebMainDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IPageLeftRightDesignService PageLeftRightDesignService = null;
        readonly IPageManifestService PageManifestService = null;
        readonly IPageSliderService PageSliderService = null;

        public PageService
            (
                WebMainDBContext db,
                IUploadedFileService UploadedFileService,
                IPageLeftRightDesignService PageLeftRightDesignService,
                IPageManifestService PageManifestService,
                IPageSliderService PageSliderService
            )
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
            this.PageLeftRightDesignService = PageLeftRightDesignService;
            this.PageManifestService = PageManifestService;
            this.PageSliderService = PageSliderService;
        }

        public ApiResult Create(PageCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var newItem = new Page()
            {
                Title = input.title,
                ButtonLink = input.bLink,
                ButtonTitle = input.bTitle,
                SiteSettingId = siteSettingId.ToIntReturnZiro(),
                SubTitle = input.subTitle,
                Summery = input.summery,
                MainImage = " ",
                MainImageSmall = " ",
                TitleAndSubtitleColorCode = input.stColor,
                CreateDate = DateTime.Now
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            try
            {
                if (input.mainImage != null && input.mainImage.Length > 0)
                    newItem.MainImage = UploadedFileService.UploadNewFile(FileType.PageMainImage, input.mainImage, null, siteSettingId, newItem.Id, ".png,.jpg,.jpeg", false);
                if (input.mainImageSmall != null && input.mainImageSmall.Length > 0)
                    newItem.MainImageSmall = UploadedFileService.UploadNewFile(FileType.PageMainImageSmall, input.mainImageSmall, null, siteSettingId, newItem.Id, ".png,.jpg,.jpeg", false);
            }
            catch (Exception)
            {
                Delete(newItem.Id, siteSettingId);
                throw;
            }

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(PageCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 200)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_200_chars);
            if (!string.IsNullOrEmpty(input.subTitle) && input.subTitle.Length > 200)
                throw BException.GenerateNewException(BMessages.SubTitle_Can_Not_Be_More_Then_200_chars);
            if (string.IsNullOrEmpty(input.summery))
                throw BException.GenerateNewException(BMessages.Please_Enter_Summery);
            if (input.summery.Length > 1000)
                throw BException.GenerateNewException(BMessages.Summery_Can_Not_Be_More_then_1000_Chars);
            if (!string.IsNullOrEmpty(input.bTitle) && input.bTitle.Length > 50)
                throw BException.GenerateNewException(BMessages.Button_Title_Can_Not_Be_More_Then_50_Chars);
            if (!string.IsNullOrEmpty(input.bLink) && input.bLink.Length > 200)
                throw BException.GenerateNewException(BMessages.Button_Link_Can_Not_Be_More_Then_200_Chars);
            if (input.id.ToLongReturnZiro() <= 0 && (input.mainImage == null || input.mainImage.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_Main_Image);
            if (input.id.ToLongReturnZiro() <= 0 && (input.mainImageSmall == null || input.mainImageSmall.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_Main_Image);
            if (input.mainImage != null && input.mainImage.Length > 0 && !UploadedFileService.IsValidImageSize(input.mainImage, true, 4.5m, 5.5m))
                throw BException.GenerateNewException(BMessages.Invalid_Image_Size);
            if (input.mainImageSmall != null && input.mainImageSmall.Length > 0 && !UploadedFileService.IsValidImageSize(input.mainImageSmall, true, 0.833m, 1.833m))
                throw BException.GenerateNewException(BMessages.Invalid_Image_Size);

        }

        public ApiResult Delete(long? id, int? siteSettingId)
        {
            var fopundItem = db.Pages.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (fopundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(fopundItem).State = EntityState.Deleted;

            try { db.SaveChanges(); } catch { }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.Pages
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    subTitle = t.SubTitle,
                    summery = t.Summery,
                    bTitle = t.ButtonTitle,
                    bLink = t.ButtonLink,
                    mainImage_address = GlobalConfig.FileAccessHandlerUrl + t.MainImage,
                    mainImageSmall_address = GlobalConfig.FileAccessHandlerUrl + t.MainImageSmall,
                    stColor = t.TitleAndSubtitleColorCode
                })
                .FirstOrDefault();
        }

        public string GenerateUrlForPage(string title, long? pageId)
        {
            return "/Page/" + pageId + "/" + GlobalConfig.replaceInvalidChars(title);
        }

        public GridResultVM<PageMainGridResultVM> GetList(PageMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new PageMainGrid();

            var qureResult = db.Pages.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.summery))
                qureResult = qureResult.Where(t => t.Summery.Contains(searchInput.summery));

            int row = searchInput.skip;

            return new GridResultVM<PageMainGridResultVM>()
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
                    summery = t.Summery
                })
                .ToList()
                .Select(t => new PageMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    summery = t.summery,
                    url = GenerateUrlForPage(t.title, t.id)
                })
                .ToList()
            };
        }

        public ApiResult Update(PageCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var fopundItem = db.Pages.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (fopundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            fopundItem.Title = input.title;
            fopundItem.ButtonLink = input.bLink;
            fopundItem.ButtonTitle = input.bTitle;
            fopundItem.SubTitle = input.subTitle;
            fopundItem.Summery = input.summery;
            fopundItem.TitleAndSubtitleColorCode = input.stColor;

            if (input.mainImage != null && input.mainImage.Length > 0)
                fopundItem.MainImage = UploadedFileService.UploadNewFile(FileType.PageMainImage, input.mainImage, null, siteSettingId, fopundItem.Id, ".png,.jpg,.jpeg", false);
            if (input.mainImageSmall != null && input.mainImageSmall.Length > 0)
                fopundItem.MainImageSmall = UploadedFileService.UploadNewFile(FileType.PageMainImageSmall, input.mainImageSmall, null, siteSettingId, fopundItem.Id, ".png,.jpg,.jpeg", false);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetSelect2(Select2SearchVM searchInput, int? siteSettingId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.Pages.Where(t => t.SiteSettingId == siteSettingId);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public PageWebVM GetBy(long? id, string pTitle, int? siteSettingId)
        {
            PageWebVM result = null;
            var foundPage = db.Pages.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();

            if (foundPage == null)
                return result;

            if (GenerateUrlForPage(foundPage.Title, foundPage.Id) != GenerateUrlForPage(pTitle, foundPage.Id))
                return result;

            result = new PageWebVM()
            {
                bLink = foundPage.ButtonLink,
                bTitle = foundPage.ButtonTitle,
                subTitle = foundPage.SubTitle,
                summery = foundPage.Summery,
                stColorCode = foundPage.TitleAndSubtitleColorCode,
                title = foundPage.Title,
                mainImage = GlobalConfig.FileAccessHandlerUrl + foundPage.MainImage,
                mainImageSmall = GlobalConfig.FileAccessHandlerUrl + foundPage.MainImageSmall,
                id = foundPage.Id,
                url = GenerateUrlForPage(foundPage.Title, foundPage.Id),
                createDate = foundPage.CreateDate,
            };
            
            result.Items.AddRange(PageLeftRightDesignService.GetListForWeb(id, siteSettingId));
            result.Items.AddRange(PageManifestService.GetListForWeb(id, siteSettingId));
            result.PageWebSliderVMs = PageSliderService.GetLightList(id, siteSettingId);

            return result;
        }

        public bool Exist(long? id, int? siteSettingId)
        {
            return db.Pages.Any(t => t.Id == id && t.SiteSettingId == siteSettingId);
        }

        public string GetSiteMap(int? siteSettingId, string baseUrl)
        {
            string siteMapBaseFolder = GlobalConfig.GetSiteMapBaseFolder();
            string siteMapFilename = Path.Combine(siteMapBaseFolder, "SiteMap_"+ siteSettingId + ".xml");

            if (File.Exists(siteMapFilename))
            {
                var fileCreateTime = File.GetCreationTime(siteMapFilename);
                if ((DateTime.Now - fileCreateTime).TotalDays > 1)
                {
                    File.Delete(siteMapFilename);
                    File.WriteAllText(siteMapFilename, getPageXml(siteSettingId, baseUrl));
                }
            }
            else
                File.WriteAllText(siteMapFilename, getPageXml(siteSettingId, baseUrl));

            return File.ReadAllText(siteMapFilename);
        }

        string getPageXml(int? siteSettingId, string baseUrl)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            var allPages = db.Pages
                .OrderByDescending(t => t.Id)
                .Where(t => t.SiteSettingId == siteSettingId)
               .Select(t => new
               {
                   id = t.Id,
                   title = t.Title,
                   publishDate = t.CreateDate,
               })
               .ToList();

            sb.Append("<url>");
            sb.Append("<loc>" + baseUrl + "/Reminder" + "</loc>");
            sb.Append("<lastmod>2022-05-05</lastmod>");
            sb.Append("</url>");

            sb.Append("<url>");
            sb.Append("<loc>" + baseUrl + "/CarThirdPartyInquiry" + "</loc>");
            sb.Append("<lastmod>2022-05-06</lastmod>");
            sb.Append("</url>");

            sb.Append("<url>");
            sb.Append("<loc>" + baseUrl + "/CarBodyInquiry" + "</loc>");
            sb.Append("<lastmod>2022-05-09</lastmod>");
            sb.Append("</url>");

            sb.Append("<url>");
            sb.Append("<loc>" + baseUrl + "/FireInsurance" + "</loc>");
            sb.Append("<lastmod>2022-05-19</lastmod>");
            sb.Append("</url>");

            foreach (var blog in allPages)
            {
                sb.Append("<url>");
                sb.Append("<loc>" + baseUrl + GenerateUrlForPage(blog.title, blog.id) + "</loc>");
                sb.Append("<lastmod>" + blog.publishDate.ToString("yyyy-MM-dd") + "</lastmod>");
                sb.Append("</url>");
            }

            sb.Append("</urlset>");

            return sb.ToString();
        }
    }
}
