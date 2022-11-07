using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.GlobalForms.Models.View
{
    public class GeneralFormCreateUpdateVM
    {
        public long? id { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString jsonStr { get; set; }
        public string description { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString termT { get; set; }
        public bool? isActive { get; set; }
        public int? siteSettingId { get; set; }
        public IFormFile rules { get; set; }
        public string rules_address { get; set; }
        public IFormFile conteractFile { get; set; }
        public string conteractFile_address { get; set; }
    }
}
