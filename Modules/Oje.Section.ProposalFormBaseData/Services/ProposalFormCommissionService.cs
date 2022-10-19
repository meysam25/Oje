using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Models.DB;
using Oje.Section.ProposalFormBaseData.Models.View;
using Oje.Section.ProposalFormBaseData.Services.EContext;
using System.Diagnostics;
using System.Linq;

namespace Oje.Section.ProposalFormBaseData.Services
{
    public class ProposalFormCommissionService : IProposalFormCommissionService
    {
        readonly ProposalFormBaseDataDBContext db = null;
        readonly IProposalFormService ProposalFormService = null;

        public ProposalFormCommissionService
            (
                ProposalFormBaseDataDBContext db,
                IProposalFormService ProposalFormService
            )
        {
            this.db = db;
            this.ProposalFormService = ProposalFormService;
        }

        public ApiResult Create(ProposalFormCommissionCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new ProposalFormCommission()
            {
                DefPrice = input.tPrice.Value - input.fPrice.Value,
                FromPrice = input.fPrice.Value,
                ToPrice = input.tPrice.Value,
                ProposalFormId = input.ppfId.Value,
                Rate = input.rate.Value,
                Title = input.title
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(ProposalFormCommissionCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.ppfId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (!ProposalFormService.Exist(input.ppfId.ToIntReturnZiro(), siteSettingId))
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.fPrice.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Price);
            if (input.tPrice.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Price);
            if (input.fPrice.ToLongReturnZiro() >= input.tPrice)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            if (input.rate == null || input.rate < 0)
                throw BException.GenerateNewException(BMessages.Invalid_Rate);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.ProposalFormCommissions.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.ProposalFormCommissions
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    ppfId = t.ProposalFormId,
                    ppfId_Title = t.ProposalForm.Title,
                    title = t.Title,
                    fPrice = t.FromPrice,
                    tPrice = t.ToPrice,
                    rate = t.Rate
                })
                .FirstOrDefault();
        }

        public GridResultVM<ProposalFormCommissionMainGridResultVM> GetList(ProposalFormCommissionMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.ProposalFormCommissions.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.ppf))
                quiryResult = quiryResult.Where(t => t.ProposalForm.Title.Contains(searchInput.ppf));
            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.fPrice.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.FromPrice == searchInput.fPrice);
            if (searchInput.tPrice.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.ToPrice == searchInput.tPrice);
            if (searchInput.rate != null && searchInput.rate != 0)
                quiryResult = quiryResult.Where(t => t.Rate == searchInput.rate);

            int row = searchInput.skip;

            return new GridResultVM<ProposalFormCommissionMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.ProposalFormId)
                .ThenBy(t => t.FromPrice)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new 
                {
                    id = t.Id,
                    ppf = t.ProposalForm.Title,
                    title = t.Title,
                    fPrice = t.FromPrice,
                    tPrice = t.ToPrice,
                    rate = t.Rate
                })
                .ToList()
                .Select(t => new ProposalFormCommissionMainGridResultVM 
                {
                    row = ++row,
                    id = t.id,
                    fPrice = t.fPrice.ToString("###,###"),
                    ppf = t.ppf,
                    rate = t.rate.ToString(),
                    title = t.title,
                    tPrice = t.tPrice.ToString("###,###")
                })
                .ToList()
            };
        }

        public ApiResult Update(ProposalFormCommissionCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.ProposalFormCommissions.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.DefPrice = input.tPrice.Value - input.fPrice.Value;
            foundItem.FromPrice = input.fPrice.Value;
            foundItem.ToPrice = input.tPrice.Value;
            foundItem.ProposalFormId = input.ppfId.Value;
            foundItem.Rate = input.rate.Value;
            foundItem.Title = input.title;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
