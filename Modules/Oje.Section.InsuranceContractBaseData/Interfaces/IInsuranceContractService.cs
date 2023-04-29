using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.DB;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System.Collections.Generic;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractService
    {
        ApiResult Create(CreateUpdateInsuranceContractVM input);
        ApiResult Delete(int? id);
        CreateUpdateInsuranceContractVM GetById(int? id);
        ApiResult Update(CreateUpdateInsuranceContractVM input);
        GridResultVM<InsuranceContractMainGridResultVM> GetList(InsuranceContractMainGrid searchInput);
        bool Exist(int id, int? siteSettingId, long? loginUserId);
        object GetLightList();
        ApiResult IsValid(contractUserInput input, int? siteSettingId, IpSections curIp);
        string GetFormJsonConfig(contractUserInput input, int? siteSettingId);
        InsuranceContract GetByCode(contractUserInput input, int? siteSettingId);
        ContractTermsInfo GetTermsInfo(contractUserInput input, int? siteSettingId);
        List<IdTitle> GetFamilyMemberList(contractUserInput input, int? siteSettingId);
        List<IdTitle> GetContractTypeList(contractUserInput input, int? siteSettingId);
        RequiredDocumentVM GetRequiredDocuments(contractUserInput input, int? insuranceContractTypeId, int? siteSettingId);
        IdTitle GetIdTitleBy(contractUserInput contractInfo, int? siteSettingId);
        string GetIdByCode(int? id, int? siteSettingId);
        bool Exist(int? id, int? siteSettingId);
        object GetLightList(int? siteSettingId);
        object IsValidSMS(contractUserInput input, int? curSiteSettingId, IpSections curIp);
        object ConfirmSMSForCreate(contractUserInput input, int? curSiteSettingId, IpSections curIp);
        int? GetIdByCode2(int? code, int? siteSettingId);
    }
}
