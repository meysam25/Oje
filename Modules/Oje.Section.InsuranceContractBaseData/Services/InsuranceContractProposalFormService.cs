using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.DB;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Oje.Section.InsuranceContractBaseData.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class InsuranceContractProposalFormService : IInsuranceContractProposalFormService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        public InsuranceContractProposalFormService(InsuranceContractBaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(InsuranceContractProposalFormCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new InsuranceContractProposalForm()
            {
                CreateDate = DateTime.Now,
                Description = input.description,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                JsonConfig = input.jConfig,
                Name = input.name,
                SiteSettingId = siteSettingId.Value,
                TermTemplate = input.termsT,
                Title = input.title
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(InsuranceContractProposalFormCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (string.IsNullOrEmpty(input.name))
                throw BException.GenerateNewException(BMessages.Please_Enter_Name);
            if (input.name.Length > 100)
                throw BException.GenerateNewException(BMessages.Name_Can_Not_Be_More_Then_100_chars);
            if (string.IsNullOrEmpty(input.jConfig))
                throw BException.GenerateNewException(BMessages.Please_Enter_Json_Config);
            if (string.IsNullOrEmpty(input.termsT))
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input.termsT.Length > 4000)
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.InsuranceContractProposalForms.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public InsuranceContractProposalFormCreateUpdateVM GetById(int? id, int? siteSettingId)
        {
            return db.InsuranceContractProposalForms
                .Where(t => t.Id == id && t.SiteSettingId == siteSettingId)
                .OrderByDescending(t => t.Id)
                .Select(t => new
                {
                    t.Id,
                    t.Description,
                    t.IsActive,
                    t.JsonConfig,
                    t.Name,
                    t.TermTemplate,
                    t.Title
                })
                .Take(1)
                .Select(t => new InsuranceContractProposalFormCreateUpdateVM()
                {
                    id = t.Id,
                    description = t.Description,
                    isActive = t.IsActive,
                    jConfig = t.JsonConfig,
                    name = t.Name,
                    termsT = t.TermTemplate,
                    title = t.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<InsuranceContractProposalFormMainGridResultVM> GetList(InsuranceContractProposalFormMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new InsuranceContractProposalFormMainGrid();

            var qureResult = db.InsuranceContractProposalForms.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<InsuranceContractProposalFormMainGridResultVM>()
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
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new InsuranceContractProposalFormMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(InsuranceContractProposalFormCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.InsuranceContractProposalForms.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.UpdateDate = DateTime.Now;
            foundItem.Description = input.description;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.JsonConfig = input.jConfig;
            foundItem.Name = input.name;
            foundItem.TermTemplate = input.termsT;
            foundItem.Title = input.title;

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


            var qureResult = db.InsuranceContractProposalForms.OrderByDescending(t => t.Id).Where(t => t.SiteSettingId == siteSettingId);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public bool Exist(int id, int? siteSettingId)
        {
            return db.InsuranceContractProposalForms.Any(t => t.SiteSettingId == siteSettingId && t.Id == id);
        }
    }
}
