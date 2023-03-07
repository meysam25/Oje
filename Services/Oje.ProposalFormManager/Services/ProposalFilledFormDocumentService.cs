using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Models.View;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFilledFormDocumentService : IProposalFilledFormDocumentService
    {
        readonly ProposalFormDBContext db = null;
        readonly IProposalFilledFormAdminBaseQueryService ProposalFilledFormAdminBaseQueryService = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IProposalFilledFormUseService ProposalFilledFormUseService = null;
        readonly IUserNotifierService UserNotifierService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public ProposalFilledFormDocumentService(
            ProposalFormDBContext db,
            IProposalFilledFormAdminBaseQueryService ProposalFilledFormAdminBaseQueryService,
            IUploadedFileService UploadedFileService,
            IProposalFilledFormUseService ProposalFilledFormUseService,
            IUserNotifierService UserNotifierService,
            IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.ProposalFilledFormAdminBaseQueryService = ProposalFilledFormAdminBaseQueryService;
            this.UploadedFileService = UploadedFileService;
            this.ProposalFilledFormUseService = ProposalFilledFormUseService;
            this.UserNotifierService = UserNotifierService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(ProposalFilledFormDocumentCreateUpdateVM input, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            createUpdateValidation(input, siteSettingId, userId);

            var foundId = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites).Where(t => t.Id == input.pKey).Select(t => t.Id).FirstOrDefault();
            if (foundId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var newDocument = new ProposalFilledFormDocument()
            {
                BankId = input.bankId,
                CashDate = input.cashDate.ConvertPersianNumberToEnglishNumber().ToEnDate(),
                Code = input.code,
                CreateDate = DateTime.Now,
                Description = input.description,
                Price = input.price.ToLongReturnZiro(),
                ProposalFilledFormId = foundId,
                SiteSettingId = siteSettingId.ToIntReturnZiro(),
                TargetDate = input.arriveDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value,
                Type = input.type.Value
            };

            db.Entry(newDocument).State = EntityState.Added;
            db.SaveChanges();


            if (input.mainFile != null && input.mainFile.Length > 0)
                newDocument.MainFileSrc = UploadedFileService.UploadNewFile(FileType.ProposalFilledForm, input.mainFile, userId, siteSettingId, foundId, ".jpg,.png,.jpeg,.pdf,.doc,.docx", true, foundId + "_" + newDocument.Id);

            newDocument.FilledSignature();
            db.SaveChanges();

            UserNotifierService.Notify(userId, UserNotificationType.ProposalFilledFormNewFinancialDocuemnt, ProposalFilledFormUseService.GetProposalFilledFormUserIds(input.pKey.ToLongReturnZiro()), input.pKey, "", siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + input.pKey);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult Update(ProposalFilledFormDocumentCreateUpdateVM input, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            createUpdateValidation(input, siteSettingId, userId);

            var foundId = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites).Where(t => t.Id == input.pKey).Select(t => t.Id).FirstOrDefault();
            if (foundId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var foundItem = db.ProposalFilledFormDocuments.Where(t => t.Id == input.id && t.ProposalFilledFormId == foundId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            if (!foundItem.IsSignature())
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

            foundItem.BankId = input.bankId;
            foundItem.CashDate = input.cashDate.ConvertPersianNumberToEnglishNumber().ToEnDate();
            foundItem.Code = input.code;
            foundItem.Description = input.description;
            foundItem.Price = input.price.ToLongReturnZiro();
            foundItem.TargetDate = input.arriveDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
            foundItem.Type = input.type.Value;
            foundItem.FilledSignature();

            if (input.mainFile != null && input.mainFile.Length > 0)
                foundItem.MainFileSrc = UploadedFileService.UploadNewFile(FileType.ProposalFilledForm, input.mainFile, userId, siteSettingId, foundId, ".jpg,.png,.jpeg,.pdf,.doc,.docx", true, foundId + "_" + foundItem.Id);
            db.SaveChanges();

            UserNotifierService.Notify(userId, UserNotificationType.ProposalFilledFormFinancialDocumentEdited, ProposalFilledFormUseService.GetProposalFilledFormUserIds(input.pKey.ToLongReturnZiro()), input.pKey, "", siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + input.pKey);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(ProposalFilledFormDocumentCreateUpdateVM input, int? siteSettingId, long? userId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (input.pKey.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (input.price.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Price);
            if (string.IsNullOrEmpty(input.arriveDate))
                throw BException.GenerateNewException(BMessages.Invalid_ArriveDate);
            if (input.arriveDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_ArriveDate);
            if (!string.IsNullOrEmpty(input.cashDate) && input.cashDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_CashDate);
            if (!string.IsNullOrEmpty(input.description) && input.description.Length > 3000)
                throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
            if (!string.IsNullOrEmpty(input.code) && input.code.Length > 50)
                throw BException.GenerateNewException(BMessages.Code_Can_Not_Be_More_Then_50_chars);
        }

        public void CreateChequeArr(long proposalFilledFormId, long proposalFilledFormPrice, int? siteSettingId, List<PaymentMethodDetailesCheckVM> checkArr, IFormCollection form)
        {
            if (proposalFilledFormId > 0 && proposalFilledFormPrice > 0 && siteSettingId.ToLongReturnZiro() > 0 && checkArr != null && checkArr.Count > 0 && form != null)
            {
                for (var i = 0; i < checkArr.Count; i++)
                {
                    var check = checkArr[i];
                    string currCheckNumber = form.Keys.Any(t => t == ("check[" + i + "].checkNumber")) ? form.GetStringIfExist("check[" + i + "].checkNumber") : "";
                    int bankId = (form.Keys.Any(t => t == ("check[" + i + "].bankId")) ? form.GetStringIfExist("check[" + i + "].bankId") : "0").ToIntReturnZiro();
                    if (string.IsNullOrEmpty(currCheckNumber))
                        throw BException.GenerateNewException(String.Format(BMessages.Please_Enter_CheckNumber_RowX.GetEnumDisplayName(), i));
                    if (currCheckNumber.Length > 50)
                        throw BException.GenerateNewException(BMessages.CheckNumber_CanNot_Be_Morethen_50_Chars);
                    if (bankId.ToIntReturnZiro() <= 0)
                        throw BException.GenerateNewException(String.Format(BMessages.Please_Select_Bank_RowX.GetEnumDisplayName(), i));

                    var newItem = new ProposalFilledFormDocument()
                    {
                        BankId = bankId,
                        Code = currCheckNumber,
                        CreateDate = DateTime.Now,
                        Price = checkArr[i].eachPaymentLong,
                        ProposalFilledFormId = proposalFilledFormId,
                        SiteSettingId = siteSettingId.Value,
                        TargetDate = checkArr[i].checkDateEn,
                        Type = ProposalFilledFormDocumentType.Cheque
                    };
                    newItem.FilledSignature();
                    db.Entry(newItem).State = EntityState.Added;
                }
                if (checkArr.Count > 0)
                    db.SaveChanges();
            }
        }

        public ApiResult Delete(long? id, long? proposalFilledFormId, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var foundId = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites).Where(t => t.Id == proposalFilledFormId).Select(t => t.Id).FirstOrDefault();
            if (foundId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var foundItem = db.ProposalFilledFormDocuments.Where(t => t.Id == id && t.ProposalFilledFormId == proposalFilledFormId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundItem.IsSignature())
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            UserNotifierService.Notify(userId, UserNotificationType.ProposalFilledFormFinancialDocuemntDeleted, ProposalFilledFormUseService.GetProposalFilledFormUserIds(proposalFilledFormId.ToLongReturnZiro()), proposalFilledFormId, "", siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(status) + "/PdfDetailesForAdmin?id=" + proposalFilledFormId);

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetBy(long? id, long? proposalFilledFormId, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var foundId = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, status, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites).Where(t => t.Id == proposalFilledFormId).Select(t => t.Id).FirstOrDefault();
            if (foundId <= 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return db.ProposalFilledFormDocuments
                .OrderByDescending(t => t.Id)
                .Where(t => t.Id == id && t.ProposalFilledFormId == proposalFilledFormId)
                .Select(t => new
                {
                    id = t.Id,
                    type = t.Type,
                    price = t.Price,
                    arriveDate = t.TargetDate,
                    code = t.Code,
                    bankId = t.BankId,
                    cashDate = t.CashDate,
                    description = t.Description,
                    src = t.MainFileSrc
                })
                .Take(1)
                .Select(t => new
                {
                    t.id,
                    type = (int)t.type,
                    price = t.price,
                    arriveDate = t.arriveDate != null ? t.arriveDate.ToFaDate() : "",
                    cashDate = t.cashDate != null ? t.cashDate.ToFaDate() : "",
                    t.code,
                    t.bankId,
                    mainFile_address = !string.IsNullOrEmpty(t.src) ? GlobalConfig.FileAccessHandlerUrl + t.src : "",
                    t.description
                })
                .FirstOrDefault();
        }

        public GridResultVM<ProposalFilledFormDocumentMainGridResultVM> GetList(ProposalFilledFormDocumentMainGrid searchInput, int? siteSettingId, long? userId, ProposalFilledFormStatus? status, List<ProposalFilledFormStatus> validStatus = null)
        {
            var endOfToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            var startOfToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var curDate = DateTime.Now;

            if (searchInput == null)
                searchInput = new ProposalFilledFormDocumentMainGrid();

            var foundId = ProposalFilledFormAdminBaseQueryService.getProposalFilledFormBaseQuery(siteSettingId, userId, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
                .Where(t => t.Id == searchInput.pKey)
                .Where(t => (status == null && validStatus != null && validStatus.Contains(t.Status) || t.Status == status))
                .Select(t => t.Id)
                .FirstOrDefault();
            var qureResult = db.ProposalFilledFormDocuments.Where(t => t.ProposalFilledFormId == foundId);

            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.type);
            if (searchInput.bankId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.BankId == searchInput.bankId);
            if (searchInput.price.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.Price == searchInput.price);
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.arriveDate) && searchInput.arriveDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.arriveDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.TargetDate != null && t.TargetDate.Value.Year == targetDate.Year && t.TargetDate.Value.Month == targetDate.Month && t.TargetDate.Value.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.cashDate) && searchInput.cashDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.cashDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CashDate != null && t.CashDate.Value.Year == targetDate.Year && t.CashDate.Value.Month == targetDate.Month && t.CashDate.Value.Day == targetDate.Day);
            }
            if (searchInput.status != null)
            {

                if (searchInput.status == CreditStatus.ItsTimePass)
                    qureResult = qureResult.Where(t => t.CashDate == null && t.TargetDate != null && t.TargetDate.Value < startOfToday);
                else if (searchInput.status == CreditStatus.NotYet)
                    qureResult = qureResult.Where(t => t.CashDate == null && t.TargetDate != null && t.TargetDate.Value >= endOfToday);
                else if (searchInput.status == CreditStatus.ItsTimeKnow)
                    qureResult = qureResult.Where(t => t.CashDate == null && t.TargetDate != null && t.TargetDate.Value.Year == curDate.Year && t.TargetDate.Value.Month == curDate.Month && t.TargetDate.Value.Day == curDate.Day);
                else if (searchInput.status == CreditStatus.GetMony)
                    qureResult = qureResult.Where(t => t.CashDate != null);
            }

            int row = searchInput.skip;

            return new GridResultVM<ProposalFilledFormDocumentMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    type = t.Type,
                    bankId = t.Bank.Title,
                    price = t.Price,
                    createDate = t.CreateDate,
                    arriveDate = t.TargetDate,
                    cashDate = t.CashDate,
                    ppfId = t.ProposalFilledFormId
                })
                .ToList()
                .Select(t => new ProposalFilledFormDocumentMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    type = t.type.GetEnumDisplayName(),
                    bankId = t.bankId,
                    price = t.price.ToString("###,###"),
                    createDate = t.createDate.ToFaDate(),
                    arriveDate = t.arriveDate != null ? t.arriveDate.ToFaDate() : "",
                    cashDate = t.cashDate != null ? t.cashDate.ToFaDate() : "",
                    ppfId = t.ppfId,
                    status =
                        t.cashDate == null && t.arriveDate != null && t.arriveDate.Value < startOfToday ? CreditStatus.ItsTimePass.GetEnumDisplayName() :
                         t.cashDate == null && t.arriveDate != null && t.arriveDate.Value >= endOfToday ? CreditStatus.NotYet.GetEnumDisplayName() :
                          t.cashDate == null && t.arriveDate != null && t.arriveDate.Value.Year == curDate.Year && t.arriveDate.Value.Month == curDate.Month && t.arriveDate.Value.Day == curDate.Day ? CreditStatus.ItsTimeKnow.GetEnumDisplayName() :
                          t.cashDate != null ? CreditStatus.GetMony.GetEnumDisplayName() :
                          "نامشخص"
                })
                .ToList()
            };
        }
    }
}
