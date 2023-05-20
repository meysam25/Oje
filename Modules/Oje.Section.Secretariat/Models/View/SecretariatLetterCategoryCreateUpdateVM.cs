namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatLetterCategoryCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string code { get; set; }
        public int? hfid { get; set; }
        public int? hfdid { get; set; }
        public bool? isActive { get; set; }
    }
}
