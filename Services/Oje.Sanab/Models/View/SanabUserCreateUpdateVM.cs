namespace Oje.Sanab.Models.View
{
    public class SanabUserCreateUpdateVM
    {
        public int? id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool? isActive { get; set; }
        public string token { get; set; }
    }
}
