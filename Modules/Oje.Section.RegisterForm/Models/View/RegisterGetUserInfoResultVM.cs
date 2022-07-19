namespace Oje.Section.RegisterForm.Models.View
{
    public class RegisterGetUserInfoResultVM
    {
        public string licencExpireDate { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string tell { get; set; }
        public int? provinceId { get; set; }
        public int? cityId { get; set; }
        public string cityId_Title { get; set; }
        public string address { get; set; }
        public string firstNameLegal { get; set; }
        public string lastNameLegal { get; set; }
        public decimal? mapLatRecivePlace_lat { get; set; }
        public decimal? mapLonRecivePlace_lon { get; set; }
        public byte? mapZoomRecivePlace_zoom { get; set; }
    }
}
