using Oje.Section.RegisterForm.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface ICompanyService
    {
        object GetLightList();
        string GetTitleById(int id);
        Company GetByUserId(long userId);
        Company GetByUserFilledRegisterFormId(long id);
    }
}
