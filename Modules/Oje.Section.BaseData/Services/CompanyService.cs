using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.BaseData.Models.View;
using Oje.Section.BaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.BaseData.Services.EContext;
using Oje.FileService.Interfaces;

namespace Oje.Section.BaseData.Services
{
    public class CompanyService : Interfaces.ICompanyService
    {
        readonly IUploadedFileService uploadedFileService = null;
        readonly BaseDataDBContext db = null;
        public CompanyService(
                IUploadedFileService uploadedFileService,
                BaseDataDBContext db
            )
        {
            this.uploadedFileService = uploadedFileService;
            this.db = db;
        }

        public ApiResult Create(CreateUpdateCompanyVM input, long? userId)
        {
            CreateValidation(input);

            var newItem = new Company()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Name = input.name,
                Title = input.title,
                Pic = " "
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.minPic != null && input.minPic.Length > 0)
            {
                newItem.Pic = uploadedFileService.UploadNewFile(FileType.CompanyLogo, input.minPic, userId, null, newItem.Id, ".png,.jpg,.jpeg", false);
                db.SaveChanges();
            }


            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        void CreateValidation(CreateUpdateCompanyVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.name))
                throw BException.GenerateNewException(BMessages.Please_Enter_Name, ApiResultErrorCode.ValidationError);
            if (input.id.ToIntReturnZiro() <= 0 && (input.minPic == null || input.minPic.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_Image, ApiResultErrorCode.ValidationError);
            if (db.Companies.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title, ApiResultErrorCode.ValidationError);
            if (db.Companies.Any(t => t.Name == input.name && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Name, ApiResultErrorCode.ValidationError);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.Companies.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public object GetById(int? id)
        {
            return db.Companies
                .Where(t => t.Id == id)
                .AsNoTracking()
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    name = t.Name,
                    isActive = t.IsActive,
                    minPic_address = GlobalConfig.FileAccessHandlerUrl + t.Pic
                })
                .FirstOrDefault();
        }

        public GridResultVM<CompanyMainGridResultVM> GetList(CompanyMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new CompanyMainGrid();

            var qureResult = db.Companies.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (!string.IsNullOrEmpty(searchInput.name))
                qureResult = qureResult.Where(t => t.Name.Contains(searchInput.name));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive.Value);

            int row = searchInput.skip;

            return new GridResultVM<CompanyMainGridResultVM>
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take).AsNoTracking()
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    name = t.Name,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new CompanyMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    name = t.name,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateCompanyVM input, long? userId)
        {
            CreateValidation(input);

            var foundItem = db.Companies.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Name = input.name;
            foundItem.Title = input.title;

            db.SaveChanges();

            if (input.minPic != null && input.minPic.Length > 0)
            {
                foundItem.Pic = uploadedFileService.UploadNewFile(FileType.CompanyLogo, input.minPic, userId, null, foundItem.Id, ".png,.jpg,.jpeg", false);
                db.SaveChanges();
            }


            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.Companies.AsNoTracking().Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
