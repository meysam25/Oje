using Oje.Section.RegisterForm.Models.DB;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface ICompanyService
    {
        object GetLightList();
        string GetTitleById(int id);
        Company GetByUserId(long userId);
        Company GetByUserFilledRegisterFormId(long id);
        Company GetById(int id);
    }
}
