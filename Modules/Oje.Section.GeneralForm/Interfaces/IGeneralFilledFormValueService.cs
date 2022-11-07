using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models.PageForms;
using System.Collections.Generic;

namespace Oje.Section.GlobalForms.Interfaces
{
    public interface IGeneralFilledFormValueService
    {
        void CreateByJsonConfig(PageForm ppfObj, long generalFilledFormId, IFormCollection form, List<ctrl> allCtrls);
    }
}
