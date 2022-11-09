using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Models.DB;
using Oje.Section.GlobalForms.Models.View;
using Oje.Section.GlobalForms.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.GlobalForms.Services
{
    public class GeneralFormStatusGridColumnService : IGeneralFormStatusGridColumnService
    {
        readonly GeneralFormDBContext db = null;
        public GeneralFormStatusGridColumnService
            (
                GeneralFormDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(GeneralFormStatusGridColumnCreateUpdateVM input)
        {
            createUpdateValidation(input);

            db.Entry(new GeneralFormStatusGridColumn()
            {
                GeneralFilledFormKeyId = input.kid.Value,
                GeneralFormId = input.fid.Value,
                GeneralFormStatusId = input.sid.Value,
                Order = input.order.ToIntReturnZiro()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(GeneralFormStatusGridColumnCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.fid.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (input.sid.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Status);
            if (input.kid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (!db.GeneralFilledFormValues.Any(t => t.GeneralFilledForm.GeneralFormId == input.fid && t.GeneralFilledFormKeyId == input.kid))
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (db.GeneralFormStatusGridColumns.Any(t => t.GeneralFormId == input.fid && t.GeneralFormStatusId == input.sid && t.GeneralFilledFormKeyId == input.kid))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
            if (!db.GeneralFormStatuses.Any(t => t.GeneralFormId == input.fid && t.Id == input.sid))
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.GeneralFormStatusGridColumns.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.GeneralFormStatusGridColumns
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    fid = t.GeneralFormId,
                    fid_Title = t.GeneralForm.Title,
                    sid = t.GeneralFormStatusId,
                    sid_Title = t.GeneralFormStatus.Title,
                    kid = t.GeneralFilledFormKeyId,
                    kid_Title = t.GeneralFilledFormKey.Title + "(" + t.GeneralFilledFormKey.Key + ")",
                    order = t.Order
                })
                .FirstOrDefault();
        }

        public GridResultVM<GeneralFormStatusGridColumnMainGridResult> GetList(GeneralFormStatusGridColumnMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.GeneralFormStatusGridColumns.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.fgTitle))
                quiryResult = quiryResult.Where(t => t.GeneralForm.Title.Contains(searchInput.fgTitle));
            if (!string.IsNullOrEmpty(searchInput.sTitle))
                quiryResult = quiryResult.Where(t => t.GeneralFormStatus.Title.Contains(searchInput.sTitle));
            if (!string.IsNullOrEmpty(searchInput.kTitle))
                quiryResult = quiryResult.Where(t => t.GeneralFilledFormKey.Key.Contains(searchInput.kTitle));

            int row = searchInput.skip;

            return new GridResultVM<GeneralFormStatusGridColumnMainGridResult>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.GeneralFormId)
                .ThenBy(t => t.GeneralFormStatus.Order)
                .ThenBy(t => t.Order)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    fgTitle = t.GeneralForm.Title,
                    sTitle = t.GeneralFormStatus.Title,
                    kTitle = t.GeneralFilledFormKey.Key
                })
                .ToList()
                .Select(t => new GeneralFormStatusGridColumnMainGridResult
                {
                    row = ++row,
                    id = t.id,
                    fgTitle = t.fgTitle,
                    kTitle = t.kTitle,
                    sTitle = t.sTitle
                })
                .ToList()
            };
        }

        public ApiResult Update(GeneralFormStatusGridColumnCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.GeneralFormStatusGridColumns.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.GeneralFilledFormKeyId = input.kid.Value;
            foundItem.GeneralFormId = input.fid.Value;
            foundItem.GeneralFormStatusId = input.sid.Value;
            foundItem.Order = input.order.ToIntReturnZiro();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public List<GeneralFormStatusGridColumn> GetListBy(long formId, long statusId)
        {
            return db.GeneralFormStatusGridColumns
                .Include(t => t.GeneralFilledFormKey)
                .Where(t => t.GeneralFormId == formId && t.GeneralFormStatusId == statusId)
                .OrderBy(t => t.Order)
                .AsNoTracking()
                .ToList();
        }
    }
}
