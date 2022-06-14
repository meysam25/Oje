using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models;

namespace Oje.AccountService.Models.View
{
    public class UserMessageCreateVM: GlobalGridParentLong
    {
        public long? userId { get; set; }
        public string message { get; set; }
        public IFormFile mainFile { get; set; }
    }
}
