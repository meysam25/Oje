using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.AccountService.Models.View;
using Oje.AccountService.Services.EContext;
using Oje.AccountService.Filters;

namespace Oje.AccountService.Services
{
    public class SectionService : ISectionService
    {
        readonly AccountDBContext db = null;
        readonly IRoleService RoleService = null;
        public SectionService(
                AccountDBContext db,
                IRoleService RoleService
            )
        {
            this.db = db;
            this.RoleService = RoleService;
        }

        public object GetListForTreeView(int? id)
        {
            var result = db.Sections.Select(t => new
            {
                id = t.Id,
                title = t.Title,
                childs = t.Controllers.Select(tt => new
                {
                    id = tt.Id,
                    title = tt.Title,
                    childs = tt.Actions.Select(ttt => new
                    {
                        id = ttt.Id,
                        title = ttt.Title + (ttt.IsMainMenuItem == true ? " (page) " : ""),
                        selected = ttt.RoleActions.Any(tttt => tttt.RoleId == id)
                    }).ToList()
                }).ToList()
            }).ToList();
            return result.Select(t => new
            {
                t.id,
                t.title,
                childs = t.childs.Select(tt => new
                {
                    tt.id,
                    tt.title,
                    childs = tt.childs.Select(ttt => new
                    {
                        ttt.id,
                        ttt.title,
                        ttt.selected,
                    }).ToList(),
                    selected = tt.childs.Where(ttt => ttt.selected == true).Count() == tt.childs.Count
                }).ToList()
            }).ToList()
            .Select(t => new
            {
                t.id,
                t.title,
                selected = t.childs.Where(tt => tt.selected == true).Count() == t.childs.Count,
                t.childs
            })
            .ToList()
            ;
        }

        public List<AccessTreeViewUser> GetListForTreeViewForUser(int? id, LoginUserVM loginUserVM, int? siteSettingId)
        {
            MyValidations.SiteSettingValidation(loginUserVM?.siteSettingId, siteSettingId);
            var loginUserMaxRoleValue = RoleService.GetRoleValueByUserId(loginUserVM.UserId, siteSettingId);
            var targetRoleMaxRoleValue = RoleService.GetRoleValueByRoleId(id.ToIntReturnZiro(), siteSettingId);
            if (loginUserMaxRoleValue <= targetRoleMaxRoleValue)
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);
            var loginUserRoleIds = RoleService.GetRoleIdsByUserId(loginUserVM?.UserId);
            if (loginUserRoleIds == null || loginUserRoleIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);
            var roleSiteSettingId = RoleService.GetRoleSiteSettignId(id);
            if (roleSiteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

            var result = db.Sections.Select(t => new
            {
                id = t.Id,
                title = t.Title,
                childs = t.Controllers.Select(tt => new
                {
                    id = tt.Id,
                    title = tt.Title,
                    childs = tt.Actions
                    .Where(ttt => ttt.RoleActions.Any(tttt => loginUserRoleIds.Contains(tttt.RoleId)))
                    .Select(ttt => new
                    {
                        id = ttt.Id,
                        title = ttt.Title + (ttt.IsMainMenuItem == true ? " (page) " : ""),
                        selected = ttt.RoleActions.Any(tttt => tttt.RoleId == id)
                    }).ToList()
                }).ToList()
            }).ToList();
            return result.Where(t => t.childs.Any(tt => tt.childs.Count > 0)).Select(t => new AccessTreeViewUser
            {
                id = t.id,
                title = t.title,
                childs = t.childs.Where(tt => tt.childs.Count > 0).Select(tt => new AccessTreeViewUser
                {
                    id = tt.id,
                    title = tt.title,
                    childs = tt.childs.Select(ttt => new AccessTreeViewUser
                    {
                        id = ttt.id,
                        title = ttt.title,
                        selected = ttt.selected,
                    }).ToList(),
                    selected = tt.childs.Where(ttt => ttt.selected == true).Count() == tt.childs.Count
                }).ToList()
            }).ToList()
            .Select(t => new AccessTreeViewUser
            {
                id = t.id,
                title = t.title,
                selected = t.childs.Where(tt => tt.selected == true).Count() == t.childs.Count,
                childs = t.childs
            })
            .ToList()
            ;
        }

        public List<Section> GetSideMenu(long? userId)
        {
            var listRoleIds = db.Users.Where(t => t.Id == userId).SelectMany(t => t.UserRoles).Select(t => t.RoleId).ToList();
            return db.Sections
                .Select(t => new Section
                {
                    Icon = t.Icon,
                    Title = t.Title,
                    Name = t.Name,
                    Controllers = t.Controllers.Select(tt => new Models.DB.Controller
                    {
                        Icon = tt.Icon,
                        Title = tt.Title,
                        Name = tt.Name,
                        HasFormGenerator = tt.HasFormGenerator,
                        Actions = tt.Actions.Where(ttt => ttt.IsMainMenuItem == true && ttt.RoleActions.Any(tttt => listRoleIds.Contains(tttt.RoleId))).Select(ttt => new Models.DB.Action
                        {
                            Icon = ttt.Icon,
                            Id = ttt.Id,
                            IsMainMenuItem = ttt.IsMainMenuItem,
                            Title = ttt.Title,
                            Name = ttt.Name
                        }).ToList()
                    }).Where(t => t.Actions.Any()).ToList()
                }).ToList().Where(t => t.Controllers.Any(tt => tt.Actions.Any())).ToList();
        }

        public object GetSideMenuAjax(long? userId)
        {
            if (userId <= 0)
                return new { };
            var listRoleIds = db.Users.Where(t => t.Id == userId).SelectMany(t => t.UserRoles).Select(t => t.RoleId).ToList();
            if (!listRoleIds.Any())
                return new { };
            return db.Sections
                .Select(t => new
                {
                    icon = t.Icon,
                    title = t.Title,
                    actions = t.Controllers
                    .SelectMany(tt => tt.Actions)
                    .Where(ttt => ttt.IsMainMenuItem == true && ttt.RoleActions.Any(tttt => listRoleIds.Contains(tttt.RoleId)))
                    .Select(ttt => new
                    {
                        icon = ttt.Icon,
                        title = ttt.Title,
                        url = ttt.Name
                    }).ToList()
                })
                .ToList()
                .Where(t => t.actions.Any())
                .ToList();
        }

        public ApiResult UpdateAccess(CreateUpdateRoleAccessVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (input.id.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Role_Is_Not_Valid, ApiResultErrorCode.ValidationError);

            var foundRole = db.Roles.Include(t => t.RoleActions).Where(t => t.Id == input.id).FirstOrDefault();
            if (foundRole == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foreach (var rSection in foundRole.RoleActions)
                db.Entry(rSection).State = EntityState.Deleted;

            if (input.a_3 != null && input.a_3.Count > 0)
            {
                List<int> allSectionIds = input.a_3.Select(t => t.Replace("a_", "").ToIntReturnZiro()).ToList();
                foreach (var rId in allSectionIds)
                {
                    if (rId > 0)
                        db.Entry(new RoleAction() { RoleId = input.id.Value, ActionId = rId }).State = EntityState.Added;
                }
            }

            db.SaveChanges();

            CustomeAuthorizeFilter.UserAccessCaches = new();
            GlobalConfig.siteMenuCache++;

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public object UpdateAccessForUser(CreateUpdateRoleAccessVM input, LoginUserVM loginUserVM, int? siteSettingId)
        {
            MyValidations.SiteSettingValidation(loginUserVM?.siteSettingId, siteSettingId);
            var loginUserMaxRoleValue = RoleService.GetRoleValueByUserId(loginUserVM.UserId, siteSettingId);
            var targetRoleMaxRoleValue = RoleService.GetRoleValueByRoleId(input.id.ToIntReturnZiro(), siteSettingId);
            if (loginUserMaxRoleValue <= targetRoleMaxRoleValue)
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);
            var loginUserRoleIds = RoleService.GetRoleIdsByUserId(loginUserVM?.UserId);
            if (loginUserRoleIds == null || loginUserRoleIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters, ApiResultErrorCode.ValidationError);
            if (input.id.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Role_Is_Not_Valid, ApiResultErrorCode.ValidationError);

            var foundRole = db.Roles.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).Include(t => t.RoleActions).FirstOrDefault();
            if (foundRole == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            var allValidActions = GetListForTreeViewForUser(input?.id, loginUserVM, siteSettingId);
            if (allValidActions == null || allValidActions.Count == 0)
                throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

            if (input.a_3 != null && input.a_3.Count > 0)
            {
                List<int> allSectionIds = input.a_3.Select(t => t.Replace("a_", "").ToIntReturnZiro()).ToList();
                foreach (var rId in allSectionIds)
                {
                    if (rId > 0)
                        if (!allValidActions.Any(t => t.childs != null && t.childs.Any(tt => tt.childs != null && tt.childs.Any(ttt => ttt.id == rId))))
                            throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);
                }
            }

            foreach (var rSection in foundRole.RoleActions)
                db.Entry(rSection).State = EntityState.Deleted;

            if (input.a_3 != null && input.a_3.Count > 0)
            {
                List<int> allSectionIds = input.a_3.Select(t => t.Replace("a_", "").ToIntReturnZiro()).ToList();
                foreach (var rId in allSectionIds)
                {
                    if (rId > 0)
                        db.Entry(new RoleAction() { RoleId = input.id.Value, ActionId = rId }).State = EntityState.Added;
                }
            }

            db.SaveChanges();

            return new ApiResult() { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetAttribute<DisplayAttribute>()?.Name };
        }

        public void UpdateModuals()
        {
            List<string> prevSections = db.Sections.Select(t => t.Name).ToList();
            List<string> prevSectionsControllers = db.Controllers.Select(t => t.Name).ToList();
            List<string> prevActions = db.Actions.Select(t => t.Name).ToList();

            List<string> currSections = new List<string>();
            List<string> currSectionsControllers = new List<string>();
            List<string> currActions = new List<string>();

            if (GlobalConfig.Moduals != null && GlobalConfig.Moduals.Count > 0)
            {
                foreach (var assembly in GlobalConfig.Moduals)
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.IsSubclassOf(typeof(Microsoft.AspNetCore.Mvc.Controller)))
                        {
                            if (type.FullName.ToLower().IndexOf("controllers") > 0)
                            {
                                AreaConfigAttribute controllerConfigAttribute = (AreaConfigAttribute)type.GetCustomAttributes(typeof(AreaConfigAttribute), true).FirstOrDefault();
                                AreaAttribute areaAttribute = (AreaAttribute)type.GetCustomAttributes(typeof(AreaAttribute), true).FirstOrDefault();
                                if (controllerConfigAttribute != null && areaAttribute != null)
                                {
                                    Section section = createModalByConfigAttribute(controllerConfigAttribute, areaAttribute);
                                    currSections.Add(section.Name);
                                    Models.DB.Controller controller = createParentModalByConfigAttribute(type.Name.Replace("Controller", ""), controllerConfigAttribute, section);
                                    currSectionsControllers.Add(controller.Name);
                                    var allMethods = type.GetMethods();
                                    foreach (var method in allMethods)
                                    {
                                        if (method.IsPublic)
                                        {
                                            string actionCode = "/" + areaAttribute.RouteValue + "/" + type.Name.Replace("Controller", "") + "/" + method.Name;
                                            if (actionCode.IndexOf("_set_") == -1 && actionCode.IndexOf("_get_") == -1 &&
                                                actionCode.IndexOf("_Dispose") == -1 && actionCode.IndexOf("_Equals") == -1 &&
                                                actionCode.IndexOf("_GetHashCode") == -1 && actionCode.IndexOf("_GetType") == -1 &&
                                                actionCode.IndexOf("_ToString") == -1 && actionCode.IndexOf("_ExecuteAsync") == -1 &&
                                                actionCode.IndexOf("_Validate") == -1)
                                            {
                                                AreaConfigAttribute methodConfigAttribute = (AreaConfigAttribute)method.GetCustomAttributes(typeof(AreaConfigAttribute), true).FirstOrDefault();
                                                if (methodConfigAttribute != null)
                                                {
                                                    createActionByConfigAttribute(methodConfigAttribute, actionCode, controller);
                                                    currActions.Add(actionCode);
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                removeWithAccessIfNotExisted(prevSections, prevSectionsControllers, prevActions, currSections, currSectionsControllers, currActions);
            }
        }

        void removeWithAccessIfNotExisted(List<string> prevSections, List<string> prevSectionsControllers, List<string> prevActions, List<string> currSections, List<string> currSectionsControllers, List<string> currActions)
        {
            foreach (var action in prevActions)
            {
                if (!currActions.Any(t => t == action))
                {
                    var foundAction = db.Actions
                        .Include(t => t.UserAdminLogConfigs)
                        .Include(t => t.UserAdminLogs)
                        .Include(t => t.RoleActions)
                        .Include(t => t.DashboardSections)
                        .Where(t => t.Name == action)
                        .FirstOrDefault();
                    if (foundAction != null)
                    {
                        if (foundAction.UserAdminLogConfigs != null)
                            foreach (var temp1 in foundAction.UserAdminLogConfigs)
                                db.Entry(temp1).State = EntityState.Deleted;
                        if (foundAction.UserAdminLogs != null)
                            foreach (var temp1 in foundAction.UserAdminLogs)
                                db.Entry(temp1).State = EntityState.Deleted;
                        if (foundAction.RoleActions != null)
                            foreach (var temp1 in foundAction.RoleActions)
                                db.Entry(temp1).State = EntityState.Deleted;
                        if (foundAction.DashboardSections != null)
                            foreach (var temp1 in foundAction.DashboardSections)
                                db.Entry(temp1).State = EntityState.Deleted;
                        db.Entry(foundAction).State = EntityState.Deleted;
                    }
                }
            }

            foreach (var controllerX in prevSectionsControllers)
            {
                if (!currSectionsControllers.Any(t => t == controllerX))
                {
                    var foundController = db.Controllers.Include(t => t.Actions).ThenInclude(t => t.RoleActions).Where(t => t.Name == controllerX).FirstOrDefault();
                    if (foundController != null)
                    {
                        if (foundController.Actions != null)
                            foreach (var tempAction in foundController.Actions)
                            {
                                if (tempAction.RoleActions != null)
                                    foreach (var temp1 in tempAction.RoleActions)
                                        db.Entry(temp1).State = EntityState.Deleted;
                                db.Entry(tempAction).State = EntityState.Deleted;
                            }

                        db.Entry(foundController).State = EntityState.Deleted;
                    }
                }
            }

            foreach (var sectionX in prevSections)
            {
                if (!currSections.Any(t => t == sectionX))
                {
                    var foundSection = db.Sections.Include(t => t.Controllers).ThenInclude(t => t.Actions).ThenInclude(t => t.RoleActions).Where(t => t.Name == sectionX).FirstOrDefault();
                    if (foundSection != null)
                    {
                        if (foundSection.Controllers != null)
                            foreach (var foundController in foundSection.Controllers)
                            {
                                if (foundController.Actions != null)
                                    foreach (var tempAction in foundController.Actions)
                                    {
                                        if (tempAction.RoleActions != null)
                                            foreach (var temp1 in tempAction.RoleActions)
                                                db.Entry(temp1).State = EntityState.Deleted;
                                        db.Entry(tempAction).State = EntityState.Deleted;
                                    }
                                db.Entry(foundController).State = EntityState.Deleted;
                            }
                        db.Entry(foundSection).State = EntityState.Deleted;
                    }
                }
            }

            db.SaveChanges();
        }

        void createActionByConfigAttribute(AreaConfigAttribute methodConfigAttribute, string actionCode, Models.DB.Controller modulSection)
        {
            Models.DB.Action foundAction = db.Actions.Where(t => t.Name == actionCode).FirstOrDefault();
            if (foundAction == null)
            {
                foundAction = new Models.DB.Action() { Name = actionCode, ControllerId = modulSection.Id };
                db.Entry(foundAction).State = EntityState.Added;
            }

            foundAction.Icon = methodConfigAttribute.Icon;
            foundAction.Title = methodConfigAttribute.Title;
            foundAction.IsMainMenuItem = methodConfigAttribute.IsMainMenuItem;

            db.SaveChanges();
        }

        Section createModalByConfigAttribute(AreaConfigAttribute controllerConfigAttribute, AreaAttribute areaAttribute)
        {
            Section addEditItem = db.Sections.Where(t => t.Name == areaAttribute.RouteValue).FirstOrDefault();

            if (addEditItem == null)
            {
                addEditItem = new Section();
                addEditItem.Name = areaAttribute.RouteValue;

                db.Entry(addEditItem).State = EntityState.Added;
            }

            addEditItem.Icon = controllerConfigAttribute.Icon;
            addEditItem.Title = controllerConfigAttribute.ModualTitle;

            db.SaveChanges();

            return addEditItem;
        }

        Models.DB.Controller createParentModalByConfigAttribute(string controllerName, AreaConfigAttribute controllerConfigAttribute, Section Modul)
        {
            string targetName = Modul.Name + "_" + controllerName;
            Models.DB.Controller newParentModalSection = db.Controllers.Where(t => t.Name == targetName).FirstOrDefault();
            if (newParentModalSection == null)
            {
                newParentModalSection = new Models.DB.Controller();
                newParentModalSection.Name = targetName;
                newParentModalSection.SectionId = Modul.Id;
                db.Entry(newParentModalSection).State = EntityState.Added;
            }
            newParentModalSection.Title = controllerConfigAttribute.Title;
            newParentModalSection.Icon = controllerConfigAttribute.Icon;
            newParentModalSection.HasFormGenerator = controllerConfigAttribute.HasFormGenerator;
            if (string.IsNullOrEmpty(newParentModalSection.Icon))
                newParentModalSection.Icon = "";

            db.SaveChanges();

            return newParentModalSection;
        }

        public object GetSelect2List(Select2SearchVM searchInput)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.Sections.OrderByDescending(t => t.Id).AsQueryable();
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => (t.Title).Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }

        public List<SiteMenueVM> GetSideMenuWidthCategory(long? userId)
        {
            var listRoleIds = db.Users.Where(t => t.Id == userId).SelectMany(t => t.UserRoles).Select(t => t.RoleId).ToList();
            List<SiteMenueVM> result = new List<SiteMenueVM>();

            var groupResult = db.Sections
                .Select(t => new
                {
                    catOrder = t.SectionCategorySections.Select(tt => tt.SectionCategory.Order).FirstOrDefault(),
                    catTitle = t.SectionCategorySections.Select(tt => tt.SectionCategory.Title).FirstOrDefault(),
                    catIcon = t.SectionCategorySections.Select(tt => tt.SectionCategory.Icon).FirstOrDefault(),
                    catIsActive = t.SectionCategorySections.Select(tt => tt.SectionCategory.IsActive).FirstOrDefault(),
                    Icon = t.Icon,
                    Title = t.Title,
                    Controllers = t.Controllers.Select(tt => new
                    {
                        catOrder = tt.ControllerCategoryControllers.Select(ttt => ttt.ControllerCategory.Order).FirstOrDefault(),
                        catTitle = tt.ControllerCategoryControllers.Select(ttt => ttt.ControllerCategory.Title).FirstOrDefault(),
                        catIcon = tt.ControllerCategoryControllers.Select(ttt => ttt.ControllerCategory.Icon).FirstOrDefault(),
                        catIsActive = tt.ControllerCategoryControllers.Select(ttt => ttt.ControllerCategory.IsActive).FirstOrDefault(),
                        Icon = tt.Icon,
                        Title = tt.Title,
                        Actions = tt.Actions.Where(ttt => ttt.IsMainMenuItem == true && ttt.RoleActions.Any(tttt => listRoleIds.Contains(tttt.RoleId))).Select(ttt => new
                        {
                            Icon = ttt.Icon,
                            Id = ttt.Id,
                            IsMainMenuItem = ttt.IsMainMenuItem,
                            Title = ttt.Title,
                            Name = ttt.Name
                        }).ToList()
                    }).Where(t => t.Actions.Any()).ToList()
                }).ToList().Where(t => t.Controllers.Any(tt => tt.Actions.Any())).ToList();

            var groupBySectionCat = groupResult.OrderBy(t => t.catOrder == 0 ? 999 : t.catOrder).GroupBy(t => new { catTitle = t.catIsActive == false ? null : t.catTitle, catIcon = t.catIsActive == false ? null : t.catIcon }).ToList();

            foreach (var group in groupBySectionCat)
            {

                if (!string.IsNullOrEmpty(group.Key.catTitle) && !string.IsNullOrEmpty(group.Key.catIcon))
                {
                    SiteMenueVM newItem = new SiteMenueVM()
                    {
                        icon = group.Key.catIcon,
                        title = group.Key.catTitle
                    };

                    List<SiteMenueVM> childList = new List<SiteMenueVM>();
                    foreach (var modal in group.ToList())
                    {
                        SiteMenueVM newChil1Item = new SiteMenueVM()
                        {
                            icon = modal.Icon,
                            title = modal.Title
                        };

                        var controllerGroupItems = modal.Controllers.OrderBy(t => t.catOrder == 0 ? 999 : t.catOrder).GroupBy(t => new { catTitle = t.catIsActive == false ? null : t.catTitle, catIcon = t.catIsActive == false ? null : t.catIcon }).ToList();
                        List<SiteMenueVM> controllerChildList = new List<SiteMenueVM>();
                        foreach (var controllerGroup in controllerGroupItems)
                        {
                            if (!string.IsNullOrEmpty(controllerGroup.Key.catTitle) && !string.IsNullOrEmpty(controllerGroup.Key.catIcon))
                            {
                                SiteMenueVM newCatItem = new SiteMenueVM()
                                {
                                    title = controllerGroup.Key.catTitle,
                                    icon = controllerGroup.Key.catIcon
                                };
                                foreach (var allCtrl in controllerGroup.ToList())
                                {
                                    newCatItem.childs.Add(new SiteMenueVM()
                                    {
                                        title = allCtrl.Actions.FirstOrDefault()?.Title,
                                        icon = allCtrl.Actions.FirstOrDefault()?.Icon,
                                        url = allCtrl.Actions.FirstOrDefault()?.Name
                                    });
                                }
                                controllerChildList.Add(newCatItem);
                            }
                            else
                            {
                                foreach (var controllerItem in controllerGroup.ToList())
                                {
                                    SiteMenueVM newCatItem = new SiteMenueVM()
                                    {
                                        title = controllerItem.Actions.FirstOrDefault()?.Title,
                                        icon = controllerItem.Actions.FirstOrDefault()?.Icon,
                                        url = controllerItem.Actions.FirstOrDefault()?.Name
                                    };

                                    controllerChildList.Add(newCatItem);
                                }
                            }
                        }
                        newChil1Item.childs = controllerChildList;
                        childList.Add(newChil1Item);
                    }
                    newItem.childs = childList;

                    result.Add(newItem);
                }
                else
                {
                    foreach (var item in group.ToList())
                    {
                        SiteMenueVM newChil1Item = new SiteMenueVM()
                        {
                            icon = item.Icon,
                            title = item.Title
                        };

                        List<SiteMenueVM> controllerList = new List<SiteMenueVM>();

                        var groupControllers = item.Controllers.OrderBy(t => t.catOrder == 0 ? 999 : t.catOrder).GroupBy(t => new { catTitle = t.catIsActive == false ? null : t.catTitle, catIcon = t.catIsActive == false ? null : t.catIcon }).ToList();
                        foreach (var gITem in groupControllers)
                        {
                            if (!string.IsNullOrEmpty(gITem.Key.catTitle) && !string.IsNullOrEmpty(gITem.Key.catIcon))
                            {
                                SiteMenueVM tempX = new SiteMenueVM()
                                {
                                    title = gITem.Key.catTitle,
                                    icon = gITem.Key.catIcon
                                };
                                foreach (var chilCtrlTempX in gITem.ToList())
                                {
                                    tempX.childs.Add(new SiteMenueVM()
                                    {
                                        title = chilCtrlTempX.Actions.FirstOrDefault()?.Title,
                                        icon = chilCtrlTempX.Actions.FirstOrDefault()?.Icon,
                                        url = chilCtrlTempX.Actions.FirstOrDefault()?.Name
                                    });
                                }
                                controllerList.Add(tempX);
                            }
                            else
                            {
                                foreach (var tempX2 in gITem.ToList())
                                {
                                    SiteMenueVM tempX = new SiteMenueVM()
                                    {
                                        title = tempX2.Actions.FirstOrDefault()?.Title,
                                        icon = tempX2.Actions.FirstOrDefault()?.Icon,
                                        url = tempX2.Actions.FirstOrDefault()?.Name
                                    };
                                    controllerList.Add(tempX);
                                }
                            }
                        }

                        newChil1Item.childs = controllerList;
                        result.Add(newChil1Item);
                    }
                }
            }

            return result;
        }
    }
}
