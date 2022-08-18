namespace Oje.Sanab.Interfaces
{
    public interface ICompanyService
    {
        object GetLightList();
        bool Exist(int id);
    }
}
