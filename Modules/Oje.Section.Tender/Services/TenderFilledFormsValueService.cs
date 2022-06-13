using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models.PageForms;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.DB;
using Oje.Section.Tender.Services.EContext;
using System;

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

        public void CreateByForm(long tenderFilledForm, IFormCollection form, PageForm ppfObj, int? tenderProposalFormJsonConfigId)
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
                                    create(tenderFilledForm, keyId, currValue, tenderProposalFormJsonConfigId);
                            }

                        }
                    }
                }
                db.SaveChanges();
            }
        }

        private void create(long tenderFilledForm, long keyId, string currValue, int? tenderProposalFormJsonConfigId)
        {
            db.Entry(new TenderFilledFormsValue()
            {
                TenderFilledFormId = tenderFilledForm,
                TenderFilledFormKeyId = keyId,
                Value = currValue,
                TenderProposalFormJsonConfigId = tenderProposalFormJsonConfigId
            }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
        }
    }
}
