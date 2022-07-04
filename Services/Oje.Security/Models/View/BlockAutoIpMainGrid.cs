using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.Security.Models.View
{
    public class BlockAutoIpMainGrid: GlobalGrid
    {
        public string ip { get; set; }
        public string createDate { get; set; }
        public string fullUsername { get; set; }
        public BlockClientConfigType? section { get; set; }
        public bool? isSuccess { get; set; }
    }
}
