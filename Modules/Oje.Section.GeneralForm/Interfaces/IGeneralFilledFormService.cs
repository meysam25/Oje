using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Models.PageForms;
using Oje.Section.GlobalForms.Models.DB;
using Oje.Section.GlobalForms.Models.View;
using System.Collections.Generic;

namespace Oje.Section.GlobalForms.Interfaces
{
    public interface IGeneralFilledFormService
    {
        object Create(int? siteSettingId, IFormCollection form, LoginUserVM loginUser);
        GeneralFilledFormPdfDetailesVM PdfDetailes(long id, int? siteSettingId, long? loginUserId, LoginUserVM adminLoginUser);
        (long formId, List<GeneralFormRequiredDocument> allRequiredFileUpload, long fistStatusId, PageForm ppfObj, List<ctrl> allCtrls, string jsonConfig)
            createValidation(int? siteSettingId, IFormCollection form, LoginUserVM loginUser);
        GeneralFilledFormPdfDetailesVM PdfDetailesByForm(IFormCollection form, int? siteSettingId);


        object Delete(long? id, int? siteSettingId, LoginUserVM loginUserVM);
        object GetList(GeneralFilledFormMainGrid searchInput, IFormCollection form, int? siteSettingId, LoginUserVM loginUserVM);
        object UpdateStatus(GeneralFilledFormUpdateStatusVM input, int? siteSettingId, long? userId, List<string> roles);
    }
}
