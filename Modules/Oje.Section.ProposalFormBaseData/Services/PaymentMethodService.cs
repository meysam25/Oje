using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Models.DB;
using Oje.Section.ProposalFormBaseData.Models.View;
using Oje.Section.ProposalFormBaseData.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.ProposalFormBaseData.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        readonly ProposalFormBaseDataDBContext db = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public PaymentMethodService(
            ProposalFormBaseDataDBContext db,
            AccountService.Interfaces.ISiteSettingService SiteSettingService,
            IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.SiteSettingService = SiteSettingService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(CreateUpdatePaymentMethodVM input)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            PaymentMethod newItem = new PaymentMethod()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                IsChek = input.isCheck.ToBooleanReturnFalse(),
                IsDefault = input.isDefault.ToBooleanReturnFalse(),
                Order = input.order.ToIntReturnZiro(),
                ProposalFormId = input.formId.ToIntReturnZiro(),
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Title = input.title,
                Type = input.type.Value,
                PrePayPercent = input.prePayPercent,
                DebitCount = input.debitCount
            };

            if (input.isDefault == true)
                setDefaultToFalse(input.id, siteSettingId, input.formId);

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.comIds != null && input.comIds.Count > 0)
            {
                foreach (var comId in input.comIds)
                {
                    db.Entry(new PaymentMethodCompany()
                    {
                        CompanyId = comId,
                        PaymentMethodId = newItem.Id
                    }).State = EntityState.Added;
                }
                db.SaveChanges();
            }



            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetEnumDisplayName() };
        }

        void setDefaultToFalse(int? id, int? siteSettingId, int? formId)
        {
            var foundList = db.PaymentMethods.Where(t => t.Id != id && t.SiteSettingId == siteSettingId && t.ProposalFormId == formId).ToList();
            foreach (var item in foundList)
                item.IsDefault = false;
        }

        void createUpdateValidation(CreateUpdatePaymentMethodVM input, int? siteSettingId, bool? canSetSiteSetting)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (input.formId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (input.comIds == null || input.comIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (input.prePayPercent != null && (input.prePayPercent < 0 || input.prePayPercent > 100))
                throw BException.GenerateNewException(BMessages.Invalid_Percent);
            if (!db.ProposalForms.Any(t => t.Id == input.formId && (t.SiteSettingId == null || t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId))))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
        }

        public ApiResult Delete(int? id)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            var deleteItem = db.PaymentMethods
                .Include(t => t.PaymentMethodCompanies)
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();
            if (deleteItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foreach (var company in deleteItem.PaymentMethodCompanies)
                db.Entry(company).State = EntityState.Deleted;

            db.Entry(deleteItem).State = EntityState.Deleted;
            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetEnumDisplayName() };
        }

        public object GetById(int? id)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            return db.PaymentMethods
                .Where(t => t.Id == id)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .OrderByDescending(t => t.Id)
                .Select(t => new
                {
                    id = t.Id,
                    comIds = t.PaymentMethodCompanies.Select(tt => tt.CompanyId).ToList(),
                    title = t.Title,
                    order = t.Order,
                    type = t.Type,
                    formId = t.ProposalFormId,
                    formId_Title = t.ProposalForm.Title,
                    isActive = t.IsActive,
                    isDefault = t.IsDefault,
                    isCheck = t.IsChek,
                    prePayPercent = t.PrePayPercent,
                    debitCount = t.DebitCount,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                })
                .Take(1)
                .ToList()
                .Select(t => new
                {
                    t.id,
                    t.comIds,
                    t.title,
                    t.order,
                    type = (int)t.type,
                    t.formId,
                    t.formId_Title,
                    t.isActive,
                    t.isDefault,
                    t.isCheck,
                    t.prePayPercent,
                    t.debitCount,
                    t.cSOWSiteSettingId,
                    t.cSOWSiteSettingId_Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<PaymentMethodMainGridResult> GetList(PaymentMethodMainGrid searchInput)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            if (searchInput == null)
                searchInput = new PaymentMethodMainGrid();

            var qureResult = db.PaymentMethods
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.form))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.form));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.comId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.PaymentMethodCompanies.Any(tt => tt.CompanyId == searchInput.comId));
            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.type);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.isDefault != null)
                qureResult = qureResult.Where(t => t.IsDefault == searchInput.isDefault);
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<PaymentMethodMainGridResult>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    form = t.ProposalForm.Title,
                    title = t.Title,
                    comId = t.PaymentMethodCompanies.Select(tt => tt.Company.Title).ToList(),
                    type = t.Type,
                    isActive = t.IsActive,
                    isDefault = t.IsDefault,
                    siteTitleMN2 = t.SiteSetting.Title
                })
                .ToList()
                .Select(t => new PaymentMethodMainGridResult
                {
                    id = t.id,
                    row = ++row,
                    form = t.form,
                    title = t.title,
                    comId = string.Join(",", t.comId),
                    type = t.type.GetEnumDisplayName(),
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    isDefault = t.isDefault == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                    siteTitleMN2 = t.siteTitleMN2
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdatePaymentMethodVM input)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            createUpdateValidation(input, siteSettingId, canSetSiteSetting);

            PaymentMethod foundItem = db.PaymentMethods
                .Where(t => t.Id == input.id)
                .Include(t => t.PaymentMethodCompanies)
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.IsChek = input.isCheck.ToBooleanReturnFalse();
            foundItem.IsDefault = input.isDefault.ToBooleanReturnFalse();
            foundItem.Order = input.order.ToIntReturnZiro();
            foundItem.ProposalFormId = input.formId.ToIntReturnZiro();
            foundItem.Title = input.title;
            foundItem.Type = input.type.Value;
            foundItem.PrePayPercent = input.prePayPercent;
            foundItem.DebitCount = input.debitCount;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            if (foundItem.PaymentMethodCompanies != null)
                foreach (var company in foundItem.PaymentMethodCompanies)
                    db.Entry(company).State = EntityState.Deleted;

            if (input.comIds != null && input.comIds.Count > 0)
            {
                foreach (var comId in input.comIds)
                {
                    db.Entry(new PaymentMethodCompany()
                    {
                        CompanyId = comId,
                        PaymentMethodId = foundItem.Id
                    }).State = EntityState.Added;
                }
            }

            if (input.isDefault == true)
                setDefaultToFalse(input.id, siteSettingId, input.formId);

            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetEnumDisplayName() };
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.PaymentMethods.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
