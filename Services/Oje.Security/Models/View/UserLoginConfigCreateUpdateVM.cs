
namespace Oje.Security.Models.View
{
    public class UserLoginConfigCreateUpdateVM
    {
        public int? failCount { get; set; }
        public int? deactiveMinute { get; set; }
        public int? inActiveLogoffMinute { get; set; }
    }
}
