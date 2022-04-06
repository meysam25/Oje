using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System.Collections.Generic;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IMyFilledContractService
    {
        object GetList(MyFilledContractMainGrid searchInput, long? loginUserId, int? siteSettingId, List<InsuranceContractProposalFilledFormType> status);
        object GetPPFImageList(GlobalGridParentLong input, int? siteSettingId, long? loginUserId, List<InsuranceContractProposalFilledFormType> status);
        object UploadImage(long? insuranceContractProposalFilledFormId, IFormFile mainFile, int? siteSettingId, long? loginUserId, List<InsuranceContractProposalFilledFormType> status);
    }
}
