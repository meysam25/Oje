using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Oje.FileService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.PageForms;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFilledFormService : IProposalFilledFormService
    {
        readonly ProposalFormDBContext db = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly IProposalFormRequiredDocumentService ProposalFormRequiredDocumentService = null;
        readonly IGlobalInqueryService GlobalInqueryService = null;
        readonly IPaymentMethodService PaymentMethodService = null;
        readonly IBankService BankService = null;
        readonly IUserService InternalUserService = null;
        readonly IProposalFilledFormJsonService ProposalFilledFormJsonService = null;
        readonly IProposalFilledFormCompanyService ProposalFilledFormCompanyService = null;
        readonly IProposalFilledFormUseService ProposalFilledFormUseService = null;
        readonly IProposalFilledFormDocumentService ProposalFilledFormDocumentService = null;
        readonly IProposalFilledFormValueService ProposalFilledFormValueService = null;
        readonly AccountService.Interfaces.IUserService UserService = null;
        readonly AccountService.Interfaces.IRoleService RoleService = null;
        readonly IUploadedFileService UploadedFileService = null;
        readonly IProposalFilledFormAdminBaseQueryService ProposalFilledFormAdminBaseQueryService = null;
        readonly IUserNotifierService UserNotifierService = null;

        public ProposalFilledFormService(
                ProposalFormDBContext db,
                IProposalFormService ProposalFormService,
                IProposalFormRequiredDocumentService ProposalFormRequiredDocumentService,
                IGlobalInqueryService GlobalInqueryService,
                IPaymentMethodService PaymentMethodService,
                IBankService BankService,
                AccountService.Interfaces.IUserService UserService,
                IProposalFilledFormJsonService ProposalFilledFormJsonService,
                IProposalFilledFormCompanyService ProposalFilledFormCompanyService,
                IUserService InternalUserService,
                IProposalFilledFormUseService ProposalFilledFormUseService,
                IProposalFilledFormDocumentService ProposalFilledFormDocumentService,
                IProposalFilledFormValueService ProposalFilledFormValueService,
                IUploadedFileService UploadedFileService,
                IUserNotifierService UserNotifierService,
                IProposalFilledFormAdminBaseQueryService ProposalFilledFormAdminBaseQueryService,
                AccountService.Interfaces.IRoleService RoleService
            )
        {
            this.db = db;
            this.ProposalFormService = ProposalFormService;
            this.ProposalFormRequiredDocumentService = ProposalFormRequiredDocumentService;
            this.GlobalInqueryService = GlobalInqueryService;
            this.PaymentMethodService = PaymentMethodService;
            this.BankService = BankService;
            this.UserService = UserService;
            this.ProposalFilledFormJsonService = ProposalFilledFormJsonService;
            this.ProposalFilledFormCompanyService = ProposalFilledFormCompanyService;
            this.InternalUserService = InternalUserService;
            this.ProposalFilledFormUseService = ProposalFilledFormUseService;
            this.ProposalFilledFormDocumentService = ProposalFilledFormDocumentService;
            this.ProposalFilledFormValueService = ProposalFilledFormValueService;
            this.UploadedFileService = UploadedFileService;
            this.ProposalFilledFormAdminBaseQueryService = ProposalFilledFormAdminBaseQueryService;
            this.UserNotifierService = UserNotifierService;
            this.RoleService = RoleService;
        }

        public ApiResult Create(int? siteSettingId, IFormCollection form, long? loginUserId, string targetUrl)
        {
            createValidation(siteSettingId, form);

            long inquiryId = form.GetStringIfExist("inquiryId").ToLongReturnZiro();
            int proposalFormId = form.GetStringIfExist("fid").ToIntReturnZiro();
            var foundProposalForm = ProposalFormService.GetById(proposalFormId, siteSettingId);
            int payCondationId = form.GetStringIfExist("payCondation").ToIntReturnZiro();
            var allRequiredFileUpload = ProposalFormRequiredDocumentService.GetProposalFormRequiredDocuments(foundProposalForm?.Id, siteSettingId);
            long newFormId = 0;
            PageForm ppfObj = null;
            try { ppfObj = JsonConvert.DeserializeObject<PageForm>(foundProposalForm.JsonConfig); } catch (Exception ) {  } 
            //catch { };// catch (Exception) { throw; }
            int companyId = 0;

            if (inquiryId > 0)
                companyId = GlobalInqueryService.GetCompanyId(inquiryId, siteSettingId);

            createCtrlValidation(form, ppfObj, allRequiredFileUpload, siteSettingId, companyId);

            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    ProposalFilledForm newForm = createNewProposalFilledForm(siteSettingId, inquiryId, proposalFormId);
                    ProposalFilledFormJsonService.Create(newForm.Id, foundProposalForm.JsonConfig);
                    if (ppfObj.panels?.FirstOrDefault()?.hasInquiry == true && companyId > 0)
                        ProposalFilledFormCompanyService.Create(inquiryId, siteSettingId, newForm.Id, newForm.Price, companyId, true, loginUserId);
                    else if (ppfObj.panels?.FirstOrDefault()?.isCompanyListRequired == true)
                        ProposalFilledFormCompanyService.Create(form.GetStringIfExist("cIds"), newForm.Id, loginUserId);

                    long ownerUserId = InternalUserService.CreateUserForProposalFormIfNeeded(form, siteSettingId, loginUserId);
                    ProposalFilledFormUseService.Create(loginUserId, ProposalFilledFormUserType.CreateUser, loginUserId, newForm.Id);
                    if (ppfObj.panels?.FirstOrDefault().isAgentRequired == true)
                        ProposalFilledFormUseService.Create(form.GetStringIfExist("agentId").ToLongReturnZiro(), ProposalFilledFormUserType.Agent, loginUserId, newForm.Id);
                    ProposalFilledFormUseService.Create(ownerUserId, ProposalFilledFormUserType.OwnerUser, loginUserId, newForm.Id);
                    if (PaymentMethodService.Exist(siteSettingId, proposalFormId, companyId) && !GlobalInqueryService.HasAnyCashDiscount(inquiryId))
                        ProposalFilledFormDocumentService.CreateChequeArr(newForm.Id, newForm.Price, siteSettingId, PaymentMethodService.GetItemDetailes(payCondationId, siteSettingId, newForm.Price, proposalFormId)?.checkArr, form);
                    ProposalFilledFormValueService.CreateByJsonConfig(ppfObj, newForm.Id, form);
                    createUploadedFiles(siteSettingId, form, loginUserId, newForm.Id);

                    tr.Commit();

                    UserNotifierService.Notify(loginUserId, UserNotificationType.NewProposalFilledForm, ProposalFilledFormUseService.GetProposalFilledFormUserIds(newForm.Id.ToLongReturnZiro()), newForm.Id, foundProposalForm.Title, siteSettingId, "/ProposalFilledForm" + ProposalFilledFormAdminBaseQueryService.getControllerNameByStatus(ProposalFilledFormStatus.New) + "/PdfDetailesForAdmin?id=" + newForm.Id);
                    newFormId = newForm.Id;

                    if (RoleService.IsUserInRole(loginUserId, "user"))
                        targetUrl = "/Proposal/Detaile";
                    else
                        targetUrl = "";
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetEnumDisplayName(), data = new { url = targetUrl, id = newFormId } };
        }

        private void createUploadedFiles(int? siteSettingId, IFormCollection form, long? loginUserId, long proposalFilledFormId)
        {
            foreach (var file in form.Files)
            {
                UploadedFileService.UploadNewFile(FileType.ProposalFilledForm, file, loginUserId, siteSettingId, proposalFilledFormId, ".jpg,.png,.pdf,.doc,.docx,.xls", true);
            }
        }

        private ProposalFilledForm createNewProposalFilledForm(int? siteSettingId, long inquiryId, int proposalFormId)
        {
            ProposalFilledForm newItem = new ProposalFilledForm()
            {
                ProposalFormId = proposalFormId,
                Price = inquiryId > 0 ? GlobalInqueryService.GetSumPriceLong(inquiryId, proposalFormId, siteSettingId) : 0,
                Status = ProposalFilledFormStatus.New,
                GlobalInqueryId = inquiryId > 0 ? inquiryId : null,
                CreateDate = DateTime.Now,
                SiteSettingId = siteSettingId.Value
            };

            db.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();

            return newItem;
        }

        private void createCtrlValidation(IFormCollection form, PageForm ppfObj, List<ProposalFormRequiredDocument> allRequiredFileUpload, int? siteSettingId, int companyId)
        {
            if (ppfObj == null || ppfObj.panels == null || ppfObj.panels.Count == 0)
                throw BException.GenerateNewException(BMessages.Json_Convert_Error);

            validateCompanyAndAgent(ppfObj, form, siteSettingId, companyId);
            validateIfInquiryRequired(form, ppfObj, siteSettingId);

            var allCtrls = ppfObj.GetAllListOf<ctrl>();

            foreach (ctrl ctrl in allCtrls)
            {
                if (ctrl.isCtrlVisible(form, allCtrls) == true)
                {
                    ctrl.requiredValidationForCtrl(ctrl, form);
                    ctrl.reqularExperssionValidationCtrl(ctrl, form);
                    ctrl.validateBaseDataEnums(ctrl, form);
                    ctrl.validateAndUpdateValuesOfDS(ctrl, form);
                    ctrl.navionalCodeValidation(ctrl, form);
                    ctrl.validateAndUpdateCtrl(ctrl, form, allCtrls);
                    ctrl.validateAndUpdateMultiRowInputCtrl(ctrl, form, ppfObj);
                    ctrl.validateMinAndMaxDayForDateInput(ctrl, form);
                    ctrl.dublicateMapValueIfNeeded(ctrl, ppfObj, form);
                }
                validateFileUpload(ctrl, allRequiredFileUpload, form);
            }
        }


        private void validateCompanyAndAgent(PageForm ppfObj, IFormCollection form, int? siteSettingId, int companyId)
        {
            if (ppfObj.panels.FirstOrDefault().isAgentRequired == true && !form.ContainsKey("agentId"))
                throw BException.GenerateNewException(BMessages.Please_Select_Agent);
            if (ppfObj.panels.FirstOrDefault().isAgentRequired == true && (string.IsNullOrEmpty(form.GetStringIfExist("agentId")) || form.GetStringIfExist("agentId").ToLongReturnZiro() <= 0))
                throw BException.GenerateNewException(BMessages.Please_Select_Agent);
            if (ppfObj.panels.FirstOrDefault().hasInquiry == true && ppfObj.panels.FirstOrDefault().isAgentRequired == true && !UserService.IsValidAgent(form.GetStringIfExist("agentId").ToLongReturnZiro(), siteSettingId, form.GetStringIfExist("fid").ToIntReturnZiro(), companyId))
                throw BException.GenerateNewException(BMessages.Please_Select_Agent);
            else if (ppfObj.panels.FirstOrDefault().isAgentRequired == true && !UserService.IsValidAgent(form.GetStringIfExist("agentId").ToLongReturnZiro(), siteSettingId, form.GetStringIfExist("fid").ToIntReturnZiro()))
                throw BException.GenerateNewException(BMessages.Please_Select_Agent);
            if (ppfObj.panels.FirstOrDefault().hasInquiry == true && companyId <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);

            if (ppfObj.panels.FirstOrDefault().isCompanyListRequired == true && !form.ContainsKey("cIds"))
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (ppfObj.panels.FirstOrDefault().isCompanyListRequired == true && string.IsNullOrEmpty(form.GetStringIfExist("cIds")))
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
        }



        private void validateIfInquiryRequired(IFormCollection form, PageForm ppfObj, int? siteSettingId)
        {
            if (ppfObj.panels.Select(t => t.hasInquiry).FirstOrDefault() == true)
            {
                if (!form.Keys.Any(t => t == "inquiryId"))
                    throw BException.GenerateNewException(BMessages.Invalid_Inquiry);
                var inQuiryId = form.GetStringIfExist("inquiryId").ToLongReturnZiro();
                if (inQuiryId <= 0)
                    throw BException.GenerateNewException(BMessages.Invalid_Inquiry);
                if (db.ProposalFilledForms.Any(t => t.GlobalInqueryId == inQuiryId))
                    throw BException.GenerateNewException(BMessages.Invalid_Inquiry);
                if (!GlobalInqueryService.IsValid(inQuiryId, siteSettingId, form.GetStringIfExist("fid").ToIntReturnZiro()))
                    throw BException.GenerateNewException(BMessages.Invalid_Inquiry);
                int companyId = GlobalInqueryService.GetCompanyId(inQuiryId, siteSettingId);
                validateDebitPayment(form, siteSettingId, inQuiryId, companyId);
            }
        }

        private void validateDebitPayment(IFormCollection form, int? siteSettingId, long inQuiryId, int companyId)
        {
            if (PaymentMethodService.Exist(siteSettingId, form.GetStringIfExist("fid").ToIntReturnZiro(), companyId) && !GlobalInqueryService.HasAnyCashDiscount(inQuiryId))
            {
                if (!form.Keys.Any(t => t == "payCondation"))
                    throw BException.GenerateNewException(BMessages.Please_Select_PayCondation);
                int payCondationId = form.GetStringIfExist("payCondation").ToIntReturnZiro();
                if (payCondationId <= 0)
                    throw BException.GenerateNewException(BMessages.Invalid_PayCondation);
                if (!PaymentMethodService.IsValid(payCondationId, siteSettingId, form.GetStringIfExist("fid").ToIntReturnZiro(), companyId))
                    throw BException.GenerateNewException(BMessages.Invalid_PayCondation);
                var allPaymentMethods = PaymentMethodService.GetLightList(form.GetStringIfExist("fid").ToIntReturnZiro(), siteSettingId, companyId);
                var foundCurrentPayementMethod = allPaymentMethods.Where(t => t.id == payCondationId.ToString()).FirstOrDefault();
                if (foundCurrentPayementMethod == null)
                    throw BException.GenerateNewException(BMessages.Invalid_PayCondation);
                var paymentMetodDetailes = PaymentMethodService.GetItemDetailes(payCondationId, siteSettingId, inQuiryId, form.GetStringIfExist("fid").ToIntReturnZiro());
                if (paymentMetodDetailes != null && paymentMetodDetailes.checkArr != null && paymentMetodDetailes.checkArr.Count > 0)
                {
                    List<int> bankIds = new List<int>();
                    for (var i = 0; i < paymentMetodDetailes.checkArr.Count; i++)
                    {
                        if (!form.Keys.Any(t => t == ("check[" + i + "].checkNumber")) || string.IsNullOrEmpty(form.GetStringIfExist(("check[" + i + "].checkNumber"))))
                            throw BException.GenerateNewException(String.Format(BMessages.Please_Enter_CheckNumber_RowX.GetEnumDisplayName(), i + 1));
                        if (!form.Keys.Any(t => t == ("check[" + i + "].bankId")) || string.IsNullOrEmpty(form.GetStringIfExist(("check[" + i + "].bankId"))) || form.GetStringIfExist(("check[" + i + "].bankId")).ToIntReturnZiro() <= 0)
                            throw BException.GenerateNewException(String.Format(BMessages.Please_Select_Bank_RowX.GetEnumDisplayName(), i + 1));
                        if (!bankIds.Any(t => t == form.GetStringIfExist(("check[" + i + "].bankId")).ToIntReturnZiro()))
                            bankIds.Add(form.GetStringIfExist(("check[" + i + "].bankId")).ToIntReturnZiro());
                    }
                    if (!BankService.IsValid(bankIds))
                        throw BException.GenerateNewException(BMessages.Invalid_Bank);
                }
                if (paymentMetodDetailes != null && paymentMetodDetailes.paymentRequreFiles != null && paymentMetodDetailes.paymentRequreFiles.Count > 0)
                {
                    for (var i = 0; i < paymentMetodDetailes.paymentRequreFiles.Count; i++)
                    {
                        var file = paymentMetodDetailes.paymentRequreFiles[i];
                        if (file.isRequired == true && (form.Files[file.title.Replace(" ", "")] == null || form.Files[file.title.Replace(" ", "")].Length == 0))
                            throw BException.GenerateNewException(String.Format(BMessages.Please_Select_X.GetEnumDisplayName(), file.title));
                    }
                }
            }
        }

        private void validateFileUpload(ctrl ctrl, List<Models.DB.ProposalFormRequiredDocument> allRequiredFileUpload, IFormCollection form)
        {
            if (ctrl.type == ctrlType.dynamicFileUpload)
            {
                if (allRequiredFileUpload != null && allRequiredFileUpload.Count > 0)
                {
                    var allRequiredFiles = allRequiredFileUpload.Where(t => t.IsRequired == true).ToList();
                    foreach (var file in allRequiredFiles)
                        if (form.Files[file.Title.Replace(" ", "")] == null || form.Files[file.Title.Replace(" ", "")].Length == 0)
                            throw BException.GenerateNewException(String.Format(BMessages.Please_Select_X.GetEnumDisplayName(), file.Title));
                }
            }
        }

        private void createValidation(int? siteSettingId, IFormCollection form)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (form == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (!form.ContainsKey("fid"))
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);

            int proposalFormId = form.GetStringIfExist("fid").ToIntReturnZiro();
            if (proposalFormId <= 0)
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);
            if (!ProposalFormService.Exist(proposalFormId, siteSettingId))
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);
        }
    }
}
