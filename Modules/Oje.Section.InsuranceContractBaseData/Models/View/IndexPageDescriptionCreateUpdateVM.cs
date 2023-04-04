using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class IndexPageDescriptionCreateUpdateVM
    {
        [Display(Name = "توضیحات")]
        [IgnoreStringEncode]
        public MyHtmlString desctpion { get; set; }
        [Display(Name = "تصویر اصلی")]
        public IFormFile mainImage { get; set; }
    }
}
