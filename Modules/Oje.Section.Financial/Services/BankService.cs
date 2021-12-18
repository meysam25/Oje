using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Financial.Models.View;
using Oje.Section.Financial.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.Financial.Services.EContext;
using Oje.Section.Financial.Interfaces;

namespace Oje.Section.Financial.Services
{
    public class BankService : IBankService
    {
        readonly IUploadedFileService uploadedFileService = null;
        readonly FinancialDBContext db = null;
        public BankService(
                IUploadedFileService uploadedFileService,
                FinancialDBContext db
            )
        {
            this.uploadedFileService = uploadedFileService;
            this.db = db;
        }

        public ApiResult Create(CreateUpdateBankVM input, long? userId)
        {
            CreateValidation(input);

            var newItem = new Bank()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Title = input.title,
                Pic = " "
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.minPic != null && input.minPic.Length > 0)
            {
                newItem.Pic = uploadedFileService.UploadNewFile(FileType.BankLogo, input.minPic, userId, null, newItem.Id, ".png,.jpg,.jpeg", false);
                db.SaveChanges();
            }


            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        void CreateValidation(CreateUpdateBankVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title, ApiResultErrorCode.ValidationError);
            if (input.id.ToIntReturnZiro() <= 0 && (input.minPic == null || input.minPic.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_Image, ApiResultErrorCode.ValidationError);
            if (db.Banks.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title, ApiResultErrorCode.ValidationError);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.Banks.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public object GetById(int? id)
        {
            return db.Banks
                .Where(t => t.Id == id)
                .AsNoTracking()
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                    minPic_address = GlobalConfig.FileAccessHandlerUrl + t.Pic
                })
                .FirstOrDefault();
        }

        public GridResultVM<BankMainGridResultVM> GetList(BankMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new BankMainGrid();

            var qureResult = db.Banks.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive.Value);

            int row = searchInput.skip;

            return new GridResultVM<BankMainGridResultVM>
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take).AsNoTracking()
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new BankMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateBankVM input, long? userId)
        {
            CreateValidation(input);

            var foundItem = db.Banks.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Title = input.title;

            db.SaveChanges();

            if (input.minPic != null && input.minPic.Length > 0)
            {
                foundItem.Pic = uploadedFileService.UploadNewFile(FileType.BankLogo, input.minPic, userId, null, foundItem.Id, ".png,.jpg,.jpeg", false);
                db.SaveChanges();
            }


            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.Banks.AsNoTracking().Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
