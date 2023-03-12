using Oje.Infrastructure.Models;

namespace Oje.ValidatedSignature.Models.View
{
    public class UserMainGrid: GlobalGrid
    {
        public long? id {  get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public bool? isActive { get; set; }
        public bool? isDelete { get; set; }
        public string createDate { get; set; }
        public string nationalCode { get; set; }
        public string website { get; set; }
    }
}
