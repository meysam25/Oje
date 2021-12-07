using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.PageForms;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Services
{
    public class ProposalFilledFormManager : IProposalFilledFormManager
    {
        readonly ProposalFormDBContext db = null;
        readonly IProposalFormManager ProposalFormManager = null;
        readonly IProposalFormRequiredDocumentManager ProposalFormRequiredDocumentManager = null;
        readonly IGlobalInqueryManager GlobalInqueryManager = null;
        readonly IPaymentMethodManager PaymentMethodManager = null;
        readonly IBankManager BankManager = null;
        readonly IUserManager InternalUserManager = null;
        readonly IProposalFilledFormJsonManager ProposalFilledFormJsonManager = null;
        readonly IProposalFilledFormCompanyManager ProposalFilledFormCompanyManager = null;
        readonly IProposalFilledFormUseManager ProposalFilledFormUseManager = null;
        readonly IProposalFilledFormDocumentManager ProposalFilledFormDocumentManager = null;
        readonly IProposalFilledFormValueManager ProposalFilledFormValueManager = null;
        readonly AccountManager.Interfaces.IUserManager UserManager = null;
        readonly AccountManager.Interfaces.IUploadedFileManager UploadedFileManager = null;

        public ProposalFilledFormManager(
                ProposalFormDBContext db,
                IProposalFormManager ProposalFormManager,
                IProposalFormRequiredDocumentManager ProposalFormRequiredDocumentManager,
                IGlobalInqueryManager GlobalInqueryManager,
                IPaymentMethodManager PaymentMethodManager,
                IBankManager BankManager,
                AccountManager.Interfaces.IUserManager UserManager,
                IProposalFilledFormJsonManager ProposalFilledFormJsonManager,
                IProposalFilledFormCompanyManager ProposalFilledFormCompanyManager,
                IUserManager InternalUserManager,
                IProposalFilledFormUseManager ProposalFilledFormUseManager,
                IProposalFilledFormDocumentManager ProposalFilledFormDocumentManager,
                IProposalFilledFormValueManager ProposalFilledFormValueManager,
                AccountManager.Interfaces.IUploadedFileManager UploadedFileManager
            )
        {
            this.db = db;
            this.ProposalFormManager = ProposalFormManager;
            this.ProposalFormRequiredDocumentManager = ProposalFormRequiredDocumentManager;
            this.GlobalInqueryManager = GlobalInqueryManager;
            this.PaymentMethodManager = PaymentMethodManager;
            this.BankManager = BankManager;
            this.UserManager = UserManager;
            this.ProposalFilledFormJsonManager = ProposalFilledFormJsonManager;
            this.ProposalFilledFormCompanyManager = ProposalFilledFormCompanyManager;
            this.InternalUserManager = InternalUserManager;
            this.ProposalFilledFormUseManager = ProposalFilledFormUseManager;
            this.ProposalFilledFormDocumentManager = ProposalFilledFormDocumentManager;
            this.ProposalFilledFormValueManager = ProposalFilledFormValueManager;
            this.UploadedFileManager = UploadedFileManager;
        }

        public ApiResult Create(int? siteSettingId, IFormCollection form, long? loginUserId)
        {
            createValidation(siteSettingId, form);

            long inquiryId = form.GetStringIfExist("inquiryId").ToLongReturnZiro();
            int proposalFormId = form.GetStringIfExist("fid").ToIntReturnZiro();
            var foundProposalForm = ProposalFormManager.GetById(proposalFormId, siteSettingId);
            int payCondationId = form.GetStringIfExist("payCondation").ToIntReturnZiro();
            var allRequiredFileUpload = ProposalFormRequiredDocumentManager.GetProposalFormRequiredDocuments(foundProposalForm?.Id, siteSettingId);
            PageForm ppfObj = null;
            try { ppfObj = JsonConvert.DeserializeObject<PageForm>(foundProposalForm.JsonConfig); } catch { }
            createCtrlValidation(form, ppfObj, allRequiredFileUpload, siteSettingId);
            int companyId = GlobalInqueryManager.GetCompanyId(inquiryId, siteSettingId);



            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    ProposalFilledForm newForm = createNewProposalFilledForm(siteSettingId, inquiryId, proposalFormId);
                    ProposalFilledFormJsonManager.Create(newForm.Id, foundProposalForm.JsonConfig);
                    ProposalFilledFormCompanyManager.Create(inquiryId, siteSettingId, newForm.Id, newForm.Price, companyId);
                    long ownerUserId = InternalUserManager.CreateUserForProposalFormIfNeeded(form, siteSettingId, loginUserId);
                    ProposalFilledFormUseManager.Create(loginUserId, ProposalFilledFormUserType.CreateUser, loginUserId, newForm.Id);
                    ProposalFilledFormUseManager.Create(form.GetStringIfExist("agentId").ToLongReturnZiro(), ProposalFilledFormUserType.Agent, loginUserId, newForm.Id);
                    ProposalFilledFormUseManager.Create(ownerUserId, ProposalFilledFormUserType.OwnerUser, loginUserId, newForm.Id);
                    if (PaymentMethodManager.Exist(siteSettingId, proposalFormId, companyId) && !GlobalInqueryManager.HasAnyCashDiscount(inquiryId))
                        ProposalFilledFormDocumentManager.CreateChequeArr(newForm.Id, newForm.Price, siteSettingId, PaymentMethodManager.GetItemDetailes(payCondationId, siteSettingId, newForm.Price, proposalFormId)?.checkArr, form);
                    ProposalFilledFormValueManager.CreateByJsonConfig(ppfObj, newForm.Id, form);
                    createUploadedFiles(siteSettingId, form, loginUserId, newForm.Id);

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUploadedFiles(int? siteSettingId, IFormCollection form, long? loginUserId, long proposalFilledFormId)
        {
            foreach(var file in form.Files)
            {
                UploadedFileManager.UploadNewFile(FileType.ProposalFilledForm, file, loginUserId, siteSettingId, proposalFilledFormId, ".jpg,.png,.pdf,.doc,.docx,.xls", true);
            }
        }

        private ProposalFilledForm createNewProposalFilledForm(int? siteSettingId, long inquiryId, int proposalFormId)
        {
            ProposalFilledForm newItem = new ProposalFilledForm()
            {
                ProposalFormId = proposalFormId,
                Price = inquiryId > 0 ? GlobalInqueryManager.GetSumPriceLong(inquiryId, proposalFormId, siteSettingId) : 0,
                Status = ProposalFilledFormStatus.New,
                GlobalInqueryId = inquiryId > 0 ? inquiryId : 0,
                CreateDate = DateTime.Now,
                SiteSettingId = siteSettingId.Value
            };

            db.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();

            return newItem;
        }

        private void createCtrlValidation(IFormCollection form, PageForm ppfObj, List<Models.DB.ProposalFormRequiredDocument> allRequiredFileUpload, int? siteSettingId)
        {
            if (ppfObj == null)
                throw BException.GenerateNewException(BMessages.Json_Convert_Error);
            


            validateIfInquiryRequired(form, ppfObj, siteSettingId);

            var allCtrls = ppfObj.GetAllListOf<ctrl>();

            foreach (ctrl ctrl in allCtrls)
            {
                if (ctrl.isCtrlVisible(form, allCtrls) == true)
                {
                    requiredValidationForCtrl(ctrl, form);
                    reqularExperssionValidationCtrl(ctrl, form);
                    validateBaseDataEnums(ctrl, form);
                    navionalCodeValidation(ctrl, form);
                }
                validateFileUpload(ctrl, allRequiredFileUpload, form);

            }
        }

        private void navionalCodeValidation(ctrl ctrl, IFormCollection form)
        {
            if (ctrl.nationalCodeValidation == true && !form.GetStringIfExist(ctrl.name).IsCodeMeli())
                throw BException.GenerateNewException(BMessages.Invalid_NationaCode);
        }

        private void validateBaseDataEnums(ctrl ctrl, IFormCollection form)
        {
            if (ctrl.type == ctrlType.dropDown && !string.IsNullOrEmpty(ctrl.dataurl))
            {
                if (ctrl.dataurl.StartsWith("/Core/BaseData/Get/"))
                {
                    string selectValue = form.GetStringIfExist(ctrl.name);
                    if (!string.IsNullOrEmpty(selectValue))
                    {
                        string enumName = ctrl.dataurl.Replace("/Core/BaseData/Get/", "");
                        var foundEnum = EnumManager.GetEnum(enumName);

                        if (foundEnum == null || foundEnum.Count == 0)
                            throw BException.GenerateNewException(String.Format(BMessages.Invalid_BaseData.GetEnumDisplayName(), ctrl.dataurl));

                        if (!foundEnum.Any(tt => tt.id == selectValue || tt.title == selectValue))
                            throw BException.GenerateNewException(String.Format(BMessages.Invalid_BaseData.GetEnumDisplayName(), ctrl.dataurl));
                        ctrl.defV = foundEnum.Where(tt => tt.id == selectValue || tt.title == selectValue).Select(tt => tt.title).FirstOrDefault();
                    }
                }
            }
        }

        private void reqularExperssionValidationCtrl(ctrl ctrl, IFormCollection form)
        {

            if (ctrl.validations != null && ctrl.validations.Count > 0)
            {
                var curCTRLValue = form.Keys.Any(t => t == ctrl.name) ? form.GetStringIfExist(ctrl.name) : "";
                if (!string.IsNullOrEmpty(curCTRLValue))
                {
                    foreach (var validation in ctrl.validations)
                    {
                        Regex rx = new Regex(validation.reg, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        if (!rx.IsMatch(curCTRLValue))
                            throw BException.GenerateNewException(ctrl.label + ": " + validation.msg);
                    }
                }
            }
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
                if (!GlobalInqueryManager.IsValid(inQuiryId, siteSettingId, form.GetStringIfExist("fid").ToIntReturnZiro()))
                    throw BException.GenerateNewException(BMessages.Invalid_Inquiry);
                int companyId = GlobalInqueryManager.GetCompanyId(inQuiryId, siteSettingId);
                validateDebitPayment(form, siteSettingId, inQuiryId, companyId);
                if (!UserManager.IsValidAgent(form.GetStringIfExist("agentId").ToLongReturnZiro(), siteSettingId, form.GetStringIfExist("fid").ToIntReturnZiro(), companyId))
                    throw BException.GenerateNewException(BMessages.Please_Select_Agent);
            }
            else
            {
                if (!UserManager.IsValidAgent(form.GetStringIfExist("agentId").ToLongReturnZiro(), siteSettingId, form.GetStringIfExist("fid").ToIntReturnZiro()))
                    throw BException.GenerateNewException(BMessages.Please_Select_Agent);
            }
        }

        private void validateDebitPayment(IFormCollection form, int? siteSettingId, long inQuiryId, int companyId)
        {
            if (PaymentMethodManager.Exist(siteSettingId, form.GetStringIfExist("fid").ToIntReturnZiro(), companyId) && !GlobalInqueryManager.HasAnyCashDiscount(inQuiryId))
            {
                if (!form.Keys.Any(t => t == "payCondation"))
                    throw BException.GenerateNewException(BMessages.Please_Select_PayCondation);
                int payCondationId = form.GetStringIfExist("payCondation").ToIntReturnZiro();
                if (payCondationId <= 0)
                    throw BException.GenerateNewException(BMessages.Invalid_PayCondation);
                if (!PaymentMethodManager.IsValid(payCondationId, siteSettingId, form.GetStringIfExist("fid").ToIntReturnZiro(), companyId))
                    throw BException.GenerateNewException(BMessages.Invalid_PayCondation);
                var allPaymentMethods = PaymentMethodManager.GetLightList(form.GetStringIfExist("fid").ToIntReturnZiro(), siteSettingId, companyId);
                var foundCurrentPayementMethod = allPaymentMethods.Where(t => t.id == payCondationId.ToString()).FirstOrDefault();
                if (foundCurrentPayementMethod == null)
                    throw BException.GenerateNewException(BMessages.Invalid_PayCondation);
                var paymentMetodDetailes = PaymentMethodManager.GetItemDetailes(payCondationId, siteSettingId, inQuiryId, form.GetStringIfExist("fid").ToIntReturnZiro());
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
                    if (!BankManager.IsValid(bankIds))
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
            if (ctrl.type == Infrastructure.Enums.ctrlType.dynamicFileUpload)
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

        private void requiredValidationForCtrl(ctrl ctrl, IFormCollection form)
        {
            if (ctrl.isRequired == true)
                if (!form.Keys.Contains(ctrl.name) || string.IsNullOrEmpty(form.GetStringIfExist(ctrl.name)))
                    if(!needToBeIgnore(ctrl.name))
                    throw BException.GenerateNewException
                            (
                                string.Format
                                    (
                                        ctrl.type == Infrastructure.Enums.ctrlType.text ? BMessages.Please_Enter_X.GetEnumDisplayName() : BMessages.Please_Select_X.GetEnumDisplayName()
                                        , ctrl.label
                                    )
                            );

        }

        private bool needToBeIgnore(string name)
        {
            switch(name)
            {
                case "payCondation":
                    return true;
                default:
                    return false;
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
            if (!ProposalFormManager.Exist(proposalFormId, siteSettingId))
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);
        }
    }
}
