
namespace Oje.Section.BaseData.Models.View
{
    public class CreateUpdateProvinceVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
        public decimal? mapLat { get; set; }
        public decimal? mapLon { get; set; }
        public byte? mapZoom { get; set; }
    }
}
