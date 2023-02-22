using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.PageForms;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderFilledFormsValueService
    {
        void CreateByForm(long tenderFilledForm, IFormCollection form, PageForm ppfObj, int? tenderProposalFormJsonConfigId, long? loginUserId = null, bool? isConsultation = null);
        void DeleteConsultValues(long filledFormId, int jsonFormId);
        object GetValues(long filledFormId, bool isConsultationv, int jsonFormId);
    }
}
