using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.PageForms;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.DB;
using Oje.Section.Tender.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public class TenderFilledFormsValueService : ITenderFilledFormsValueService
    {
        readonly TenderDBContext db = null;
        readonly ITenderFilledFormKeyService TenderFilledFormKeyService = null;

        public TenderFilledFormsValueService(TenderDBContext db, ITenderFilledFormKeyService TenderFilledFormKeyService)
        {
            this.db = db;
            this.TenderFilledFormKeyService = TenderFilledFormKeyService;
        }


        public void CreateByForm(long tenderFilledForm, IFormCollection form, PageForm ppfObj, int? tenderProposalFormJsonConfigId, long? loginUserId = null, bool? isConsultation = null)
        {
            if (ppfObj != null && form != null && tenderFilledForm > 0)
            {
                var allCtrls = ppfObj.GetAllListOf<ctrl>();
                foreach (ctrl ctrl in allCtrls)
                {
                    if (ctrl.isCtrlVisible(form, allCtrls))
                    {
                        if (
                            ctrl.type == ctrlType.text ||
                            ctrl.type == ctrlType.dropDown2 ||
                            ctrl.type == ctrlType.dropDown ||
                            ctrl.type == ctrlType.checkBox ||
                            ctrl.type == ctrlType.radio ||
                            ctrl.type == ctrlType.persianDateTime
                        )
                        {
                            string currValue = "";
                            if (ctrl.disabled == true && ctrl.multiPlay != null && ctrl.multiPlay.Count > 0)
                                currValue = ctrl.defV;
                            else if (ctrl.type == ctrlType.text || ctrl.type == ctrlType.persianDateTime || ctrl.type == ctrlType.checkBox || ctrl.type == ctrlType.radio)
                            {
                                if (!string.IsNullOrEmpty(ctrl.defV))
                                    currValue = ctrl.defV;
                                else
                                    currValue = form.GetStringIfExist(ctrl.name);
                            }
                            else if (ctrl.type == ctrlType.dropDown || ctrl.type == ctrlType.dropDown2)
                                currValue = ctrl.defV;
                            if (!string.IsNullOrEmpty(currValue))
                            {
                                if (currValue.Length > 4000)
                                    throw BException.GenerateNewException(String.Format(BMessages.X_Length_Can_Not_Be_More_Then_4000.GetEnumDisplayName(), ctrl.label));
                                long keyId = TenderFilledFormKeyService.CreateIfNeeded(ctrl.name);
                                if (keyId > 0)
                                    createUpdateIfNeeded(tenderFilledForm, keyId, currValue, tenderProposalFormJsonConfigId, loginUserId, isConsultation);
                            }

                        }
                    }
                }
                db.SaveChanges();
            }
        }

        public void DeleteConsultValues(long filledFormId, int jsonFormId)
        {
            var allValues = db.TenderFilledFormsValues.Where(t => t.IsConsultation == true && t.TenderFilledFormId == filledFormId && t.TenderProposalFormJsonConfigId == jsonFormId).ToList();
            if (allValues != null)
            {
                foreach (var value in allValues)
                    if (!value.IsSignature())
                        throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);
                foreach (var value in allValues)
                    db.Entry(value).State = EntityState.Deleted;
            }
                
            db.SaveChanges();
        }

        public object GetValues(long filledFormId, bool isConsultationv, int jsonFormId)
        {
            var result = new Dictionary<string, string>();

            var tempValue = db.TenderFilledFormsValues.Where(t => t.TenderFilledFormId == filledFormId && t.IsConsultation == isConsultationv && t.TenderProposalFormJsonConfigId == jsonFormId).Select(t => new { key = t.TenderFilledFormKey.Key, value = t.Value }).ToList();
            if (tempValue != null)
                foreach (var value in tempValue)
                    result.Add(value.key, value.value);

            return result;
        }

        private void createUpdateIfNeeded(
                long tenderFilledForm, long keyId, string currValue,
                int? tenderProposalFormJsonConfigId, long? loginUserId, bool? isConsultation
            )
        {
            if (isConsultation != true)
            {
                var newItem = new TenderFilledFormsValue()
                {
                    TenderFilledFormId = tenderFilledForm,
                    TenderFilledFormKeyId = keyId,
                    Value = currValue,
                    TenderProposalFormJsonConfigId = tenderProposalFormJsonConfigId,

                };
                db.Entry(newItem).State = EntityState.Added;
                newItem.FilledSignature();
            }
            else
            {
                var foundValue = db.TenderFilledFormsValues
                    .Where(t => t.TenderFilledFormKeyId == keyId && t.IsConsultation == isConsultation &&
                                t.TenderProposalFormJsonConfigId == tenderProposalFormJsonConfigId && t.TenderFilledFormId == tenderFilledForm
                            )
                    .FirstOrDefault();
                if (foundValue == null)
                {
                    var newItem = new TenderFilledFormsValue()
                    {
                        TenderFilledFormId = tenderFilledForm,
                        TenderFilledFormKeyId = keyId,
                        Value = currValue,
                        TenderProposalFormJsonConfigId = tenderProposalFormJsonConfigId,
                        UserId = loginUserId,
                        IsConsultation = isConsultation

                    };
                    db.Entry(newItem).State = EntityState.Added;
                    newItem.FilledSignature();
                }
                else
                {
                    if (foundValue.IsSignature())
                    {
                        foundValue.UserId = loginUserId;
                        foundValue.Value = currValue;
                    }
                }
            }

        }
    }
}
