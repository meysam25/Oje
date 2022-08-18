namespace Oje.Sanab.Interfaces
{
    public interface IVehicleTypeService
    {
        object GetLightList();
        bool Exist(int id);
    }
}
