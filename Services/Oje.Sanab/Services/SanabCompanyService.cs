using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Sanab.Interfaces;
using Oje.Sanab.Models.DB;
using Oje.Sanab.Models.View;
using Oje.Sms.Services.EContext;

namespace Oje.Sanab.Services
{
    public class SanabCompanyService : ISanabCompanyService
    {
        readonly SanabDBContext db = null;
        readonly ICompanyService CompanyService = null;
        public SanabCompanyService
            (
                SanabDBContext db,
                ICompanyService CompanyService
            )
        {
            this.db = db;
            this.CompanyService = CompanyService;
        }

        public ApiResult Create(SanabCompanyCreateUpdateVM input)
        {
            createUpdateValidation(input);

            db.Entry(new SanabCompany()
            {
                Code = input.code,
                CompanyId = input.cid.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(SanabCompanyCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.cid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (!CompanyService.Exist(input.cid.ToIntReturnZiro()))
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (string.IsNullOrEmpty(input.code))
                throw BException.GenerateNewException(BMessages.Please_Enter_Code);
            if (input.code.Length > 30)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (db.SanabCompanies.Any(t => t.CompanyId == input.cid && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.SanabCompanies.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.SanabCompanies
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    code = t.Code,
                    cid = t.CompanyId
                })
                .FirstOrDefault();
        }

        public GridResultVM<SanabCompanyMainGridResultVM> GetList(SanabCompanyMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.SanabCompanies.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.company))
                quiryResult = quiryResult.Where(t => t.Company.Title.Contains(searchInput.company));
            if (!string.IsNullOrEmpty(searchInput.code))
                quiryResult = quiryResult.Where(t => t.Code.Contains(searchInput.code));

            int row = searchInput.skip;

            return new GridResultVM<SanabCompanyMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    company = t.Company.Title,
                    code = t.Code
                })
                .ToList()
                .Select(t => new SanabCompanyMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    company = t.company,
                    code = t.code
                })
                .ToList()
            };
        }

        public ApiResult Update(SanabCompanyCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.SanabCompanies.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Code = input.code;
            foundItem.CompanyId = input.cid.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public int? GetCompanyId(int? code)
        {
            string strCode = code + "";
            return db.SanabCompanies.Where(t => t.Code == strCode).Select(t => t.CompanyId).FirstOrDefault();
        }
    }
}
