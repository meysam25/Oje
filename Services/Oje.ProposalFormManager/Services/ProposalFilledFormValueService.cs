﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models.PageForms;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFilledFormValueService : IProposalFilledFormValueService
    {
        readonly ProposalFormDBContext db = null;
        readonly IProposalFilledFormKeyService ProposalFilledFormKeyService = null;
        public ProposalFilledFormValueService(ProposalFormDBContext db, IProposalFilledFormKeyService ProposalFilledFormKeyService)
        {
            this.db = db;
            this.ProposalFilledFormKeyService = ProposalFilledFormKeyService;
        }

        void addNewRow(long proposalFilledFormId, int keyId, string currValue)
        {
            var newItem = new ProposalFilledFormValue()
            {
                ProposalFilledFormId = proposalFilledFormId,
                ProposalFilledFormKeyId = keyId,
                Value = currValue
            };
            newItem.FilledSignature();
            db.Entry(newItem).State = EntityState.Added;
        }

        public void CreateByJsonConfig(PageForm ppfObj, long proposalFilledFormId, IFormCollection form, List<ctrl> allCtrls, bool? isEdit = false)
        {
            if (ppfObj != null && form != null && proposalFilledFormId > 0)
            {
                foreach (ctrl ctrl in allCtrls)
                {
                    if (ctrl.isCtrlVisible(form, allCtrls))
                    {
                        if 
                        (
                            ctrl.type == ctrlType.text ||
                            ctrl.type == ctrlType.dropDown2 ||
                            ctrl.type == ctrlType.dropDown ||
                            ctrl.type == ctrlType.checkBox ||
                            ctrl.type == ctrlType.radio ||
                            ctrl.type == ctrlType.persianDateTime ||
                            ctrl.type == ctrlType.carPlaque || 
                            ctrl.type == ctrlType.number
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
                            else if (ctrl.type == ctrlType.carPlaque)
                                currValue = ctrl.defV;
                            if (!string.IsNullOrEmpty(currValue))
                            {
                                if (currValue.Length > 4000)
                                    currValue = currValue.Substring(0, 3999);
                                int keyId = ProposalFilledFormKeyService.CreateIfNeeded(ctrl.name);
                                if (keyId > 0)
                                {
                                    if (isEdit == false)
                                    {
                                        addNewRow(proposalFilledFormId, keyId, currValue);
                                    }
                                    else
                                    {
                                        var foundValue = db.ProposalFilledFormValues.Where(t => t.ProposalFilledFormKeyId == keyId && t.ProposalFilledFormId == proposalFilledFormId).FirstOrDefault();
                                        if (foundValue != null)
                                        {
                                            if (!foundValue.IsSignature())
                                                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);
                                            foundValue.Value = currValue;
                                            foundValue.FilledSignature();
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            addNewRow(proposalFilledFormId, keyId, currValue);
                                        }
                                    }

                                }
                            }

                        }
                    }
                }

                if (ppfObj.exteraCtrls != null)
                {
                    foreach (var item in ppfObj.exteraCtrls)
                    {
                        var currValue = form.GetStringIfExist(item.id);
                        if (!string.IsNullOrEmpty(currValue))
                        {
                            int keyId = ProposalFilledFormKeyService.CreateIfNeeded(item.id);
                            addNewRow(proposalFilledFormId, keyId, currValue);
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        public void UpdateBy(long proposalFilledFormId, IFormCollection form, PageForm jsonObj)
        {
            if (jsonObj == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            updateValidation(jsonObj, form);
            removeChekBoxCtrls(jsonObj, proposalFilledFormId);
            CreateByJsonConfig(jsonObj, proposalFilledFormId, form, jsonObj.GetAllListOf<ctrl>(), true);
        }

        private void removeChekBoxCtrls(PageForm jsonObj, long proposalFilledFormId)
        {
            var allCheckBoxs = jsonObj.GetAllListOf<ctrl>().Where(t => t.type == ctrlType.checkBox).ToList();

            bool hasAnyRow = false;
            foreach (var cb in allCheckBoxs)
            {
                if (!string.IsNullOrEmpty(cb.name))
                {
                    var foundPrevCheckBoxValue = db.ProposalFilledFormValues.Where(t => t.ProposalFilledFormId == proposalFilledFormId && t.ProposalFilledFormKey.Key == cb.name).FirstOrDefault();
                    if (foundPrevCheckBoxValue != null)
                    {
                        db.Entry(foundPrevCheckBoxValue).State = EntityState.Deleted;
                        hasAnyRow = true;
                    }
                }
            }
            if (hasAnyRow == true)
                db.SaveChanges();
        }

        private void updateValidation(PageForm jsonObj, IFormCollection form)
        {
            var allCtrls = jsonObj.GetAllListOf<ctrl>();

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
                    ctrl.validateAndUpdateMultiRowInputCtrl(ctrl, form, jsonObj);
                }
            }
        }
    }
}
