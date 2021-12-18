using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarThirdBaseData.Interfaces;
using Oje.Section.CarThirdBaseData.Models.View;
using Oje.Section.CarThirdBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.CarThirdBaseData.Services.EContext;

namespace Oje.Section.CarThirdBaseData.Services
{
    public class ThirdPartyRequiredFinancialCommitmentService : IThirdPartyRequiredFinancialCommitmentService
    {
        readonly CarThirdBaseDataDBContext db = null;
        public ThirdPartyRequiredFinancialCommitmentService(CarThirdBaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateThirdPartyRequiredFinancialCommitmentVM input)
        {
            createValidation(input);

            var newItem = new ThirdPartyRequiredFinancialCommitment()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Order = input.order.ToIntReturnZiro(),
                Price = input.price.ToLongReturnZiro(),
                Title = input.title
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.cIds != null)
                foreach (int cId in input.cIds)
                    db.Entry(new ThirdPartyRequiredFinancialCommitmentCompany() { CompanyId = cId, ThirdPartyRequiredFinancialCommitmentId = newItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateThirdPartyRequiredFinancialCommitmentVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.cIds == null || input.cIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.price.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Price);

        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.ThirdPartyRequiredFinancialCommitments.Where(t => t.Id == id).Include(t => t.ThirdPartyRequiredFinancialCommitmentCompanies).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.ThirdPartyRequiredFinancialCommitmentCompanies != null)
                foreach (var item in foundItem.ThirdPartyRequiredFinancialCommitmentCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateThirdPartyRequiredFinancialCommitmentVM GetById(int? id)
        {
            return db.ThirdPartyRequiredFinancialCommitments
                .Where(t => t.Id == id)
                .Select(t => new CreateUpdateThirdPartyRequiredFinancialCommitmentVM
                {
                    id = t.Id,
                    cIds = t.ThirdPartyRequiredFinancialCommitmentCompanies.Select(tt => tt.CompanyId).ToList(),
                    isActive = t.IsActive,
                    order = t.Order,
                    price = t.Price,
                    title = t.Title
                }).FirstOrDefault();
        }

        public GridResultVM<ThirdPartyRequiredFinancialCommitmentMainGridResultVM> GetList(ThirdPartyRequiredFinancialCommitmentMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new ThirdPartyRequiredFinancialCommitmentMainGrid();

            var resultQure = db.ThirdPartyRequiredFinancialCommitments.AsQueryable();

            if (searchInput.company.ToIntReturnZiro() > 0)
                resultQure = resultQure.Where(t => t.ThirdPartyRequiredFinancialCommitmentCompanies.Any(tt => tt.CompanyId == searchInput.company));
            if (!string.IsNullOrEmpty(searchInput.title))
                resultQure = resultQure.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.price > 0)
                resultQure = resultQure.Where(t => t.Price == searchInput.price);
            if (searchInput.order.ToIntReturnZiro() > 0)
                resultQure = resultQure.Where(t => t.Order == searchInput.order);
            if (searchInput.isActive != null)
                resultQure = resultQure.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<ThirdPartyRequiredFinancialCommitmentMainGridResultVM>()
            {
                total = resultQure.Count(),
                data = resultQure.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new 
                {
                    id = t.Id,
                    company = t.ThirdPartyRequiredFinancialCommitmentCompanies.Select(tt => tt.Company.Title).ToList(),
                    title = t.Title,
                    price = t.Price,
                    order = t.Order,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new ThirdPartyRequiredFinancialCommitmentMainGridResultVM 
                {
                    row = ++row,
                    id = t.id,
                    company = string.Join(",", t.company),
                    isActive = t.isActive == true? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    order = t.order,
                    price = t.price.ToString("###,###"),
                    title = t.title
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateThirdPartyRequiredFinancialCommitmentVM input)
        {
            createValidation(input);

            var foundItem = db.ThirdPartyRequiredFinancialCommitments.Where(t => t.Id == input.id).Include(t => t.ThirdPartyRequiredFinancialCommitmentCompanies).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.ThirdPartyRequiredFinancialCommitmentCompanies != null)
                foreach (var item in foundItem.ThirdPartyRequiredFinancialCommitmentCompanies)
                    db.Entry(item).State = EntityState.Deleted;

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Order = input.order.ToIntReturnZiro();
            foundItem.Price = input.price.ToLongReturnZiro();
            foundItem.Title = input.title;

            if (input.cIds != null)
                foreach (int cId in input.cIds)
                    db.Entry(new ThirdPartyRequiredFinancialCommitmentCompany() { CompanyId = cId, ThirdPartyRequiredFinancialCommitmentId = foundItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetSelect2List(Select2SearchVM searchInput)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.ThirdPartyRequiredFinancialCommitments.OrderByDescending(t => t.Id).AsQueryable();
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}
