namespace Oje.Section.Tender.Interfaces
{
    public interface ICompanyService
    {
        object GetLightList(long? userId);
        object GetLightList();
        object GetLightListString();
    }
}
