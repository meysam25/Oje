using Microsoft.AspNetCore.Http;

namespace Oje.Worker.ProposalFormReminder
{
    public class FakeIHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
