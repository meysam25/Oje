using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models.PageForms;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Models.DB;
using Oje.Section.GlobalForms.Services.EContext;
using System.Collections.Generic;

namespace Oje.Section.GlobalForms.Services
{
    public class GeneralFilledFormValueService: IGeneralFilledFormValueService
    {
        readonly GeneralFormDBContext db = null;
        readonly IGeneralFilledFormKeyService GeneralFilledFormKeyService = null;

        public GeneralFilledFormValueService
            (
                GeneralFormDBContext db,
                IGeneralFilledFormKeyService GeneralFilledFormKeyService
            )
        {
            this.db = db;
            this.GeneralFilledFormKeyService = GeneralFilledFormKeyService;
        }

        public void CreateByJsonConfig(PageForm ppfObj, long generalFilledFormId, IFormCollection form, List<ctrl> allCtrls)
        {
            if (ppfObj != null && form != null && generalFilledFormId > 0)
            {
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
                            ctrl.type == ctrlType.persianDateTime ||
                            ctrl.type == ctrlType.carPlaque ||
                            ctrl.type == ctrlType.number
                        )
                        {
                            string currValue = "";
                            if (ctrl.disabled == true && ctrl.multiPlay != null && ctrl.multiPlay.Count > 0)
                                currValue = ctrl.defV;
                            else if (ctrl.type == ctrlType.text || ctrl.type == ctrlType.persianDateTime || ctrl.type == ctrlType.checkBox || ctrl.type == ctrlType.radio || ctrl.type == ctrlType.number)
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
                                int keyId = GeneralFilledFormKeyService.CreateIfNeeded(ctrl.name, ctrl.label);
                                if (keyId > 0)
                                    addNewRow(generalFilledFormId, keyId, currValue);
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
                            int keyId = GeneralFilledFormKeyService.CreateIfNeeded(item.id, item.title);
                            addNewRow(generalFilledFormId, keyId, currValue);
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        void addNewRow(long generalFilledFormId, int keyId, string currValue)
        {
            db.Entry(new GeneralFilledFormValue()
            {
                GeneralFilledFormId = generalFilledFormId,
                GeneralFilledFormKeyId = keyId,
                Value = currValue
            }).State = EntityState.Added;
        }
    }
}
