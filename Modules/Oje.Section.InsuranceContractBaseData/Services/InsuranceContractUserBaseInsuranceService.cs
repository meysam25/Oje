using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.DB;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Oje.Section.InsuranceContractBaseData.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class InsuranceContractUserBaseInsuranceService : IInsuranceContractUserBaseInsuranceService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        public InsuranceContractUserBaseInsuranceService(InsuranceContractBaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(InsuranceContractUserBaseInsuranceCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new InsuranceContractUserBaseInsurance()
            {
                Code = input.code,
                SiteSettingId = siteSettingId.Value,
                Title = input.title
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(InsuranceContractUserBaseInsuranceCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (string.IsNullOrEmpty(input.code))
                throw BException.GenerateNewException(BMessages.Please_Enter_Code);
            if (db.InsuranceContractUserBaseInsurances.Any(t => t.SiteSettingId == siteSettingId && t.Id != input.id && t.Code == input.code))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);

        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.InsuranceContractUserBaseInsurances.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.InsuranceContractUserBaseInsurances.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).Select(t => new
            {
                id = t.Id,
                title = t.Title,
                code = t.Code
            }).FirstOrDefault();
        }

        public GridResultVM<InsuranceContractUserBaseInsuranceMainGridResultVM> GetList(InsuranceContractUserBaseInsuranceMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new InsuranceContractUserBaseInsuranceMainGrid();

            var quiryResult = db.InsuranceContractUserBaseInsurances.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.code))
                quiryResult = quiryResult.Where(t => t.Code.Contains(searchInput.code));

            int row = searchInput.skip;

            return new GridResultVM<InsuranceContractUserBaseInsuranceMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    code = t.Code,
                    title = t.Title,
                })
                .ToList()
                .Select(t => new InsuranceContractUserBaseInsuranceMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    code = t.code,
                    title = t.title
                })
                .ToList()
            };
        }

        public ApiResult Update(InsuranceContractUserBaseInsuranceCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.InsuranceContractUserBaseInsurances.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Code = input.code;
            foundItem.Title = input.title;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };
            result.AddRange(db.InsuranceContractUserBaseInsurances.Where(t => t.SiteSettingId == siteSettingId).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }

        public bool Exist(int? siteSettingId, int? id)
        {
            return db.InsuranceContractUserBaseInsurances.Any(t => t.SiteSettingId == siteSettingId && t.Id == id);
        }

        public int? GetByCode(int? siteSettingId, string code)
        {
            return db.InsuranceContractUserBaseInsurances.Where(t => t.SiteSettingId == siteSettingId && t.Code == code).Select(t => t.Id).FirstOrDefault();
        }
    }
}
