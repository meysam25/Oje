using Oje.Infrastructure.Exceptions;
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
    public class RoleManager: IRoleManager
    {
        readonly ProposalFormDBContext db = null;
        public RoleManager(ProposalFormDBContext db)
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
                db.Entry(new UserRole() { UserId = userId, RoleId = foundRole.Id }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.SaveChanges();
            }

        }
    }
}
