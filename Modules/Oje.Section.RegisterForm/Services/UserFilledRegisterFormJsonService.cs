using Microsoft.EntityFrameworkCore;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Services.EContext;
using System.Linq;

namespace Oje.Section.RegisterForm.Services
{
    public class UserFilledRegisterFormJsonService : IUserFilledRegisterFormJsonService
    {
        readonly RegisterFormDBContext db = null;
        public UserFilledRegisterFormJsonService(RegisterFormDBContext db)
        {
            this.db = db;
        }

        public void Create(string foundJsonFormStr, long UserFilledRegisterFormId)
        {
            db.Entry(new UserFilledRegisterFormJson()
            {
                JsonConfig = foundJsonFormStr,
                UserFilledRegisterFormId = UserFilledRegisterFormId
            }).State = EntityState.Added;
            db.SaveChanges();
        }

        public string GetBy(long id)
        {
            return db.UserFilledRegisterFormJsons.Where(t => t.UserFilledRegisterFormId == id).Select(t => t.JsonConfig).FirstOrDefault();
        }
    }
}
