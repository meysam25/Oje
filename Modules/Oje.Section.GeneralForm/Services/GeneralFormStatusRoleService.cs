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
    public class GeneralFormStatusRoleService : IGeneralFormStatusRoleService
    {
        readonly GeneralFormDBContext db = null;
        public GeneralFormStatusRoleService
            (
                GeneralFormDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(GeneralFormStatusRoleCreateUpdateVM input)
        {
            createUpdateValidation(input);

            db.Entry(new GeneralFormStatusRole()
            {
                GeneralFormId = input.fid.Value,
                GeneralFormStatusId = input.sid,
                RoleId = input.rid.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(GeneralFormStatusRoleCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.fid.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (input.rid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_One_Or_More_Role);
            if (input.sid.ToLongReturnZiro() > 0 && !db.GeneralFormStatuses.Any(t => t.GeneralFormId == input.fid.Value && t.Id == input.sid))
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (db.GeneralFormStatusRoles.Any(t => t.RoleId == input.rid && t.GeneralFormId == input.fid && t.GeneralFormStatusId == input.sid && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.GeneralFormStatusRoles.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.GeneralFormStatusRoles
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    fid = t.GeneralFormId,
                    fid_Title = t.GeneralForm.Title,
                    sid = t.GeneralFormStatusId,
                    sid_Title = t.GeneralFormStatus.Title,
                    rid = t.RoleId,
                    rid_Title = t.Role.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<GeneralFormStatusRoleMainGridResult> GetList(GeneralFormStatusRoleMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.GeneralFormStatusRoles.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.fTitle))
                quiryResult = quiryResult.Where(t => t.GeneralForm.Title.Contains(searchInput.fTitle));
            if (!string.IsNullOrEmpty(searchInput.sTitle))
                quiryResult = quiryResult.Where(t => t.GeneralFormStatus.Title.Contains(searchInput.sTitle));
            if (!string.IsNullOrEmpty(searchInput.rTItle))
                quiryResult = quiryResult.Where(t => t.Role.Title.Contains(searchInput.rTItle));

            int row = searchInput.skip;

            return new GridResultVM<GeneralFormStatusRoleMainGridResult>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.GeneralFormId)
                .ThenByDescending(t => t.Role)
                .ThenBy(t => t.GeneralFormStatus != null ? t.GeneralFormStatus.Order : 1)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    fTitle = t.GeneralForm.Title,
                    sTitle = t.GeneralFormStatusId > 0 ? t.GeneralFormStatus.Title : "",
                    rTItle = t.Role.Title
                })
                .ToList()
                .Select(t => new GeneralFormStatusRoleMainGridResult
                {
                    row = ++row,
                    id = t.id,
                    fTitle = t.fTitle,
                    rTItle = t.rTItle,
                    sTitle = t.sTitle
                })
                .ToList()
            };
        }

        public ApiResult Update(GeneralFormStatusRoleCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.GeneralFormStatusRoles.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.GeneralFormId = input.fid.Value;
            foundItem.GeneralFormStatusId = input.sid;
            foundItem.RoleId = input.rid.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public List<GeneralFormStatusRole> GetByRoles(List<string> roles)
        {
            roles = roles ?? new();
            return db.GeneralFormStatusRoles.Where(t => roles.Contains(t.Role.Name)).AsNoTracking().ToList();
        }
    }
}
