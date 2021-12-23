using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Models.DB;
using Oje.Section.ProposalFormBaseData.Models.View;
using Oje.Section.ProposalFormBaseData.Services.EContext;
using System.Linq;

namespace Oje.Section.ProposalFormBaseData.Services
{
    public class PaymentMethodFileService : IPaymentMethodFileService
    {
        readonly ProposalFormBaseDataDBContext db = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IUploadedFileService uploadedFileService = null;
        public PaymentMethodFileService(
            ProposalFormBaseDataDBContext db,
            AccountService.Interfaces.ISiteSettingService SiteSettingService,
            IUploadedFileService uploadedFileService
            )
        {
            this.db = db;
            this.SiteSettingService = SiteSettingService;
            this.uploadedFileService = uploadedFileService;
        }

        public ApiResult Create(CreateUpdatePaymentMethodFileVM input)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            createUpdateValidation(input, siteSettingId);

            var newItem = new PaymentMethodFile()
            {
                IsRequired = input.isRequired.ToBooleanReturnFalse(),
                PaymentMethodId = input.payId.ToIntReturnZiro(),
                Title = input.title
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.minPic != null && input.minPic.Length > 0)
                newItem.DownloadImageUrl = uploadedFileService.UploadNewFile(FileType.DebitSellRequredDocument, input.minPic, null, siteSettingId, newItem.Id, ".png,.jpg,.jpeg,.pdf,.doc", false);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(CreateUpdatePaymentMethodFileVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.payId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_PaymentMethod);
            if (!db.PaymentMethods.Any(t => t.Id == input.payId && t.SiteSettingId == siteSettingId))
                throw BException.GenerateNewException(BMessages.Not_Found);
        }

        public ApiResult Delete(int? id)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            var deleteItem = db.PaymentMethodFiles.Where(t => t.Id == id && t.PaymentMethod.SiteSettingId == siteSettingId).FirstOrDefault();
            if (deleteItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(deleteItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;

            return db.PaymentMethodFiles
                .Where(t => t.Id == id && t.PaymentMethod.SiteSettingId == siteSettingId)
                .OrderByDescending(t => t.Id)
                .Take(1)
                .Select(t => new 
                {
                    id = t.Id,
                    title = t.Title,
                    payId = t.PaymentMethodId,
                    isRequired = t.IsRequired,
                    t.DownloadImageUrl
                })
                .ToList()
                .Select(t => new
                {
                    id = t.id,
                    title = t.title,
                    payId = t.payId,
                    isRequired = t.isRequired,
                    minPic_address = GlobalConfig.FileAccessHandlerUrl + t.DownloadImageUrl
                })
                .FirstOrDefault();
        }

        public GridResultVM<PaymentMethodFileMainGridResult> GetList(PaymentMethodFileMainGrid searchInput)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            if (searchInput == null)
                searchInput = new PaymentMethodFileMainGrid();

            var qureResult = db.PaymentMethodFiles.Where(t => t.PaymentMethod.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.payId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.PaymentMethodId == searchInput.payId);
            if (searchInput.isRequred != null)
                qureResult = qureResult.Where(t => t.IsRequired == searchInput.isRequred);

            int row = searchInput.skip;

            return new GridResultVM<PaymentMethodFileMainGridResult>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    payId = t.PaymentMethod.Title,
                    isRequred = t.IsRequired
                })
                .ToList()
                .Select(t => new PaymentMethodFileMainGridResult
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    payId = t.payId,
                    isRequred = t.isRequred == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdatePaymentMethodFileVM input)
        {
            int? siteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            createUpdateValidation(input, siteSettingId);

            var editItem = db.PaymentMethodFiles.Where(t => t.Id == input.id && t.PaymentMethod.SiteSettingId == siteSettingId).FirstOrDefault();
            if (editItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            editItem.IsRequired = input.isRequired.ToBooleanReturnFalse();
            editItem.PaymentMethodId = input.payId.ToIntReturnZiro();
            editItem.Title = input.title;
            if (input.minPic != null && input.minPic.Length > 0)
                editItem.DownloadImageUrl = uploadedFileService.UploadNewFile(FileType.DebitSellRequredDocument, input.minPic, null, siteSettingId, editItem.Id, ".png,.jpg,.jpeg,.pdf,.doc", false);

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
