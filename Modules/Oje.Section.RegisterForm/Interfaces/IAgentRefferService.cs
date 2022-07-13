namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IAgentRefferService
    {
        string GetRefferCode(int? siteSettingId, int? companyId);
    }
}
