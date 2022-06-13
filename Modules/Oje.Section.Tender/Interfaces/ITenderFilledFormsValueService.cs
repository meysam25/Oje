using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models.PageForms;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderFilledFormsValueService
    {
        void CreateByForm(long tenderFilledForm, IFormCollection form, PageForm ppfObj, int? tenderProposalFormJsonConfigId);
    }
}
