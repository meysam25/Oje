using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class RoleService: IRoleService
    {
        readonly ProposalFormDBContext db = null;
        public RoleService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public void AddUserToUserRole(long userId)
        {
            var foundRole = db.Roles.Where(t => t.Name == "user").FirstOrDefault();
            if (foundRole == null)
                throw BException.GenerateNewException(BMessages.Role_Is_Not_Valid);

            if(!db.UserRoles.Any(t => t.UserId == userId && t.RoleId == foundRole.Id))
            {
                var newUserRole = new UserRole() { UserId = userId, RoleId = foundRole.Id };
                newUserRole.FilledSignature();
                db.Entry(newUserRole).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.SaveChanges();
            }

        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.Roles.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
