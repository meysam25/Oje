using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oje.Section.Security.Interfaces;
using Oje.Section.Security.Services.EContext;
using Oje.Section.Security.Models.View;
using Oje.Section.Security.Models.DB;

namespace Oje.Section.Security.Services
{
    public class FileAccessRoleManager : IFileAccessRoleManager
    {
        readonly SecurityDBContext db = null;
        public FileAccessRoleManager(SecurityDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateFileAccessRoleVM input)
        {
            CreateValidation(input);

            db.Entry(new FileAccessRole()
            {
                RoleId = input.roleId.Value,
                FileType = input.fType.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateFileAccessRoleVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.fType.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_File_Type);
            if (input.roleId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_One_Or_More_Role);

            if (db.FileAccessRoles.Any(t => t.FileType == input.fType && t.RoleId == input.roleId && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.FileAccessRoles.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.FileAccessRoles.Where(t => t.Id == id).AsNoTracking()
                .Select(t => new
                {
                    id = t.Id,
                    fType = t.FileType,
                    roleId = t.RoleId
                })
                .ToList()
                .Select(t => new 
                {
                    id = t.id,
                    fType = t.fType.ToString(),
                    roleId = t.roleId
                })
                .FirstOrDefault();
        }

        public GridResultVM<FileAccessRoleMainGridResultVM> GetList(FileAccessRoleMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new FileAccessRoleMainGrid();

            var qureResult = db.FileAccessRoles.AsQueryable();

            if (searchInput.role.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.RoleId == searchInput.role);
            if (searchInput.fType != null)
                qureResult = qureResult.Where(t => t.FileType == searchInput.fType);

            var row = searchInput.skip;

            return new GridResultVM<FileAccessRoleMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take).AsNoTracking()
                .Select(t => new
                {
                    id = t.Id,
                    role = t.Role.Title,
                    fType = t.FileType,
                    fCount = db.UploadedFiles.Where(tt => tt.FileType == t.FileType).Count()
                })
                .ToList()
                .Select(t => new FileAccessRoleMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    role = t.role,
                    fType = t.fType.GetAttribute<DisplayAttribute>()?.Name + "(" + t.fCount + " فایل)"
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateFileAccessRoleVM input)
        {
            CreateValidation(input);

            var foundItem = db.FileAccessRoles.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.RoleId = input.roleId.Value;
            foundItem.FileType = input.fType.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
