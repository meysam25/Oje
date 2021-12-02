using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBaseData.Interfaces;
using Oje.Section.CarBaseData.Models.View;
using Oje.Section.CarBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.CarBaseData.Services.EContext;

namespace Oje.Section.CarBaseData.Services
{
    public class CarExteraDiscountManager : ICarExteraDiscountManager
    {
        readonly CarDBContext db = null;
        public CarExteraDiscountManager(CarDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateCarExteraDiscountVM input)
        {
            CreateValidation(input);

            db.Entry(new CarExteraDiscount()
            {
                ProposalFormId = input.formId.Value,
                Title = input.title,
                IsOption = input.isOption.ToBooleanReturnFalse(),
                Type = input.type.Value,
                CarTypeId = input.cTypeId,
                CalculateType = input.calcType.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                HasPrevInsurance = input.hasPrevInsurance,
                Description = input.description,
                CarExteraDiscountCategoryId = input.catId,
                DontRemoveInSearch = input.dotRemove,
                Order = input.order
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateCarExteraDiscountVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.formId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (input.calcType == null)
                throw BException.GenerateNewException(BMessages.Please_Select_CalcType);
            if (!string.IsNullOrEmpty(input.description) && input.description.Length > 4000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (db.CarExteraDiscounts.Any(t => t.Title == input.title && t.ProposalFormId == input.formId && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.CarExteraDiscounts.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateCarExteraDiscountVM GetById(int? id)
        {
            return db.CarExteraDiscounts.Where(t => t.Id == id).Select(t => new CreateUpdateCarExteraDiscountVM
            {
                id = t.Id,
                calcType = t.CalculateType,
                cTypeId = t.CarTypeId,
                description = t.Description,
                formId = t.ProposalFormId,
                formId_Title = t.ProposalForm.Title,
                hasPrevInsurance = t.HasPrevInsurance,
                isActive = t.IsActive,
                isOption = t.IsOption,
                title = t.Title,
                type = t.Type,
                order = t.Order,
                dotRemove = t.DontRemoveInSearch == null ? false : t.DontRemoveInSearch.Value,
                catId = t.CarExteraDiscountCategoryId
            }).FirstOrDefault();
        }

        public GridResultVM<CarExteraDiscountMainGridResultVM> GetList(CarExteraDiscountMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new CarExteraDiscountMainGrid();

            var qureResult = db.CarExteraDiscounts.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.formTitle))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.formTitle));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.isOption != null)
                qureResult = qureResult.Where(t => t.IsOption == searchInput.isOption);

            int row = searchInput.skip;


            return new GridResultVM<CarExteraDiscountMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    formTitle = t.ProposalForm.Title,
                    title = t.Title,
                    isActive = t.IsActive,
                    isOption = t.IsOption
                })
                .ToList()
                .Select(t => new CarExteraDiscountMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    formTitle = t.formTitle,
                    title = t.title,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    isOption = t.isOption == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateCarExteraDiscountVM input)
        {
            CreateValidation(input);

            var foundItem = db.CarExteraDiscounts.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.ProposalFormId = input.formId.Value;
            foundItem.Title = input.title;
            foundItem.IsOption = input.isOption.ToBooleanReturnFalse();
            foundItem.Type = input.type.Value;
            foundItem.CarTypeId = input.cTypeId;
            foundItem.CalculateType = input.calcType.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.HasPrevInsurance = input.hasPrevInsurance;
            foundItem.Description = input.description;
            foundItem.Order = input.order;
            foundItem.CarExteraDiscountCategoryId = input.catId;
            foundItem.DontRemoveInSearch = input.dotRemove;

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

            var qureResult = db.CarExteraDiscounts.OrderByDescending(t => t.Id).AsQueryable();
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
