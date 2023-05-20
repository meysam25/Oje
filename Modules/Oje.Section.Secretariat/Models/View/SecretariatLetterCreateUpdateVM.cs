using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.Secretariat.Models.View
{
    public class SecretariatLetterCreateUpdateVM
    {
        public long? id { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public string subject { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString description { get; set; }
        public int? cId { get; set; }
        public int? sId { get; set; }
        public string mobile { get; set; }
    }
}
