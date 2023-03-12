using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Models.View;
using Oje.ValidatedSignature.Services.EContext;

namespace Oje.ValidatedSignature.Services
{
    public class UserService : IUserService
    {
        readonly ValidatedSignatureDBContext db = null;
        public UserService
            (
                ValidatedSignatureDBContext db
            )
        {
            this.db = db;
        }

        public object GetBy(long? id)
        {
            string result = "";
            var foundItem = db.Users
                .Include(t => t.UserRoles).ThenInclude(t => t.Role).ThenInclude(t => t.RoleActions)
                .FirstOrDefault(t => t.Id == id);

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundItem.IsSignature())
            {
                result += "change has been found in Users" + Environment.NewLine;
                result += foundItem.GetSignatureChanges();
            }

            if (foundItem.UserRoles != null)
            {
                foreach (var item in foundItem.UserRoles)
                {
                    if (!item.IsSignature())
                    {
                        result += "change has been found in UserRoles" + Environment.NewLine;
                        result += item.GetSignatureChanges();
                    }
                    if (item.Role != null)
                    {
                        if (!item.Role.IsSignature())
                        {
                            result += "change has been found in Role" + Environment.NewLine;
                            result += item.Role.GetSignatureChanges();
                        }
                        if (item.Role.RoleActions != null)
                        {
                            foreach (var action in item.Role.RoleActions)
                            {
                                if (!action.IsSignature())
                                {
                                    result += "change has been found in RoleActions" + Environment.NewLine;
                                    result += action.GetSignatureChanges();
                                }
                            }
                        }
                    }
                }
            }

            return new { description = string.IsNullOrEmpty(result) ? BMessages.Its_All_Valid.GetEnumDisplayName() : result };
        }

        public GridResultVM<UserMainGridResultVM> GetList(UserMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.Users
                .Include(t => t.UserRoles).ThenInclude(t => t.Role).ThenInclude(t => t.RoleActions)
                .Include(t => t.SiteSetting)
                .AsQueryable();

            if (searchInput.id.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Id == searchInput.id);
            if (!string.IsNullOrEmpty(searchInput.username))
                quiryResult = quiryResult.Where(t => t.Username.Contains(searchInput.username));
            if (!string.IsNullOrEmpty(searchInput.firstname))
                quiryResult = quiryResult.Where(t => t.Firstname.Contains(searchInput.firstname));
            if (!string.IsNullOrEmpty(searchInput.lastname))
                quiryResult = quiryResult.Where(t => t.Lastname.Contains(searchInput.lastname));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (searchInput.isDelete != null)
                quiryResult = quiryResult.Where(t => t.IsDelete == searchInput.isDelete);
            if (!string.IsNullOrEmpty(searchInput.nationalCode))
                quiryResult = quiryResult.Where(t => t.Nationalcode.Contains(searchInput.nationalCode));
            if (!string.IsNullOrEmpty(searchInput.website))
                quiryResult = quiryResult.Where(t => t.SiteSetting.Title.Contains(searchInput.website));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }

            bool hasSort = false;

            if (searchInput.sortField == "id" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.Id);
            }
            else if (searchInput.sortField == "id" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.Id);
            }
            else if (searchInput.sortField == "username" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.Username);
            }
            else if (searchInput.sortField == "username" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.Username);
            }
            else if (searchInput.sortField == "firstname" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.Firstname);
            }
            else if (searchInput.sortField == "firstname" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.Username);
            }
            else if (searchInput.sortField == "lastname" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.Lastname);
            }
            else if (searchInput.sortField == "lastname" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.Lastname);
            }
            else if (searchInput.sortField == "isActive" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.IsActive);
            }
            else if (searchInput.sortField == "isActive" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.IsActive);
            }
            else if (searchInput.sortField == "isDelete" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.IsDelete);
            }
            else if (searchInput.sortField == "isDelete" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.IsDelete);
            }
            else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.CreateDate);
            }
            else if (searchInput.sortField == "createDate" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);
            }
            else if (searchInput.sortField == "nationalCode" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.Nationalcode);
            }
            else if (searchInput.sortField == "nationalCode" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.Nationalcode);
            }
            else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.SiteSettingId);
            }
            else if (searchInput.sortField == "website" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.SiteSettingId);
            }

            if (hasSort == false)
                quiryResult = quiryResult.OrderByDescending(t => t.Id);

            int row = searchInput.skip;
            return new GridResultVM<UserMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                    .Skip(searchInput.skip)
                    .Take(searchInput.take)
                    .ToList()
                    .Select(t => new UserMainGridResultVM
                    {
                        createDate = t.CreateDate.ToFaDate(),
                        firstname = t.Firstname,
                        id = t.Id,
                        isActive = t.IsActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                        isDelete = t.IsDelete == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                        lastname = t.Lastname,
                        nationalCode = t.Nationalcode,
                        row = ++row,
                        username = t.Username,
                        website = t.SiteSetting != null ? t.SiteSetting.Title : "",
                        isValid =
                            t.IsSignature()
                            &&
                                (
                                    t.UserRoles == null ||
                                    t.UserRoles.Count == t.UserRoles.Count(
                                            tt =>
                                                tt.IsSignature()
                                                && (tt.Role == null || tt.Role.IsSignature())
                                                && (tt.Role == null || tt.Role.RoleActions == null || tt.Role.RoleActions.Count == tt.Role.RoleActions.Count(ttt => ttt.IsSignature()))
                                        )
                                )
                            ?
                            BMessages.Yes.GetEnumDisplayName()
                            :
                            BMessages.No.GetEnumDisplayName()
                    })
                    .ToList()
            };
        }
    }
}
