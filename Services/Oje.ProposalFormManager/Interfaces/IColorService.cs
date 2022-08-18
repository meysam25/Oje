using Oje.ProposalFormService.Models.DB;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IColorService
    {
        object GetLightList();
        Color GetById(int id);
    }
}
