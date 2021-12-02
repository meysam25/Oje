using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.ProposalFormBaseData.Services.EContext;
using Oje.Section.ProposalFormBaseData.Models.View;

namespace Oje.Section.ProposalFormBaseData.Services
{
    public class ProposalFormCategoryManager : IProposalFormCategoryManager
    {
        readonly ProposalFormBaseDataDBContext db = null;
        public ProposalFormCategoryManager(ProposalFormBaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateProposalFormCategoryVM input)
        {
            CreateValidation(input);

            db.Entry(new ProposalFormCategory() { Title = input.title }).State = EntityState.Added;
            db.SaveChanges();


            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        void CreateValidation(CreateUpdateProposalFormCategoryVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title, ApiResultErrorCode.ValidationError);
            if (db.ProposalFormCategories.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title, ApiResultErrorCode.ValidationError);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.ProposalFormCategories.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();
            
            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>() ?.Name };
        }

        public object GetById(int? id)
        {
            return db.ProposalFormCategories.Where(t => t.Id == id).Select(t => new 
            {
                id = t.Id,
                title = t.Title
            }).FirstOrDefault();
        }

        public GridResultVM<ProposalFormCategoryMainGridResultVM> GetList(ProposalFormCategoryMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new ProposalFormCategoryMainGrid();

            var queryResult = db.ProposalFormCategories.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                queryResult = queryResult.Where(t => t.Title.Contains(searchInput.title));

            int row = searchInput.skip;

            return new GridResultVM<ProposalFormCategoryMainGridResultVM>() 
            {
                total = queryResult.Count(),
                data = queryResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new 
                {
                    id = t.Id,
                    title = t.Title,
                    proposalFormCount = t.ProposalForms.Count()
                })
                .ToList()
                .Select(t => new ProposalFormCategoryMainGridResultVM 
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    proposalFormCount = t.proposalFormCount
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateProposalFormCategoryVM input)
        {
            CreateValidation(input);

            var foundItem = db.ProposalFormCategories.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Title = input.title;

            db.SaveChanges();


            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
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

            var qureResult = db.ProposalFormCategories.OrderByDescending(t => t.Id).AsQueryable();
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
