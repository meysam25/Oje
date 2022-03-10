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
    public class UserFilledRegisterFormKeyService: IUserFilledRegisterFormKeyService
    {
        readonly RegisterFormDBContext db = null;
        public UserFilledRegisterFormKeyService
            (
                RegisterFormDBContext db
            )
        {
            this.db = db;
        }

        public long CreateIfNeeded(string name)
        {
            if (string.IsNullOrEmpty(name))
                return 0;
            long result = db.UserFilledRegisterFormKeys.Where(t => t.Key == name).Select(t => t.Id).FirstOrDefault();

            if (result > 0)
                return result;

            var newItem = new UserFilledRegisterFormKey() { Key = name };
            db.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();

            return newItem.Id;
        }
    }
}
