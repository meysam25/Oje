using Oje.Section.GlobalForms.Models.DB;

namespace Oje.Section.GlobalForms.Interfaces
{
    public interface IGeneralFormJsonService
    {
        void Create(long generalFormId, string jsonConfig);
        GeneralFormCacheJson GetCacheBy(long generalFilledFormId);
    }
}
