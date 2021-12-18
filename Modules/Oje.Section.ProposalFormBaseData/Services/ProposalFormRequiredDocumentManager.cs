using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.ProposalFormBaseData.Services.EContext;
using Oje.Section.ProposalFormBaseData.Models.View;

namespace Oje.Section.ProposalFormBaseData.Services
{
    public class ProposalFormRequiredDocumentService : IProposalFormRequiredDocumentService
    {
        readonly ProposalFormBaseDataDBContext db = null;
        readonly IUploadedFileService UploadedFileService = null;
        public ProposalFormRequiredDocumentService(ProposalFormBaseDataDBContext db, IUploadedFileService UploadedFileService)
        {
            this.db = db;
            this.UploadedFileService = UploadedFileService;
        }

        public ApiResult Create(CreateUpdateProposalFormRequiredDocumentVM input, long? userId)
        {
            CreateValidation(input, userId);

            var newItem = new ProposalFormRequiredDocument()
            {
                CreateDate = DateTime.Now,
                CreateUserId = userId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                IsRequired = input.isRequired.ToBooleanReturnFalse(),
                ProposalFormRequiredDocumentTypeId = input.typeId.Value,
                Title = input.title
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.downloadFile != null && input.downloadFile.Length > 0)
            {
                newItem.DownloadFile = UploadedFileService.UploadNewFile(FileType.RequiredDocument, input.downloadFile, userId, null, newItem.Id, ".png,.jpg,.jpeg", false);
                db.SaveChanges();
            }

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        private void CreateValidation(CreateUpdateProposalFormRequiredDocumentVM input, long? userId)
        {
            if (userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First, ApiResultErrorCode.NeedLoginFist);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (input.typeId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Type, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title, ApiResultErrorCode.ValidationError);
            if (db.ProposalFormRequiredDocuments.Any(t => t.ProposalFormRequiredDocumentTypeId == input.typeId && t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title, ApiResultErrorCode.ValidationError);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.ProposalFormRequiredDocuments.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.ValidationError);
            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public CreateUpdateProposalFormRequiredDocumentVM GetById(int? id)
        {
            return db.ProposalFormRequiredDocuments.Where(t => t.Id == id).Select(t => new CreateUpdateProposalFormRequiredDocumentVM
            {
                id = t.Id,
                downloadFile_address = !string.IsNullOrEmpty(t.DownloadFile) ? GlobalConfig.FileAccessHandlerUrl + t.DownloadFile: "",
                isActive = t.IsActive,
                isRequired = t.IsRequired,
                title = t.Title,
                typeId = t.ProposalFormRequiredDocumentTypeId,
                typeId_Title = t.ProposalFormRequiredDocumentType.Title + "(" + t.ProposalFormRequiredDocumentType.ProposalForm.Title + ")"
            }).FirstOrDefault();
        }

        public GridResultVM<ProposalFormRequiredDocumentMainGridResultVM> GetList(ProposalFormRequiredDocumentMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new ProposalFormRequiredDocumentMainGrid();

            var qureResult = db.ProposalFormRequiredDocuments.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.type))
                qureResult = qureResult.Where(t => t.ProposalFormRequiredDocumentType.Title.Contains(searchInput.type));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.isRequired != null)
                qureResult = qureResult.Where(t => t.IsRequired == searchInput.isRequired);

            var row = searchInput.skip;

            return new GridResultVM<ProposalFormRequiredDocumentMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
            .Select(t => new 
            {
                id = t.Id,
                type = t.ProposalFormRequiredDocumentType.Title,
                title = t.Title,
                isActive = t.IsActive,
                isRequired = t.IsRequired
            })
            .ToList()
            .Select(t => new ProposalFormRequiredDocumentMainGridResultVM 
            {
                row = ++row,
                id = t.id,
                type = t.type,
                title = t.title,
                isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                isRequired = t.isRequired == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName()
            })
            .ToList()
            };
        }

        public ApiResult Update(CreateUpdateProposalFormRequiredDocumentVM input, long? userId)
        {
            CreateValidation(input, userId);

            var foundItem = db.ProposalFormRequiredDocuments.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);


            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.IsRequired = input.isRequired.ToBooleanReturnFalse();
            foundItem.ProposalFormRequiredDocumentTypeId = input.typeId.Value;
            foundItem.Title = input.title;
            foundItem.UpdateDate = DateTime.Now;
            foundItem.UpdateUserId = userId;

            db.SaveChanges();

            if (input.downloadFile != null && input.downloadFile.Length > 0)
            {
                foundItem.DownloadFile = UploadedFileService.UploadNewFile(FileType.RequiredDocument, input.downloadFile, userId, null, foundItem.Id, ".png,.jpg,.jpeg", false);
                db.SaveChanges();
            }

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }
    }
}
