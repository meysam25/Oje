using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Models.View
{
    public class GlobalMetaTagCreateUpdateVM
    {
        [Display(Name = "تگ شماره 1")]
        [IgnoreStringEncode]
        public MyHtmlString tag1 { get; set; }
    }
}
