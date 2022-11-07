using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oje.FileService.Interfaces;
using Oje.FileService.Services;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.PageForms;
using Oje.Infrastructure.Models.Pdf.ProposalFilledForm;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Models.DB;
using Oje.Section.GlobalForms.Models.View;
using Oje.Section.GlobalForms.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.GlobalForms.Services
{
    public class GeneralFilledFormService : IGeneralFilledFormService
    {
        readonly GeneralFormDBContext db = null;
        readonly IGeneralFormService GeneralFormService = null;
        readonly IGeneralFormRequiredDocumentService GeneralFormRequiredDocumentService = null;
        readonly IGeneralFormStatusService GeneralFormStatusService = null;
        readonly IGeneralFormJsonService GeneralFormJsonService = null;
        readonly IInternalUserService InternalUserService = null;
        readonly IGeneralFilledFormValueService GeneralFilledFormValueService = null;
        readonly IUploadedFileService UploadedFileService = null;
        static string accpetableExtension = ".jpg,.png,.pdf,.doc,.docx,.xls";

        public GeneralFilledFormService
            (
                GeneralFormDBContext db,
                IGeneralFormService GeneralFormService,
                IGeneralFormRequiredDocumentService GeneralFormRequiredDocumentService,
                IGeneralFormStatusService GeneralFormStatusService,
                IGeneralFormJsonService GeneralFormJsonService,
                IInternalUserService InternalUserService,
                IGeneralFilledFormValueService GeneralFilledFormValueService,
                IUploadedFileService UploadedFileService
            )
        {
            this.db = db;
            this.GeneralFormService = GeneralFormService;
            this.GeneralFormRequiredDocumentService = GeneralFormRequiredDocumentService;
            this.GeneralFormStatusService = GeneralFormStatusService;
            this.GeneralFormJsonService = GeneralFormJsonService;
            this.InternalUserService = InternalUserService;
            this.GeneralFilledFormValueService = GeneralFilledFormValueService;
            this.UploadedFileService = UploadedFileService;
        }

        public object Create(int? siteSettingId, IFormCollection form, LoginUserVM loginUser)
        {
            var validObj = createValidation(siteSettingId, form, loginUser);

            var newForm = create(validObj.formId, validObj.fistStatusId, siteSettingId.Value, loginUser.UserId);
            GeneralFormJsonService.Create(newForm.Id, validObj.jsonConfig);
            InternalUserService.UpdateUserInfoIfNeeded(form, loginUser.UserId, siteSettingId);
            GeneralFilledFormValueService.CreateByJsonConfig(validObj.ppfObj, newForm.Id, form, validObj.allCtrls);
            foreach (var file in form.Files)
                UploadedFileService.UploadNewFile(FileType.ProposalFilledForm, file, loginUser?.UserId, siteSettingId, newForm.Id, accpetableExtension, true);

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetEnumDisplayName(), data = new { url = "/General/Detaile", id = newForm.Id } };
        }

        GeneralFilledForm create(long formId, long fistStatusId, int siteSettingId, long userId)
        {
            var newItem = new GeneralFilledForm()
            {
                CreateDate = DateTime.Now,
                GeneralFormId = formId,
                GeneralFormStatusId = fistStatusId,
                IsDelete = false,
                SiteSettingId = siteSettingId,
                CreateUserId = userId
            };
            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            return newItem;
        }

        public (long formId, List<GeneralFormRequiredDocument> allRequiredFileUpload, long fistStatusId, PageForm ppfObj, List<ctrl> allCtrls, string jsonConfig)
            createValidation(int? siteSettingId, IFormCollection form, LoginUserVM loginUser)
        {
            List<GeneralFormRequiredDocument> allRequiredFileUpload = null;
            GeneralForm foundForm = null;
            PageForm ppfObj = null;
            List<ctrl> allCtrls = null;

            long formId = 0;
            long fistStatusId = 0;

            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (loginUser == null || loginUser.UserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            if (form == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (!form.ContainsKey("fid"))
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);
            formId = form.GetStringIfExist("fid").ToLongReturnZiro();
            if (formId <= 0)
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);
            foundForm = GeneralFormService.GetByIdNoTracking(formId, siteSettingId);
            if (foundForm == null)
                throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);
            allRequiredFileUpload = GeneralFormRequiredDocumentService.GetActiveListNoTracking(foundForm.Id);
            fistStatusId = GeneralFormStatusService.GetFirstId(foundForm.Id);
            if (fistStatusId <= 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            try { ppfObj = JsonConvert.DeserializeObject<PageForm>(foundForm.JsonConfig); } catch (Exception) { }
            if (ppfObj == null || ppfObj.panels == null || ppfObj.panels.Count == 0)
                throw BException.GenerateNewException(BMessages.Json_Convert_Error);
            foreach (var file in form.Files)
                if (!file.IsValidExtension(accpetableExtension))
                    throw BException.GenerateNewException(BMessages.File_Is_Not_Valid);
            allCtrls = ppfObj.GetAllListOf<ctrl>();
            if (allCtrls == null || allCtrls.Count == 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);

            List<IdTitle> exteraTextBox = new List<IdTitle>();

            foreach (ctrl ctrl in allCtrls)
            {
                if (ctrl.isCtrlVisible(form, allCtrls) == true)
                {
                    if ((ctrl.type == ctrlType.text || ctrl.type == ctrlType.textarea) && !string.IsNullOrEmpty(ctrl.name))
                    {
                        var curValue = form.GetStringIfExist(ctrl.name);
                        if (!string.IsNullOrEmpty(curValue) && curValue.Length > 4000)
                            throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);
                    }
                    ctrl.requiredValidationForCtrl(ctrl, form);
                    ctrl.reqularExperssionValidationCtrl(ctrl, form);
                    ctrl.validateBaseDataEnums(ctrl, form);
                    ctrl.validateAndUpdateValuesOfDS(ctrl, form);
                    ctrl.navionalCodeValidation(ctrl, form);
                    ctrl.validateAndUpdateCtrl(ctrl, form, allCtrls);
                    ctrl.validateAndUpdateMultiRowInputCtrl(ctrl, form, ppfObj);
                    ctrl.validateMinAndMaxDayForDateInput(ctrl, form);
                    exteraTextBox.AddRange(ctrl.dublicateMapValueIfNeeded(ctrl, ppfObj, form));
                }
                validateFileUpload(ctrl, allRequiredFileUpload, form);
            }

            ppfObj.exteraCtrls = exteraTextBox;

            return (formId, allRequiredFileUpload, fistStatusId, ppfObj, allCtrls, foundForm.JsonConfig);
        }

        void validateFileUpload(ctrl ctrl, List<GeneralFormRequiredDocument> allRequiredFileUpload, IFormCollection form)
        {
            if (ctrl.type == ctrlType.dynamicFileUpload)
            {
                if (allRequiredFileUpload != null && allRequiredFileUpload.Count > 0)
                {
                    var allRequiredFiles = allRequiredFileUpload.Where(t => t.IsRequired == true).ToList();
                    foreach (var file in allRequiredFiles)
                        if (form.Files[file.Name.Replace(" ", "")] == null || form.Files[file.Name.Replace(" ", "")].Length == 0)
                            throw BException.GenerateNewException(String.Format(BMessages.Please_Select_X.GetEnumDisplayName(), file.Title));
                }
            }
        }

        public GeneralFilledFormPdfDetailesVM PdfDetailes(long id, int? siteSettingId, long? loginUserId)
        {
            var result = new GeneralFilledFormPdfDetailesVM();
            var foundItem = db.GeneralFilledForms.Where(t => t.Id == id && t.SiteSettingId == siteSettingId && t.CreateUserId == loginUserId)
               .Select(t => new
               {
                   t.Id,
                   t.Price,
                   t.GeneralFormId,
                   t.CreateDate,
                   t.SiteSettingId,
                   t.PaymentTraceCode,
                   ppfTitle = t.GeneralForm.Title,
                   createUserFullname = t.CreateUser.Firstname + " " + t.CreateUser.Lastname,
                   values = t.GeneralFilledFormValues.Select(tt => new
                   {
                       tt.Value,
                       Key = tt.GeneralFilledFormKey.Key
                   }).ToList()
               })
               .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var foundJson = GeneralFormJsonService.GetCacheBy(foundItem.Id);
            if (foundJson == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Json_Config);
            var jsonObj = JsonConvert.DeserializeObject<PageForm>(foundJson.JsonConfig);
            if (jsonObj == null)
                throw BException.GenerateNewException(BMessages.Json_Convert_Error);
            var foundSw = jsonObj.GetAllListOf<stepWizard>();
            if (foundSw == null || foundSw.Count == 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            foundSw.FirstOrDefault().steps = ignoreSteps(foundSw.FirstOrDefault().steps);
            var fFoundSw = foundSw.FirstOrDefault();
            List<FilledFormPdfGroupVM> listGroup = new();
            List<ProposalFilledFormPaymentVM> foundPaymentList = new List<ProposalFilledFormPaymentVM>();
            if (foundPaymentList != null && foundPaymentList.Count > 0)
            {
                List<FilledFormPdfGroupItem> ProposalFilledFormPdfGroupPaymentItems = new();
                foreach (var item in foundPaymentList)
                {
                    ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "نام حساب", value = item.fullName });
                    ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "مبلغ", value = item.price.ToString("###,###") + " ریال" });
                    ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "تاریخ", value = item.payDate.ToFaDate() + " " + item.payDate.ToString("hh:mm") });
                    ProposalFilledFormPdfGroupPaymentItems.Add(new FilledFormPdfGroupItem() { cssClass = "col-md-4 col-sm-6 col-xs-12 col-lg-3", title = "کد پیگیری", value = item.traceCode });
                }
                listGroup.Add(new FilledFormPdfGroupVM() { title = "وضعیت پرداخت", ProposalFilledFormPdfGroupItems = ProposalFilledFormPdfGroupPaymentItems });
            }
           
            foreach (var step in fFoundSw.steps)
            {
                var allCtrls = step.GetAllListOf<ctrl>();
                List<FilledFormPdfGroupItem> ProposalFilledFormPdfGroupItems = new();

                if (allCtrls != null && allCtrls.Count > 0)
                {
                    foreach (var ctrl in allCtrls)
                    {
                        string title = !string.IsNullOrEmpty(ctrl.label) ? ctrl.label : ctrl.ph;
                        string value = foundItem.values.Where(t => t.Key == ctrl.name).Select(t => t.Value).FirstOrDefault();
                        if ((!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(value)) || ctrl.type == ctrlType.multiRowInput)
                        {
                            if (ctrl.type == ctrlType.checkBox)
                                title = "";
                            if (ctrl.type == ctrlType.multiRowInput && ctrl.ctrls != null && ctrl.ctrls.Count > 0)
                            {
                                for (var i = 0; i < 20; i++)
                                    foreach (var subCtrl in ctrl.ctrls)
                                    {
                                        string currKey = ctrl.name + "[" + i + "]." + subCtrl.name;
                                        string subTitle = !string.IsNullOrEmpty(subCtrl.label) ? subCtrl.label : subCtrl.ph;
                                        string subValue = foundItem.values.Where(t => t.Key == currKey).Select(t => t.Value).FirstOrDefault();
                                        if (!string.IsNullOrEmpty(subTitle) && !string.IsNullOrEmpty(subValue))
                                            ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem() { cssClass = subCtrl.parentCL, title = subTitle, value = subValue });
                                    }
                            }
                            else if ((!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(value)) || (ctrl.type == ctrlType.checkBox && !string.IsNullOrEmpty(value)))
                                ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem() { cssClass = ctrl.parentCL, title = title, value = value });
                        }
                        else if (ctrl.type == ctrlType.carPlaque)
                        {
                            string value1 = foundItem.values.Where(t => t.Key == ctrl.name).Select(t => t.Value).FirstOrDefault();
                            if (!string.IsNullOrEmpty(value1))
                                ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem() { cssClass = ctrl.parentCL, title = title, value = value1 });

                        }
                    }
                    if (ProposalFilledFormPdfGroupItems.Count > 0)
                        listGroup.Add(new FilledFormPdfGroupVM() { title = step.title, ProposalFilledFormPdfGroupItems = ProposalFilledFormPdfGroupItems });
                }
            }

            //result.loginUserWalletBalance = UserService.GetUserWalletBalance(userId, siteSettingId);


            result.generalFilledFormId = foundItem.Id;
            result.generalFilledFormPdfGroupVMs = listGroup;
            result.traceCode = foundItem.PaymentTraceCode;
            result.ppfTitle = foundItem.ppfTitle;
            result.id = foundItem.SiteSettingId + "/" + foundItem.GeneralFormId + "/" + foundItem.Id;
            result.price = foundItem.Price;
            result.ppfCreateDate = foundItem.CreateDate.ToFaDate();

            return result;
        }

        private List<step> ignoreSteps(List<step> allSteps)
        {
            if (allSteps != null)
                return allSteps.Where(t =>  t.id != "requiredDocumnet" && t.id != "selectAgent" && t.id != "companyStep").ToList();

            return allSteps;
        }

        public GeneralFilledFormPdfDetailesVM PdfDetailesByForm(IFormCollection form, int? siteSettingId)
        {
            var result = new GeneralFilledFormPdfDetailesVM();
            int proposalFormId = form.GetStringIfExist("fid").ToIntReturnZiro();
            if (proposalFormId <= 0)
                return result;
            var foundProposalForm = GeneralFormService.GetByIdNoTracking(proposalFormId, siteSettingId);
            PageForm jsonObj = null;
            try { jsonObj = JsonConvert.DeserializeObject<PageForm>(foundProposalForm.JsonConfig); } catch (Exception) { }
            if (jsonObj == null)
                throw BException.GenerateNewException(BMessages.Json_Convert_Error);
            var foundSw = jsonObj.GetAllListOf<stepWizard>();
            if (foundSw == null || foundSw.Count == 0)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            foundSw.FirstOrDefault().steps = ignoreSteps(foundSw.FirstOrDefault().steps);
            var fFoundSw = foundSw.FirstOrDefault();
            List<FilledFormPdfGroupVM> listGroup = new();

            var values = form.Keys.Select(t => new { Key = t, Value = form.GetStringIfExist(t) }).ToList();
            foreach (var step in fFoundSw.steps)
            {
                var allCtrls = step.GetAllListOf<ctrl>();
                List<FilledFormPdfGroupItem> ProposalFilledFormPdfGroupItems = new();

                if (allCtrls != null && allCtrls.Count > 0)
                {
                    foreach (var ctrl in allCtrls)
                    {
                        string title = !string.IsNullOrEmpty(ctrl.label) ? ctrl.label : ctrl.ph;
                        string value = values.Where(t => t.Key == ctrl.name).Select(t => t.Value).FirstOrDefault();
                        if ((!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(value)) || ctrl.type == ctrlType.multiRowInput)
                        {
                            if (!string.IsNullOrEmpty(ctrl.dataurl) && ctrl.dataurl.Contains("/Core/BaseData/Get/"))
                            {
                                string enumName = ctrl.dataurl.Replace("/Core/BaseData/Get/", "");
                                var allEnums = EnumService.GetEnum(enumName);
                                if (allEnums != null && allEnums.Count > 0)
                                {
                                    var foundItem = allEnums.Where(t => t.id == value).FirstOrDefault();
                                    if (foundItem != null)
                                        value = foundItem.title;
                                }
                            }
                            if (ctrl.type == ctrlType.checkBox)
                                title = "";
                            if (ctrl.type == ctrlType.multiRowInput && ctrl.ctrls != null && ctrl.ctrls.Count > 0)
                            {
                                for (var i = 0; i < 20; i++)
                                {
                                    foreach (var subCtrl in ctrl.ctrls)
                                    {
                                        string currKey = ctrl.name + "[" + i + "]." + subCtrl.name;
                                        string subTitle = !string.IsNullOrEmpty(subCtrl.label) ? subCtrl.label : subCtrl.ph;
                                        string subValue = values.Where(t => t.Key == currKey).Select(t => t.Value).FirstOrDefault();
                                        if (!string.IsNullOrEmpty(subTitle) && !string.IsNullOrEmpty(subValue))
                                        {
                                            ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem() { cssClass = subCtrl.parentCL, title = subTitle, value = subValue });
                                        }
                                    }
                                }
                            }
                            else if ((!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(value)) || (ctrl.type == ctrlType.checkBox && !string.IsNullOrEmpty(value)))
                                ProposalFilledFormPdfGroupItems.Add(new FilledFormPdfGroupItem() { cssClass = ctrl.parentCL, title = title, value = value });
                        }
                    }
                    if (ProposalFilledFormPdfGroupItems.Count > 0)
                        listGroup.Add(new FilledFormPdfGroupVM() { title = step.title, ProposalFilledFormPdfGroupItems = ProposalFilledFormPdfGroupItems });
                }
            }

            result.generalFilledFormId = 0;
            result.generalFilledFormPdfGroupVMs = listGroup;
            result.ppfTitle = foundProposalForm.Title;
            result.ppfCreateDate = DateTime.Now.ToFaDate();

            return result;
        }
    }
}
