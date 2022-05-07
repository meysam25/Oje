using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Section.InsuranceContractBaseData.Models.View;

namespace Oje.Section.InsuranceContractBaseData.Interfaces
{
    public interface IInsuranceContractUserService
    {
        ApiResult Create(CreateUpdateInsuranceContractUserVM input, InsuranceContractUserStatus status);
        ApiResult Delete(long? id, InsuranceContractUserStatus status);
        CreateUpdateInsuranceContractUserVM GetById(long? id, InsuranceContractUserStatus status);
        ApiResult Update(CreateUpdateInsuranceContractUserVM input, InsuranceContractUserStatus status);
        GridResultVM<InsuranceContractUserMainGridResultVM> GetList(InsuranceContractUserMainGrid searchInput, InsuranceContractUserStatus status);
        ApiResult ChangeStatus(long? id, InsuranceContractUserStatus fromStatus, InsuranceContractUserStatus toStatus);
        ApiResult CreateFromExcel(GlobalExcelFile input, InsuranceContractUserStatus status, int? siteSettingId);
        ApiResult CreateFromExcelChild(GlobalExcelFile input, InsuranceContractUserStatus status, int? siteSettingId);
    }
}
