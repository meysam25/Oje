using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Security.Models.View;

namespace Oje.Security.Interfaces
{
    public interface IErrorService
    {
        void Create(long? UserId, string requestId, ApiResultErrorCode? type, BMessages? bMessageCode, string cMessages, IpSections ip, string cLineNumbers, string cFilenames);
        object GetBy(string requestId);
        GridResultVM<ErrorMainGridResultVM> GetList(ErrorMainGrid searchInput);
    }
}
