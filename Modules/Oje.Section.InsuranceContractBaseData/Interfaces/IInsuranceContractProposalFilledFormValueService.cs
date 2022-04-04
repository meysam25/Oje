using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models.PageForms;
using System;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractProposalFilledFormValueService
    {
        void CreateByJsonConfig(PageForm ppfObj, long insuranceContractProposalFilledFormId, IFormCollection form);
    }
}
