using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.AccountService.Models.View
{
    public class LoginDescrptionResultVM
    {
        [IgnoreStringEncode]
        public MyHtmlString desc { get; set; }
    }
}
