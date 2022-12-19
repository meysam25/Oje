using Oje.Infrastructure.Filters;
using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Models
{
    public class GlobalMetaTagCreateUpdateVM
    {
        [Display(Name = "تگ شماره 1")]
        [IgnoreStringEncode]
        public MyHtmlString tag1 { get; set; }
    }
}
