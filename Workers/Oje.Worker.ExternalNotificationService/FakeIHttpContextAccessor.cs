using Microsoft.AspNetCore.Http;

namespace Oje.Worker.ExternalNotificationService
{
    public class FakeIHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
