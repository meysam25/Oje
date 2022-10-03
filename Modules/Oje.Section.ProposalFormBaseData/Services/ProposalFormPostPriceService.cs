using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.ProposalFormBaseData.Services.EContext;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Models.View;
using Microsoft.AspNetCore.Http;

namespace Oje.Section.ProposalFormBaseData.Services
{
    public class ProposalFormPostPriceService : IProposalFormPostPriceService
    {
        readonly ProposalFormBaseDataDBContext db = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public ProposalFormPostPriceService(
                ProposalFormBaseDataDBContext db,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IProposalFormService ProposalFormService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.SiteSettingService = SiteSettingService;
            this.ProposalFormService = ProposalFormService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(CreateUpdateProposalFormPostPriceVM input)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            CreateValidation(input, siteSettingId);

            db.Entry(new ProposalFormPostPrice()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Price = input.price.ToIntReturnZiro(),
                ProposalFormId = input.ppfId.ToIntReturnZiro(),
                SiteSettingId = siteSettingId.Value,
                Title = input.title
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateProposalFormPostPriceVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.ppfId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (!ProposalFormService.Exist(input.ppfId.ToIntReturnZiro(), siteSettingId))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.price.ToIntReturnZiro() < 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            if (db.ProposalFormPostPrices.Any(t => t.Id != input.id && t.SiteSettingId == siteSettingId && t.ProposalFormId == input.ppfId && t.Title == input.title))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            var foundItem = db.ProposalFormPostPrices
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateProposalFormPostPriceVM GetById(int? id)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            return db.ProposalFormPostPrices
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new CreateUpdateProposalFormPostPriceVM
                {
                    id = t.Id,
                    isActive = t.IsActive,
                    ppfId = t.ProposalFormId,
                    price = t.Price,
                    title = t.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<ProposalFormPostPriceMainGridResultVM> GetList(ProposalFormPostPriceMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new ProposalFormPostPriceMainGrid();

            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            var qureResult = db.ProposalFormPostPrices.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.ppf))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.ppf));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.price  != null && searchInput.price.ToIntReturnZiro() >= 0)
                qureResult = qureResult.Where(t => t.Price == searchInput.price);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<ProposalFormPostPriceMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    isActive = t.IsActive,
                    ppf = t.ProposalForm.Title,
                    price = t.Price,
                    title = t.Title,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new ProposalFormPostPriceMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    ppf = t.ppf,
                    title = t.title,
                    price = t.price == 0 ? "0" : t.price.ToString("###,###"),
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateProposalFormPostPriceVM input)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            CreateValidation(input, siteSettingId);

            var foundItem = db.ProposalFormPostPrices
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Price = input.price.ToIntReturnZiro();
            foundItem.ProposalFormId = input.ppfId.ToIntReturnZiro();
            foundItem.Title = input.title;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
