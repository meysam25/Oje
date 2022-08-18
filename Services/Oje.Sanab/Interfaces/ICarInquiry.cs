using Oje.Sanab.Models.View;

namespace Oje.Sanab.Interfaces
{
    public interface ICarInquiry
    {
        Task<CarDiscountResultVM> CarDiscount(int? siteSettingId, string PlkSrl, string Plk1, string Plk2, string Plk3, string NationalCode);
    }
}
