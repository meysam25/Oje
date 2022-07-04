using Oje.Sanab.Models.View;

namespace Oje.Sanab.Interfaces
{
    public interface IUserInquiry
    {
        Task<UserResultVM> GetUserInfo(string token, string NationalCode, string Mobile, string birthdate);
        Task<DriverLicenceResultVM> GetDriverLicence(string token, string LicenseNumber, string NationalId);
    }
}
