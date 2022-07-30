using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Models.View;
using Oje.AccountService.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;

namespace Oje.AccountService.Services
{
    public class DashboardSectionService : IDashboardSectionService
    {
        readonly AccountDBContext db = null;
        public DashboardSectionService
            (
                AccountDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(DashboardSectionCreateUpdateVM input)
        {
            createUpdateValidation(input);

            DashboardSection newItem = new DashboardSection()
            {
                ActionId = input.actionId.ToLongReturnZiro(),
                Class = input.@class,
                RoleId = input.pKey.ToIntReturnZiro(),
                Type = input.type.Value,
                DashboardSectionCategoryId = input.catId,
                Order = input.order,
                Color = input.color
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.types != null && input.types.Any())
            {
                foreach (var nt in input.types)
                    db.Entry(new DashboardSectionUserNotificationType()
                    {
                        DashboardSectionId = newItem.Id,
                        Type = nt
                    }).State = EntityState.Added;

                db.SaveChanges();
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(DashboardSectionCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.@class))
                throw BException.GenerateNewException(BMessages.Please_Enter_Class);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (input.actionId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Section);
            if (input.pKey.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_One_Or_More_Role);
            if (db.DashboardSections.Any(t => t.Id != input.id && t.RoleId == input.pKey && t.ActionId == input.actionId && t.Type == input.type))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            DashboardSection foundItem = db.DashboardSections.Include(t => t.DashboardSectionUserNotificationTypes).Where(t => t.Id == id).FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.DashboardSectionUserNotificationTypes != null && foundItem.DashboardSectionUserNotificationTypes.Any())
                for (var i = 0; i < foundItem.DashboardSectionUserNotificationTypes.Count; i++)
                    db.Entry(foundItem.DashboardSectionUserNotificationTypes[i]).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.DashboardSections
                .Where(t => t.Id == id)
                .OrderByDescending(t => t.Id)
                .Select(t => new
                {
                    id = t.Id,
                    @class = t.Class,
                    type = t.Type,
                    actionId = t.ActionId,
                    actionId_Title = t.Action.Title,
                    catId = t.DashboardSectionCategoryId,
                    order = t.Order,
                    color = t.Color,
                    types = t.DashboardSectionUserNotificationTypes.Select(tt => tt.Type).ToList()
                })
                .Take(1)
                .Select(t => new
                {
                    t.id,
                    t.@class,
                    type = (int)t.type,
                    t.actionId,
                    t.actionId_Title,
                    t.catId,
                    t.order,
                    t.color,
                    t.types
                })
                .FirstOrDefault();
        }

        public GridResultVM<DashboardSectionGridResultVM> GetList(DashboardSectionGridFilters searchInput)
        {
            if (searchInput == null)
                searchInput = new DashboardSectionGridFilters();

            var qureResult = db.DashboardSections.AsQueryable();

            if (searchInput.pKey.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.RoleId == searchInput.pKey);

            if (!string.IsNullOrEmpty(searchInput.action))
                qureResult = qureResult.Where(t => t.Action.Title.Contains(searchInput.action));
            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.type);

            int row = searchInput.skip;

            return new GridResultVM<DashboardSectionGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    type = t.Type,
                    action = t.Action.Title
                })
                .ToList()
                .Select(t => new DashboardSectionGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    type = t.type.GetEnumDisplayName(),
                    action = t.action
                })
                .ToList()
            };
        }

        public ApiResult Update(DashboardSectionCreateUpdateVM input)
        {
            createUpdateValidation(input);

            DashboardSection foundItem = db.DashboardSections.Include(t => t.DashboardSectionUserNotificationTypes).Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (foundItem.DashboardSectionUserNotificationTypes != null && foundItem.DashboardSectionUserNotificationTypes.Any())
                for (var i = 0; i < foundItem.DashboardSectionUserNotificationTypes.Count; i++)
                    db.Entry(foundItem.DashboardSectionUserNotificationTypes[i]).State = EntityState.Deleted;

            foundItem.ActionId = input.actionId.ToLongReturnZiro();
            foundItem.Class = input.@class;
            foundItem.Type = input.type.Value;
            foundItem.DashboardSectionCategoryId = input.catId;
            foundItem.Order = input.order;
            foundItem.Color = input.color;

            if (input.types != null && input.types.Any())
            {
                foreach (var nt in input.types)
                    db.Entry(new DashboardSectionUserNotificationType()
                    {
                        DashboardSectionId = foundItem.Id,
                        Type = nt
                    }).State = EntityState.Added;

                db.SaveChanges();
            }

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public string GetDashboardConfigByUserId(int? siteSettingId, long? userId)
        {
            var allConfigs = db.Users
                .Where(t => t.Id == userId && t.SiteSettingId == siteSettingId)
                .SelectMany(t => t.UserRoles)
                .Select(t => t.Role)
                .SelectMany(t => t.DashboardSections)
                .Select(t => new
                {
                    id = t.Id,
                    type = t.Type,
                    parentCL = t.Class,
                    url = t.Action.Name,
                    label = t.Action.Title,
                    icon = t.Action.Controller.Actions.Where(tt => tt.IsMainMenuItem == true).Select(tt => tt.Icon).FirstOrDefault(),
                    gId = t.DashboardSectionCategoryId,
                    gTitle = t.DashboardSectionCategory.Title,
                    gType = t.DashboardSectionCategory.Type,
                    gOrder = t.DashboardSectionCategory.Order,
                    gcssClass = t.DashboardSectionCategory.Css,
                    order = t.Order,
                    color = t.Color,
                    nTypes = t.DashboardSectionUserNotificationTypes.Select(tt => tt.Type.ToString()).ToList()
                })
                .ToList();

            var listPanels = new List<object>();

            var groupConfigs = allConfigs.GroupBy(t => new { t.gTitle, t.gType, t.gOrder }).OrderBy(t => t.Key.gOrder).ToList();
            foreach (var groupConfig in groupConfigs)
            {
                listPanels.Add(new
                {
                    @class = groupConfig.FirstOrDefault()?.gcssClass,
                    title = groupConfig.Key.gTitle,
                    type = groupConfig.Key.gType != null ? groupConfig.Key.gType.Value.ToString() : null,
                    ctrls = groupConfig.OrderBy(tt => tt.order)
                    .Select(t => new
                    {
                        id = "content_" + t.id,
                        type = t.type.ToString(),
                        t.parentCL,
                        label = t.label.Replace("تنظیمات ", "").Replace("صفحه ", ""),
                        url = t.type == Infrastructure.Enums.DashboardSectionType.Content || t.type == Infrastructure.Enums.DashboardSectionType.TabContent || t.type == Infrastructure.Enums.DashboardSectionType.Tab || t.type == Infrastructure.Enums.DashboardSectionType.TabContentDynamicContent ? t.url : putIndexAtEntOfUrl(t.url),
                        icon = t.icon,
                        t.color,
                        t.nTypes
                    })
                    .ToList()
                });
            }

            return
                JsonConvert.SerializeObject(new
                {
                    panels = listPanels
                });
        }

        private string putIndexAtEntOfUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string[] arrParameters = url.Split('/');
                string result = "";
                for (var i = 0; i < arrParameters.Length - 1; i++)
                {
                    result += arrParameters[i].ToString() + "/";
                }
                result += "Index";

                return result;
            }

            return url;
        }
    }
}
