using Oje.Infrastructure.Enums;

namespace Oje.AccountService.Models.View
{
    public class UpdateUserForUserVM
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string tell { get; set; }
        public string nationalCode { get; set; }
        public string postalCode { get; set; }
        public string address { get; set; }
        public string birthDate { get; set; }
        public decimal? mapLat { get; set; }
        public decimal? mapLon { get; set; }
        public byte? mapZoom { get; set; }
        public string fatherName { get; set; }
        public Gender? gender { get; set; }
        public string shenasnameNo { get; set; }
        public MarrageStatus? marrageStatus { get; set; }
    }
}
