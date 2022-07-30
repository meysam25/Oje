using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Services.EContext;
using Oje.PaymentService.Models.DB;
using Oje.PaymentService.Models.View;
using Oje.FileService.Interfaces;

namespace Oje.PaymentService.Services
{
    public class BankService : IBankService
    {
        readonly IUploadedFileService UploadedFileService = null;
        readonly PaymentDBContext db = null;
        public BankService(
                IUploadedFileService UploadedFileService,
                PaymentDBContext db
            )
        {
            this.UploadedFileService = UploadedFileService;
            this.db = db;
        }

        public ApiResult Create(BankCreateUpdateVM input, long? userId)
        {
            CreateValidation(input);

            var newItem = new Bank()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Title = input.title,
                Pic = " ",
                BankCode = input.code
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.minPic != null && input.minPic.Length > 0)
            {
                newItem.Pic = UploadedFileService.UploadNewFile(FileType.BankLogo, input.minPic, userId, null, newItem.Id, ".png,.jpg,.jpeg", false);
                db.SaveChanges();
            }


            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        void CreateValidation(BankCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title, ApiResultErrorCode.ValidationError);
            if (input.id.ToIntReturnZiro() <= 0 && (input.minPic == null || input.minPic.Length == 0))
                throw BException.GenerateNewException(BMessages.Please_Select_Image, ApiResultErrorCode.ValidationError);
            if (db.Banks.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title, ApiResultErrorCode.ValidationError);
            if (input.code != null && db.Banks.Any(t => t.BankCode == input.code && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
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
                    minPic_address = GlobalConfig.FileAccessHandlerUrl + t.Pic,
                    code = t.BankCode
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
            if (searchInput.code != null)
                qureResult = qureResult.Where(t => t.BankCode == searchInput.code);

            int row = searchInput.skip;

            return new GridResultVM<BankMainGridResultVM>
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take).AsNoTracking()
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                    code = t.BankCode
                })
                .ToList()
                .Select(t => new BankMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    code = t.code
                })
                .ToList()
            };
        }

        public ApiResult Update(BankCreateUpdateVM input, long? userId)
        {
            CreateValidation(input);

            var foundItem = db.Banks.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Title = input.title;
            foundItem.BankCode = input.code;

            db.SaveChanges();

            if (input.minPic != null && input.minPic.Length > 0)
            {
                foundItem.Pic = UploadedFileService.UploadNewFile(FileType.BankLogo, input.minPic, userId, null, foundItem.Id, ".png,.jpg,.jpeg", false);
                db.SaveChanges();
            }


            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.Banks.Where(t => t.IsActive == true).AsNoTracking().Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }

        public int? GetByCode(int? code)
        {
            return db.Banks.Where(t => t.BankCode == code).Select(t => t.Id).FirstOrDefault();
        }

        public Bank GetBy(int id)
        {
            return db.Banks.Where(t => t.IsActive == true && t.Id == id).FirstOrDefault();
        }
    }
}
