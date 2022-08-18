namespace Oje.Sanab.Interfaces
{
    public interface ICarTypeService
    {
        object GetLightList();
        bool Exist(int? ctId);
    }
}
