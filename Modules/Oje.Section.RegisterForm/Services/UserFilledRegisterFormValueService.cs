using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models.PageForms;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Services
{
    public class UserFilledRegisterFormValueService: IUserFilledRegisterFormValueService
    {
        readonly RegisterFormDBContext db = null;
        readonly IUserFilledRegisterFormKeyService UserFilledRegisterFormKeyService = null;
        public UserFilledRegisterFormValueService
            (
                RegisterFormDBContext db,
                IUserFilledRegisterFormKeyService UserFilledRegisterFormKeyService
            )
        {
            this.db = db;
            this.UserFilledRegisterFormKeyService = UserFilledRegisterFormKeyService;
        }

        public void CreateByJsonConfig(PageForm jsonOpject, long formId, IFormCollection requestForm)
        {
            if (jsonOpject != null && requestForm != null && formId > 0)
            {
                var allCtrls = jsonOpject.GetAllListOf<ctrl>();
                foreach (ctrl ctrl in allCtrls)
                {
                    if (ctrl.isCtrlVisible(requestForm, allCtrls))
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
                                    currValue = requestForm.GetStringIfExist(ctrl.name);
                            }
                            else if (ctrl.type == ctrlType.dropDown || ctrl.type == ctrlType.dropDown2)
                                currValue = ctrl.defV;
                            if (!string.IsNullOrEmpty(currValue))
                            {
                                if (currValue.Length > 4000)
                                    throw BException.GenerateNewException(String.Format(BMessages.X_Length_Can_Not_Be_More_Then_4000.GetEnumDisplayName(), ctrl.label));
                                long keyId = UserFilledRegisterFormKeyService.CreateIfNeeded(ctrl.name);
                                if (keyId > 0)
                                    addNewRow(formId, keyId, currValue);
                            }
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        private void addNewRow(long formId, long keyId, string currValue)
        {
            db.Entry(new UserFilledRegisterFormValue()
            {
                UserFilledRegisterFormId = formId,
                UserFilledRegisterFormKeyId = keyId,
                Value = currValue
            }).State = EntityState.Added;
            db.SaveChanges();
        }
    }
}
