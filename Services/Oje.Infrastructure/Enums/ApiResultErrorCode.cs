
namespace Oje.Infrastructure.Enums
{
    public enum ApiResultErrorCode
    {
        InvalidUserOrPassword = 2,
        ValidationError = 3,
        InActiveUser = 4,
        InvalidCaptcha = 5,
        NeedLoginFist = 6,
        NotFound = 7,
        UnauthorizeAccess = 403,
        UnknownError = 500,
    }
}
