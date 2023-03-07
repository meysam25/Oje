using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.ValidatedSignature.Models.View
{
    public class UploadedFileMainGrid: GlobalGrid
    {
        public long? id { get; set; }
        public FileType? ft { get; set; }
        public string user { get; set; }
        public bool? rAccess { get; set; }
        public string fct { get; set; }
        public long? fs { get; set; }
        public string website { get; set; }
    }
}
