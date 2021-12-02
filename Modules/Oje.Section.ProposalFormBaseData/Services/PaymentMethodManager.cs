using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Models.DB;
using Oje.Section.ProposalFormBaseData.Models.View;
using Oje.Section.ProposalFormBaseData.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Services
{
    public class PaymentMethodManager : IPaymentMethodManager
    {
        readonly ProposalFormBaseDataDBContext db = null;
        readonly AccountManager.Interfaces.ISiteSettingManager SiteSettingManager = null;
        public PaymentMethodManager(
            ProposalFormBaseDataDBContext db,
            AccountManager.Interfaces.ISiteSettingManager SiteSettingManager
            )
        {
            this.db = db;
            this.SiteSettingManager = SiteSettingManager;
        }

        public ApiResult Create(CreateUpdatePaymentMethodVM input)
        {
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            createUpdateValidation(input, siteSettingId);

            PaymentMethod newItem = new PaymentMethod()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                IsChek = input.isCheck.ToBooleanReturnFalse(),
                IsDefault = input.isDefault.ToBooleanReturnFalse(),
                Order = input.order.ToIntReturnZiro(),
                ProposalFormId = input.formId.ToIntReturnZiro(),
                SiteSettingId = siteSettingId.Value,
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

        void createUpdateValidation(CreateUpdatePaymentMethodVM input, int? siteSettingId)
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
        }

        public ApiResult Delete(int? id)
        {
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;

            var deleteItem = db.PaymentMethods.Include(t => t.PaymentMethodCompanies).Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();
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
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;

            return db.PaymentMethods
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
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
                    debitCount = t.DebitCount
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
                    t.debitCount
                })
                .FirstOrDefault();
        }

        public GridResultVM<PaymentMethodMainGridResult> GetList(PaymentMethodMainGrid searchInput)
        {
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            if (searchInput == null)
                searchInput = new PaymentMethodMainGrid();

            var qureResult = db.PaymentMethods.Where(t => t.SiteSettingId == siteSettingId);

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
                    isDefault = t.IsDefault
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
                    isDefault = t.isDefault == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdatePaymentMethodVM input)
        {
            int? siteSettingId = SiteSettingManager.GetSiteSetting()?.Id;
            createUpdateValidation(input, siteSettingId);

            PaymentMethod editItem = db.PaymentMethods.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId).Include(t => t.PaymentMethodCompanies).FirstOrDefault();
            editItem.IsActive = input.isActive.ToBooleanReturnFalse();
            editItem.IsChek = input.isCheck.ToBooleanReturnFalse();
            editItem.IsDefault = input.isDefault.ToBooleanReturnFalse();
            editItem.Order = input.order.ToIntReturnZiro();
            editItem.ProposalFormId = input.formId.ToIntReturnZiro();
            editItem.Title = input.title;
            editItem.Type = input.type.Value;
            editItem.PrePayPercent = input.prePayPercent;
            editItem.DebitCount = input.debitCount;

            if (editItem.PaymentMethodCompanies != null)
                foreach (var company in editItem.PaymentMethodCompanies)
                    db.Entry(company).State = EntityState.Deleted;

            if (input.comIds != null && input.comIds.Count > 0)
            {
                foreach (var comId in input.comIds)
                {
                    db.Entry(new PaymentMethodCompany()
                    {
                        CompanyId = comId,
                        PaymentMethodId = editItem.Id
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
