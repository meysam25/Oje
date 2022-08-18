using Oje.Sanab.Models.View;

namespace Oje.Sanab.Interfaces
{
    public interface IUserInquiry
    {
        Task<UserResultVM> GetUserInfo(int? siteSettingId, string NationalCode, string Mobile, string birthdate);
        Task<DriverLicenceResultVM> GetDriverLicence(int? siteSettingId, string LicenseNumber, string NationalId);
    }
}
