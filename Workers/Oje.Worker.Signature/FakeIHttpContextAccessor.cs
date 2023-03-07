using Microsoft.AspNetCore.Http;

namespace Oje.Worker.Signature
{
    public class FakeIHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext { get => null; set => throw new NotImplementedException(); }
    }
}
