using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;

namespace Oje.Security.Models.View
{
    public class ErrorMainGrid: GlobalGrid
    {
        public string userFullname { get; set; }
        public string ip { get; set; }
        public string createDate { get; set; }
        public string description { get; set; }
        public string lineNumber { get; set; }
        public string fileName { get; set; }
        public BMessages? bMessageCode { get; set; }
        public ApiResultErrorCode? type { get; set; }
    }
}
