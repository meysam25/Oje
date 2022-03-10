using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IUserFilledRegisterFormJsonService
    {
        void Create(string foundJsonFormStr, long UserFilledRegisterFormId);
        string GetBy(long id);
    }
}
