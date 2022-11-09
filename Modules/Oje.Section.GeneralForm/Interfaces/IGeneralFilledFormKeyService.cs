using Oje.Infrastructure.Models;

namespace Oje.Section.GlobalForms.Interfaces
{
    public interface IGeneralFilledFormKeyService
    {
        int CreateIfNeeded(string name, string title);
        object GetSelect2List(Select2SearchVM searchInput, long? generalFormId);
    }
}
