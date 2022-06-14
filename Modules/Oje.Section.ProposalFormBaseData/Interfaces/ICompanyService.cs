
namespace Oje.Section.ProposalFormBaseData.Interfaces
{
    public interface ICompanyService
    {
        object GetLightList();
        object GetLightList(long? userId);
    }
}
