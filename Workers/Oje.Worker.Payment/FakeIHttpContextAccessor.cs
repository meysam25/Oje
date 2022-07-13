using Microsoft.AspNetCore.Http;

namespace Oje.Worker.Payment
{
    public class FakeIHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
