
namespace Oje.Section.WebMain.Models.View
{
    public class PageManifestCreateUpdateVM
    {
        public long? id { get; set; }
        public long? pid { get; set; }
        public string title { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }
    }
}
