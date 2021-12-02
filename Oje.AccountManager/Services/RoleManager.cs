using Oje.AccountManager.Interfaces;
using Oje.AccountManager.Models;
using Oje.AccountManager.Models.DB;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.AccountManager.Services.EContext;
using Oje.AccountManager.Models.View;

namespace Oje.AccountManager.Services
{
    public class RoleManager : IRoleManager
    {
        readonly AccountDBContext db = null;
        readonly IProposalFormManager proposalFormManager = null;
        const int minusRoleValue = 100;
        public RoleManager(AccountDBContext db, IProposalFormManager proposalFormManager)
        {
            this.db = db;
            this.proposalFormManager = proposalFormManager;
        }

        public ApiResult Create(CreateUpdateRoleVM input)
        {
            CreateValidation(input);

            var newItem = new Role();

            newItem.Name = input.name;
            newItem.Title = input.title;
            newItem.Value = input.value.Value;
            newItem.DisabledOnlyMyStuff = input.disabledOnlyMyStuff.ToBooleanReturnFalse();
            newItem.Type = input.type;
            newItem.SiteSettingId = input.sitesettingId;

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.formIds != null && input.formIds.Count > 0)
            {
                foreach (var formId in input.formIds)
                    db.Entry(new RoleProposalForm() { ProposalFormId = formId, RoleId = newItem.Id }).State = EntityState.Added;

                db.SaveChanges();
            }


            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        private void CreateValidation(CreateUpdateRoleVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.name))
                throw BException.GenerateNewException(BMessages.Please_Enter_Name, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title, ApiResultErrorCode.ValidationError);
            if (db.Roles.Any(t => t.Name == input.name && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Name, ApiResultErrorCode.ValidationError);
            if (db.Roles.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title, ApiResultErrorCode.ValidationError);
            if (input.value.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Value, ApiResultErrorCode.ValidationError);
            if (input.formIds != null && input.formIds.Count > 0)
                foreach (var formId in input.formIds)
                    if (!proposalFormManager.Exist(input.sitesettingId, formId))
                        throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);
        }

        public GridResultVM<RoleGridResultVM> GetList(RoleGridFilters searchInput)
        {
            if (searchInput == null)
                searchInput = new RoleGridFilters();

            var qureResult = db.Roles.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.name))
                qureResult = qureResult.Where(t => t.Name.Contains(searchInput.name));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.siteSetting.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.SiteSettingId == searchInput.siteSetting);
            if (searchInput.value.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.Value == searchInput.value);

            int row = searchInput.skip;

            return new GridResultVM<RoleGridResultVM>
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    name = t.Name,
                    title = t.Title,
                    userCount = t.UserRoles.Count(),
                    siteSetting = t.SiteSetting.Title,
                    value = t.Value
                })
                .ToList()
                .Select(t => new RoleGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    name = t.name,
                    title = t.title,
                    siteSetting = t.siteSetting,
                    value = t.value == 0 ? "0" : t.value.ToString("###,###"),
                    userCount = t.userCount
                })
                .ToList()
            };
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.Roles.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);
            var countUser = db.UserRoles.Where(t => t.RoleId == id).Count();
            if (countUser > 0)
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted, ApiResultErrorCode.ValidationError);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public object GetById(int? id)
        {
            return db.Roles.Where(t => t.Id == id).Select(t => new
            {
                id = t.Id,
                name = t.Name,
                title = t.Title,
                value = t.Value,
                type = t.Type,
                disabledOnlyMyStuff = t.DisabledOnlyMyStuff == null ? false : t.DisabledOnlyMyStuff,
                sitesettingId = t.SiteSettingId,
                formIds = t.RoleProposalForms.Select(tt => new { id = tt.ProposalFormId, title = tt.ProposalForm.Title }).ToList()
            }).FirstOrDefault();
        }

        public ApiResult Update(CreateUpdateRoleVM input)
        {
            CreateValidation(input);

            var foundItem = db.Roles.Include(t => t.RoleProposalForms).Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.RoleProposalForms.Count > 0)
                foreach (var form in foundItem.RoleProposalForms)
                    db.Entry(form).State = EntityState.Deleted;

            foundItem.Title = input.title;
            foundItem.Name = input.name;
            foundItem.Value = input.value.Value;
            foundItem.DisabledOnlyMyStuff = input.disabledOnlyMyStuff.ToBooleanReturnFalse();
            foundItem.Type = input.type;
            foundItem.SiteSettingId = input.sitesettingId;

            if (input.formIds != null && input.formIds.Count > 0)
                foreach (var formId in input.formIds)
                    db.Entry(new RoleProposalForm() { ProposalFormId = formId, RoleId = foundItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }


        public object GetRoleLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };
            result.AddRange(db.Roles.Select(t => new
            {
                id = t.Id,
                title = t.Title
            }).ToList());

            return result;
        }

        public ApiResult CreateUser(CreateUpdateUserRoleVM input, LoginUserVM loginUserVM, int? siteSettingId)
        {
            createUserValidation(input, loginUserVM, siteSettingId);

            var newItem = new Role();

            newItem.Name = input.name;
            newItem.Title = input.title;
            newItem.Value = GetRoleValueByUserId(loginUserVM.UserId, siteSettingId);
            newItem.DisabledOnlyMyStuff = input.disabledOnlyMyStuff.ToBooleanReturnFalse();
            newItem.Type = input.type;
            newItem.SiteSettingId = loginUserVM.siteSettingId.Value;

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.formIds != null && input.formIds.Count > 0)
            {
                foreach (var formId in input.formIds)
                    db.Entry(new RoleProposalForm() { ProposalFormId = formId, RoleId = newItem.Id }).State = EntityState.Added;

                db.SaveChanges();
            }


            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public long GetRoleValueByUserId(long UserId, int? siteSettingId)
        {
            try
            {
                return db.UserRoles
                    .Where(t => t.UserId == UserId)
                    .Select(t => t.Role)
                    .Where(t => t.SiteSettingId == null || t.SiteSettingId == siteSettingId)
                    .Select(t => t.Value).Max() - minusRoleValue;
            }
            catch
            {
                return int.MinValue;
            }
        }

        private void createUserValidation(CreateUpdateUserRoleVM input, LoginUserVM loginUserVM, int? siteSettingId)
        {
            MyValidations.SiteSettingValidation(loginUserVM?.siteSettingId, siteSettingId);
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.name))
                throw BException.GenerateNewException(BMessages.Please_Enter_Name, ApiResultErrorCode.ValidationError);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title, ApiResultErrorCode.ValidationError);
            if (db.Roles.Any(t => t.Name == input.name && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Name, ApiResultErrorCode.ValidationError);
            if (db.Roles.Any(t => t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Title, ApiResultErrorCode.ValidationError);
            if (input.formIds != null && input.formIds.Count > 0)
                foreach (var formId in input.formIds)
                    if (!proposalFormManager.Exist(siteSettingId, formId))
                        throw BException.GenerateNewException(BMessages.ProposalForm_Not_Founded);

        }

        public ApiResult DeleteUser(int? id, LoginUserVM loginUserVM, int? siteSettingId)
        {
            MyValidations.SiteSettingValidation(loginUserVM?.siteSettingId, siteSettingId);

            var roleValue = GetRoleValueByUserId(loginUserVM.UserId, siteSettingId);

            var foundItem = db.Roles.Where(t => t.Id == id && (t.SiteSettingId == siteSettingId || t.SiteSettingId == null) && t.Value <= roleValue).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            if (foundItem.SiteSettingId == null)
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted);

            var countUser = db.UserRoles.Where(t => t.RoleId == id).Count();
            if (countUser > 0)
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Deleted, ApiResultErrorCode.ValidationError);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public object GetByIdUser(int? id, LoginUserVM loginUserVM, int? siteSettingId)
        {
            MyValidations.SiteSettingValidation(loginUserVM?.siteSettingId, siteSettingId);

            var roleValue = GetRoleValueByUserId(loginUserVM.UserId, siteSettingId);

            return db.Roles
                .Where(t => t.Id == id && (t.SiteSettingId == loginUserVM.siteSettingId || t.SiteSettingId == null) && t.Value <= roleValue)
                .Select(t => new 
                {
                    id = t.Id,
                    name = t.Name,
                    title = t.Title,
                    type = t.Type,
                    disabledOnlyMyStuff = t.DisabledOnlyMyStuff == null ? false : t.DisabledOnlyMyStuff,
                    formIds = t.RoleProposalForms.Select(tt => new { id = tt.ProposalFormId, title = tt.ProposalForm.Title }).ToList()
                }).FirstOrDefault();
        }

        public ApiResult UpdateUser(CreateUpdateUserRoleVM input, LoginUserVM loginUserVM, int? siteSettingId)
        {
            createUserValidation(input, loginUserVM, siteSettingId);

            if (loginUserVM?.siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.User_SiteSetting_Can_Not_Be_Founded);

            var roleValue = GetRoleValueByUserId(loginUserVM.UserId, siteSettingId);

            var foundItem = db.Roles.Include(t => t.RoleProposalForms).Where(t => t.Id == input.id && (t.SiteSettingId == siteSettingId || t.SiteSettingId == null) && t.Value <= roleValue).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);
            if (foundItem.SiteSettingId == null)
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

            if (foundItem.RoleProposalForms.Count > 0)
                foreach (var form in foundItem.RoleProposalForms)
                    db.Entry(form).State = EntityState.Deleted;

            foundItem.Title = input.title;
            foundItem.Name = input.name;
            foundItem.DisabledOnlyMyStuff = input.disabledOnlyMyStuff.ToBooleanReturnFalse();
            foundItem.Type = input.type;

            if (input.formIds != null && input.formIds.Count > 0)
                foreach (var formId in input.formIds)
                    db.Entry(new RoleProposalForm() { ProposalFormId = formId, RoleId = foundItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public GridResultVM<RoleUserGridResultVM> GetListUser(RoleUserGridFilters searchInput, LoginUserVM loginUserVM, int? siteSettingId)
        {
            MyValidations.SiteSettingValidation(loginUserVM?.siteSettingId, siteSettingId);

            var roleValue = GetRoleValueByUserId(loginUserVM.UserId, siteSettingId);

            if (searchInput == null)
                searchInput = new RoleUserGridFilters();

            var qureResult = db.Roles.Where(t => (t.SiteSettingId == loginUserVM.siteSettingId || t.SiteSettingId == null) && t.Value <= roleValue);

            if (!string.IsNullOrEmpty(searchInput.name))
                qureResult = qureResult.Where(t => t.Name.Contains(searchInput.name));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));

            int row = searchInput.skip;

            return new GridResultVM<RoleUserGridResultVM>
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    name = t.Name,
                    title = t.Title,
                    t.SiteSettingId
                })
                .ToList()
                .Select(t => new RoleUserGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    name = t.name,
                    title = t.title,
                    owner = t.SiteSettingId == null ? BMessages.Admin.GetAttribute<DisplayAttribute>()?.Name : BMessages.You.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public long GetRoleValueByRoleId(int roleId, int? siteSettingId)
        {
            try
            {
                return db.Roles
                    .Where(t => t.Id == roleId && (t.SiteSettingId == null || t.SiteSettingId == siteSettingId))
                    .Select(t => t.Value).FirstOrDefault() - minusRoleValue;
            }
            catch
            {
                return int.MinValue;
            }
        }

        public List<int> GetRoleIdsByUserId(long? userId)
        {
            return db.UserRoles.Where(t => t.UserId == userId).Select(t => t.RoleId).ToList();
        }

        public int? GetRoleSiteSettignId(int? id)
        {
            return db.Roles.Where(t => t.Id == id).Select(t => t.SiteSettingId).FirstOrDefault();
        }

        public object GetRoleLightListForUser(LoginUserVM loginUserVM, int? siteSettingId)
        {
            MyValidations.SiteSettingValidation(loginUserVM?.siteSettingId, siteSettingId);

            var roleValue = GetRoleValueByUserId(loginUserVM.UserId, siteSettingId);

            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };
            result.AddRange(db.Roles
                                .Where(t => (t.SiteSettingId == siteSettingId || t.SiteSetting == null) && t.Value <= roleValue)
                                .Select(t => new
                                {
                                    id = t.Id,
                                    title = t.Title
                                }).ToList()
            );

            return result;
        }

        public int CreateOrGetRole(string title, string name, int value)
        {
            if (string.IsNullOrEmpty(title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (string.IsNullOrEmpty(name))
                throw BException.GenerateNewException(BMessages.Please_Enter_Name);
            if (value <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Value);

            var foundItem = db.Roles.Where(t => t.Name == name && t.SiteSettingId == null).FirstOrDefault();
            if (foundItem != null)
                return foundItem.Id;

            foundItem = new Role()
            {
                Title = title,
                Name = name,
                Value = value
            };
            db.Entry(foundItem).State = EntityState.Added;
            db.SaveChanges();

            return foundItem.Id;
        }
    }
}
