namespace Oje.Section.InquiryBaseData.Interfaces
{
    public interface IInsuranceContractService
    {
        object GetLightList(int? siteSettingId);
        bool Exist(int id, int? siteSettingId);
    }
}
