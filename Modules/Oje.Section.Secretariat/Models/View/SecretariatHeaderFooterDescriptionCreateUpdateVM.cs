using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatHeaderFooterDescriptionCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString header { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString footer { get; set; }
    }
}
