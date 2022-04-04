using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models.PageForms;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.DB;
using Oje.Section.InsuranceContractBaseData.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class InsuranceContractProposalFilledFormValueService: IInsuranceContractProposalFilledFormValueService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        readonly IInsuranceContractProposalFilledFormKeyService InsuranceContractProposalFilledFormKeyService = null;
        public InsuranceContractProposalFilledFormValueService
            (
                InsuranceContractBaseDataDBContext db, 
                IInsuranceContractProposalFilledFormKeyService InsuranceContractProposalFilledFormKeyService
            )
        {
            this.db = db;
            this.InsuranceContractProposalFilledFormKeyService = InsuranceContractProposalFilledFormKeyService;
        }

        public void CreateByJsonConfig(PageForm ppfObj, long insuranceContractProposalFilledFormId, IFormCollection form)
        {
            if (ppfObj != null && form != null && insuranceContractProposalFilledFormId > 0)
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
                                int keyId = InsuranceContractProposalFilledFormKeyService.CreateIfNeeded(ctrl.name);
                                if (keyId > 0)
                                    addNewRow(insuranceContractProposalFilledFormId, keyId, currValue);
                            }

                        }
                    }
                }
                db.SaveChanges();
            }
        }

        private void addNewRow(long insuranceContractProposalFilledFormId, int keyId, string currValue)
        {
            db.Entry(new InsuranceContractProposalFilledFormValue()
            {
                InsuranceContractProposalFilledFormId = insuranceContractProposalFilledFormId,
                InsuranceContractProposalFilledFormKeyId = keyId,
                Value = currValue
            }).State = EntityState.Added;
            db.SaveChanges();
        }
    }
}
