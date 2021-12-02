using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.FireBaseData.Interfaces;
using Oje.Section.FireBaseData.Models.View;
using Oje.Section.FireBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.FireBaseData.Services.EContext;

namespace Oje.Section.FireBaseData.Services
{
    public class FireInsuranceCoverageManager : IFireInsuranceCoverageManager
    {
        readonly FireBaseDataDBContext db = null;
        public FireInsuranceCoverageManager(FireBaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateFireInsuranceCoverageVM input)
        {
            createValidation(input);

            var newItem = new FireInsuranceCoverage()
            {
                FireInsuranceCoverageTitleId = input.titleId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                ProposalFormId = input.pfId.Value,
                Rate = input.rate.Value,
                Title = input.title
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.cIds != null && input.cIds.Count > 0)
                foreach (var cid in input.cIds)
                    db.Entry(new FireInsuranceCoverageCompany() { CompanyId = cid, FireInsuranceCoverageId = newItem.Id }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateFireInsuranceCoverageVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.titleId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Cover);
            if (input.cIds == null || input.cIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (input.pfId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.rate == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Rate);
            if (input.rate <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Rate);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.FireInsuranceCoverages.Include(t => t.FireInsuranceCoverageCompanies).Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.FireInsuranceCoverageCompanies != null)
                foreach (var com in foundItem.FireInsuranceCoverageCompanies)
                    db.Entry(com).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateFireInsuranceCoverageVM GetById(int? id)
        {
            return db.FireInsuranceCoverages.Where(t => t.Id == id).Select(t => new CreateUpdateFireInsuranceCoverageVM
            {
                id = t.Id,
                cIds = t.FireInsuranceCoverageCompanies.Select(tt => tt.CompanyId).ToList(),
                isActive = t.IsActive,
                pfId = t.ProposalFormId,
                pfId_Title = t.ProposalForm.Title,
                rate = t.Rate,
                title = t.Title,
                titleId = t.FireInsuranceCoverageTitleId,
                titleId_Title = t.FireInsuranceCoverageTitle.Title
            }).FirstOrDefault();
        }

        public GridResultVM<FireInsuranceCoverageMainGridResultVM> GetList(FireInsuranceCoverageMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new FireInsuranceCoverageMainGrid();

            var qureResult = db.FireInsuranceCoverages.AsQueryable();

            if (searchInput.company.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.FireInsuranceCoverageCompanies.Any(tt => tt.CompanyId == searchInput.company));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.titleId))
                qureResult = qureResult.Where(t => t.FireInsuranceCoverageTitle.Title.Contains(searchInput.titleId));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.rate != null && searchInput.rate > 0)
                qureResult = qureResult.Where(t => t.Rate == searchInput.rate);
            if (!string.IsNullOrEmpty(searchInput.ppfTitle))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.ppfTitle));

            int row = searchInput.skip;

            return new GridResultVM<FireInsuranceCoverageMainGridResultVM> 
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new 
                {
                    id = t.Id,
                    company = t.FireInsuranceCoverageCompanies.Select(tt => tt.Company.Title).ToList(),
                    titleId = t.FireInsuranceCoverageTitle.Title,
                    title = t.Title,
                    rate = t.Rate,
                    isActive = t.IsActive,
                    ppfTitle = t.ProposalForm.Title
                })
                .ToList()
                .Select(t => new FireInsuranceCoverageMainGridResultVM 
                {
                    row = ++row,
                    id = t.id,
                    company = string.Join(",", t.company),
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    rate = t.rate,
                    title = t.title,
                    titleId = t.titleId,
                    ppfTitle = t.ppfTitle
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateFireInsuranceCoverageVM input)
        {
            createValidation(input);

            var foundItem = db.FireInsuranceCoverages.Include(t => t.FireInsuranceCoverageCompanies).Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.FireInsuranceCoverageCompanies != null)
                foreach (var com in foundItem.FireInsuranceCoverageCompanies)
                    db.Entry(com).State = EntityState.Deleted;

            foundItem.FireInsuranceCoverageTitleId = input.titleId.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.ProposalFormId = input.pfId.Value;
            foundItem.Rate = input.rate.Value;
            foundItem.Title = input.title;

            if (input.cIds != null && input.cIds.Count > 0)
                foreach (var cid in input.cIds)
                    db.Entry(new FireInsuranceCoverageCompany() { CompanyId = cid, FireInsuranceCoverageId = foundItem.Id }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
