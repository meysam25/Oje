using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Models.View;
using Oje.Section.InquiryBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.InquiryBaseData.Services.EContext;

namespace Oje.Section.InquiryBaseData.Services
{
    public class NoDamageDiscountService : INoDamageDiscountService
    {
        readonly InquiryBaseDataDBContext db = null;
        public NoDamageDiscountService(
                InquiryBaseDataDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateNoDamageDiscountVM input)
        {
            CreateValidation(input);

            NoDamageDiscount newItem = new NoDamageDiscount()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Order = input.order.ToIntReturnZiro(),
                Percent = input.percent.Value,
                ProposalFormId = input.ppfId.Value,
                Title = input.title
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            input.cIds = input.cIds.GroupBy(t => t).Select(t => t.Key).ToList();

            foreach (var cid in input.cIds)
            {
                db.Entry(new NoDamageDiscountCompany() { CompanyId = cid, NoDamageDiscountId = newItem.Id }).State = EntityState.Added;
            }
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateNoDamageDiscountVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.percent == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Percent);
            if (input.percent > 100 || input.percent < 0)
                throw BException.GenerateNewException(BMessages.Invalid_Percent);
            if (input.ppfId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (input.id.ToIntReturnZiro() <= 0 && (input.cIds == null || input.cIds.Count == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (db.NoDamageDiscounts.Any(t => t.Id != input.id && t.Title == input.title && t.ProposalFormId == input.ppfId))
                throw BException.GenerateNewException(BMessages.Dublicate_Title);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.NoDamageDiscounts.Where(t => t.Id == id).Include(t => t.NoDamageDiscountCompanies).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);
            if (foundItem.NoDamageDiscountCompanies != null && foundItem.NoDamageDiscountCompanies.Count > 0)
                foreach (var item in foundItem.NoDamageDiscountCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateNoDamageDiscountVM GetById(int? id)
        {
            return db.NoDamageDiscounts.Where(t => t.Id == id).Select(t => new CreateUpdateNoDamageDiscountVM
            {
                id = t.Id,
                cIds = t.NoDamageDiscountCompanies.Select(tt => tt.CompanyId).ToList(),
                isActive = t.IsActive,
                order = t.Order,
                title = t.Title,
                percent = t.Percent,
                ppfId = t.ProposalFormId,
                ppfId_Title = t.ProposalForm.Title
            }).FirstOrDefault();
        }

        public GridResultVM<NoDamageDiscountMainGridResultVM> GetList(NoDamageDiscountMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new NoDamageDiscountMainGrid();

            var qureResult = db.NoDamageDiscounts.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.cId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.NoDamageDiscountCompanies.Any(tt => tt.CompanyId == searchInput.cId));
            if (!string.IsNullOrEmpty(searchInput.formTitle))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.formTitle));
            if (searchInput.pecent != null && searchInput.pecent >= 0)
                qureResult = qureResult.Where(t => t.Percent == searchInput.pecent);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            var row = searchInput.skip;

            return new GridResultVM<NoDamageDiscountMainGridResultVM>
            {
                total = qureResult.Count(),
                data = qureResult.OrderBy(t => t.Order).ThenByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    cId = t.NoDamageDiscountCompanies.Select(tt => tt.Company.Title).ToList(),
                    formTitle = t.ProposalForm.Title,
                    pecent = t.Percent,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new NoDamageDiscountMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    cId = string.Join(",", t.cId),
                    formTitle = t.formTitle,
                    pecent = t.pecent,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateNoDamageDiscountVM input)
        {
            CreateValidation(input);

            var foundItem = db.NoDamageDiscounts.Include(t => t.NoDamageDiscountCompanies).Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);
            if (foundItem.NoDamageDiscountCompanies != null && foundItem.NoDamageDiscountCompanies.Count > 0)
                foreach (var item in foundItem.NoDamageDiscountCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.ProposalFormId = input.ppfId.Value;
            foundItem.Order = input.order.ToIntReturnZiro();
            foundItem.Title = input.title;
            foundItem.Percent = input.percent.Value;


            if (input.cIds != null && input.cIds.Count > 0)
            {
                input.cIds = input.cIds.GroupBy(t => t).Select(t => t.Key).ToList();

                foreach (var cid in input.cIds)
                {
                    db.Entry(new NoDamageDiscountCompany() { CompanyId = cid, NoDamageDiscountId = foundItem.Id }).State = EntityState.Added;
                }
            }

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
