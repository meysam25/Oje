using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models.PageForms;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Services
{
    public class ProposalFilledFormValueManager : IProposalFilledFormValueManager
    {
        readonly ProposalFormDBContext db = null;
        readonly IProposalFilledFormKeyManager ProposalFilledFormKeyManager = null;
        public ProposalFilledFormValueManager(ProposalFormDBContext db, IProposalFilledFormKeyManager ProposalFilledFormKeyManager)
        {
            this.db = db;
            this.ProposalFilledFormKeyManager = ProposalFilledFormKeyManager;
        }

        public void CreateByJsonConfig(PageForm ppfObj, long proposalFilledFormId, IFormCollection form)
        {
            if (ppfObj != null && form != null && proposalFilledFormId > 0)
            {
                var allCtrls = ppfObj.GetAllListOf<ctrl>();
                foreach (ctrl ctrl in allCtrls)
                {
                    if (ctrl.isCtrlVisible(form, allCtrls) && ctrl.disabled != true)
                    {
                        if (
                            ctrl.type == Infrastructure.Enums.ctrlType.text ||
                            ctrl.type == Infrastructure.Enums.ctrlType.dropDown2 ||
                            ctrl.type == Infrastructure.Enums.ctrlType.dropDown
                        )
                        {
                            string currValue = "";
                            if (ctrl.type == Infrastructure.Enums.ctrlType.text)
                                currValue = form.GetStringIfExist(ctrl.name);
                            else if (ctrl.type == Infrastructure.Enums.ctrlType.dropDown || ctrl.type == Infrastructure.Enums.ctrlType.dropDown2)
                                currValue = ctrl.defV;
                            if (!string.IsNullOrEmpty(currValue))
                            {
                                if (currValue.Length > 4000)
                                    throw BException.GenerateNewException(String.Format(BMessages.X_Length_Can_Not_Be_More_Then_4000.GetEnumDisplayName(), ctrl.label));
                                int keyId = ProposalFilledFormKeyManager.CreateIfNeeded(ctrl.name);
                                if (keyId > 0)
                                {
                                    db.Entry(new ProposalFilledFormValue()
                                    {
                                        ProposalFilledFormId = proposalFilledFormId,
                                        ProposalFilledFormKeyId = keyId,
                                        Value = currValue
                                    }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                                }
                            }

                        }
                    }
                }
                db.SaveChanges();
            }
        }
    }
}
